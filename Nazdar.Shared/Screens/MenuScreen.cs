using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MenuScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            // load saves & add big start buttons
            string[] slots = new[] { Save.Slot1, Save.Slot2, Save.Slot3, Save.Survival };
            for (int i = 0; i < 4; i++)
            {
                FileIO saveFile = new FileIO(slots[i]);
                dynamic saveData = saveFile.Load();
                string[] data = Tools.ParseSaveData(saveData);

                // create button
                string buttonText = data[0] == " " ? Translation.Get("menu.newGame") : data[0];
                if (i == 3)
                {
                    buttonText = Translation.Get("menu.survival");
                }

                buttons.Add(
                    "startButton" + (i + 1),
                    new Button(
                        Offset.MenuX,
                        55 + i * 35,
                        null,
                        ButtonSize.Large,
                        buttonText,
                        i == 0,
                        data
                    )
                );
            }

            // add other buttons
            var controlsButton = new Button(Offset.MenuX, 225 + 0 * 27, null, ButtonSize.Medium, Translation.Get("menu.controls"));
            buttons.Add("controlsButton", controlsButton);
            var creditsButton = new Button(Offset.MenuX + controlsButton.Hitbox.Width + 10, 225 + 0 * 27, null, ButtonSize.Medium, Translation.Get("menu.credits"));
            buttons.Add("creditsButton", creditsButton);

            var languageButton = new Button(Offset.MenuX + controlsButton.Hitbox.Width + 10 + creditsButton.Hitbox.Width + 10, 225 + 0 * 27, null, ButtonSize.Medium, Translation.Get("menu.language"), text: Translation.GetLanguageName(Translation.CurrentLanguage));
            buttons.Add("languageButton", languageButton);

            // another row
            var musicButton = new Button(Offset.MenuX, 225 + 1 * 27, null, ButtonSize.Medium, Translation.Get("menu.music"), text: Translation.Get("menu.off"));
            buttons.Add("musicButton", musicButton);
            buttons.Add("soundsButton", new Button(Offset.MenuX + musicButton.Hitbox.Width + 10, 225 + 1 * 27, null, ButtonSize.Medium, Translation.Get("menu.sounds"), text: Translation.Get("menu.off")));

            var vibrationsButton = new Button(Offset.MenuX, 225 + 2 * 27, null, ButtonSize.Medium, Translation.Get("menu.vibrations"), text: Translation.Get("menu.off"));
            buttons.Add("vibrationsButton", vibrationsButton);

            // fullscreen - only on desktop = GL
            if (Game1.CurrentPlatform == Platform.GL)
            {
                buttons.Add("fullscreenButton", new Button(Offset.MenuX + vibrationsButton.Hitbox.Width + 10, 225 + 2 * 27, null, ButtonSize.Medium, Translation.Get("menu.fullscreen"), text: Translation.Get("menu.off")));
            }
            
            buttons.Add("exitButton", new Button(Offset.MenuX, 160 + 6 * 27, null, ButtonSize.Medium, Translation.Get("menu.exit")));
#if DEBUG
            if (Game1.CurrentPlatform != Platform.Android)
            {
                buttons.Add("openFolderButton", new Button(Offset.MenuX + 500, Offset.MenuFooter, null, ButtonSize.Small, "saves"));
            }
#endif

            Audio.SongTransition(0.25f, "Menu");

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Button.UpdateButtons(this.buttons);

            // start game
            if (this.buttons.GetValueOrDefault("startButton1").HasBeenClicked())
            {
                Audio.PlaySound("Story");
                this.Game.SaveSlot = Save.Slot1;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
            else if (this.buttons.GetValueOrDefault("startButton2").HasBeenClicked())
            {
                Audio.PlaySound("Story");
                this.Game.SaveSlot = Save.Slot2;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
            else if (this.buttons.GetValueOrDefault("startButton3").HasBeenClicked())
            {
                Audio.PlaySound("Story");
                this.Game.SaveSlot = Save.Slot3;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
            else if (this.buttons.GetValueOrDefault("startButton4").HasBeenClicked())
            {
                Audio.PlaySound("Survival");
                this.Game.SaveSlot = Save.Survival;
                this.Game.LoadScreen(typeof(Screens.SurvivalScreen));
            }

            // settings - fullscreen
            if (this.buttons.ContainsKey("fullscreenButton"))
            {
                if (Controls.Keyboard.HasBeenPressed(Keys.F) || Controls.Keyboard.HasBeenPressed(Keys.F11) || this.buttons.GetValueOrDefault("fullscreenButton").HasBeenClicked())
                {
                    this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                    this.Game.Graphics.ApplyChanges();
                    Settings.SaveSettings(Game);
                }

                this.buttons.GetValueOrDefault("fullscreenButton").Text = this.Game.Graphics.IsFullScreen ? Translation.Get("menu.on") : Translation.Get("menu.off");
            }

            // settings - vibrations
            if (this.buttons.ContainsKey("vibrationsButton"))
            {
                if (this.buttons.GetValueOrDefault("vibrationsButton").HasBeenClicked())
                {
                    Game1.Vibrations = !Game1.Vibrations;
                    Settings.SaveSettings(Game);
                }

                this.buttons.GetValueOrDefault("vibrationsButton").Text = Game1.Vibrations ? Translation.Get("menu.on") : Translation.Get("menu.off");
            }

            // settings - music
            if (Controls.Keyboard.HasBeenPressed(Keys.M) || this.buttons.GetValueOrDefault("musicButton").HasBeenClicked())
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                Settings.SaveSettings(Game);
            }
            this.buttons.GetValueOrDefault("musicButton").Text = MediaPlayer.IsMuted ? Translation.Get("menu.off") : Translation.Get("menu.on");

            // settings - sounds
            if (Controls.Keyboard.HasBeenPressed(Keys.S) || this.buttons.GetValueOrDefault("soundsButton").HasBeenClicked())
            {
                if (SoundEffect.MasterVolume == 0)
                {
                    SoundEffect.MasterVolume = 1;
                }
                else
                {
                    SoundEffect.MasterVolume = 0;
                }
                Settings.SaveSettings(Game);
            }
            this.buttons.GetValueOrDefault("soundsButton").Text = SoundEffect.MasterVolume == 1 ? Translation.Get("menu.on") : Translation.Get("menu.off");

            // settings - language
            if (this.buttons.ContainsKey("languageButton") && this.buttons.GetValueOrDefault("languageButton").HasBeenClicked())
            {
                // Toggle between English and Czech
                Translation.CurrentLanguage = Translation.CurrentLanguage == "en" ? "cs" : "en";
                Settings.SaveSettings(Game);
                
                // Reload menu to apply translations
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
                return;
            }

            if (this.buttons.ContainsKey("languageButton"))
            {
                this.buttons.GetValueOrDefault("languageButton").Text = Translation.GetLanguageName(Translation.CurrentLanguage);
            }

            // exit game from menu
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape) || this.buttons.GetValueOrDefault("exitButton").HasBeenClicked())
            {
                this.Game.Exit();
            }

            // back to splash screen
            if (this.buttons.GetValueOrDefault("controlsButton").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.ControlsScreen));
            }

            // credits
            if (this.buttons.GetValueOrDefault("creditsButton").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.CreditsScreen));
            }

            // open save folder (only if DEBUG)
            if (this.buttons.ContainsKey("openFolderButton") && this.buttons.GetValueOrDefault("openFolderButton").HasBeenClicked())
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Settings.GetPath(),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "NAZDAR!", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            int i = 0;
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                // draw button
                button.Value.Draw(this.Game.SpriteBatch);

                // draw save slot content next to button
                if (button.Value.Data != null && button.Value.Data.Length > 0)
                {
                    this.Game.SpriteBatch.DrawString(
                        Assets.Fonts["Small"],
                        button.Value.Data[1],
                        new Vector2(1.5f * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y),
                        MyColor.White);
                    this.Game.SpriteBatch.DrawString(
                        Assets.Fonts["Small"],
                        button.Value.Data[2],
                        new Vector2(1.5f * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y + 11),
                        MyColor.White);
                    this.Game.SpriteBatch.DrawString(
                        Assets.Fonts["Small"],
                        button.Value.Data[3],
                        new Vector2(1.5f * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y + 22),
                        MyColor.White);
                }
                i++;
            }

            // version
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "v" + Version.Number, new Vector2(Offset.MenuX + 565, Offset.MenuFooter), MyColor.Gray1);
#if DEBUG
            // save path
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], Settings.GetPath(), new Vector2(Offset.MenuX, Offset.MenuFooter + 10), MyColor.Gray2);
#endif
            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
