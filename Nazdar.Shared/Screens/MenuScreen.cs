using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MenuScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private FileIO settingsFile = new FileIO("settings.json");

        public override void Initialize()
        {
            // add buttons
            buttons.Add("startButton1", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Slot #1", true));
            buttons.Add("startButton2", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "Slot #2"));
            buttons.Add("startButton3", new Button(Offset.MenuX, 140, null, ButtonSize.Large, "Slot #3"));
            buttons.Add("controlsButton", new Button(Offset.MenuX, 190, null, ButtonSize.Medium, "Controls"));
            buttons.Add("fullscreenButton", new Button(Offset.MenuX, 220, null, ButtonSize.Medium, "Toggle Fullscreen"));
            buttons.Add("musicButton", new Button(Offset.MenuX, 250, null, ButtonSize.Medium, "Toggle Music"));
            buttons.Add("soundsButton", new Button(Offset.MenuX, 280, null, ButtonSize.Medium, "Toggle Sounds"));
            buttons.Add("exitButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Exit"));

#if DEBUG
            buttons.Add("openFolderButton", new Button(Offset.MenuX + 560, Offset.MenuFooter - 20, null, ButtonSize.Small, "open"));
#endif

            // load settings
            this.LoadSettings();

            // load saves to show info next to slot button
            string[] slots = new[] { Save.Slot1, Save.Slot2, Save.Slot3 };
            for (int i = 0; i < 3; i++)
            {
                FileIO saveFile = new FileIO(slots[i]);
                dynamic saveData = saveFile.Load();
                this.buttons.GetValueOrDefault("startButton" + (i + 1)).Data = Tools.ParseSaveData(saveData);
            }

            Audio.SongTransition(0.25f, "Menu");

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // update buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Update();
            }

            // iterate through buttons up/down
            if (Controls.Keyboard.HasBeenPressed(Keys.Down) || Controls.Gamepad.HasBeenPressed(Buttons.DPadDown) || Controls.Gamepad.HasBeenPressedThumbstick(Direction.Down))
            {
                Tools.ButtonsIterateWithKeys(Direction.Down, this.buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up) || Controls.Gamepad.HasBeenPressed(Buttons.DPadUp) || Controls.Gamepad.HasBeenPressedThumbstick(Direction.Up))
            {
                Tools.ButtonsIterateWithKeys(Direction.Up, this.buttons);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter) || Controls.Gamepad.HasBeenPressed(Buttons.A))
            {
                foreach (KeyValuePair<string, Button> button in this.buttons)
                {
                    if (button.Value.Focus)
                    {
                        button.Value.Clicked = true;
                        break;
                    }
                }
            }

            // start game
            if (this.buttons.GetValueOrDefault("startButton1").HasBeenClicked())
            {
                this.Game.SaveSlot = Save.Slot1;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
            else if (this.buttons.GetValueOrDefault("startButton2").HasBeenClicked())
            {
                this.Game.SaveSlot = Save.Slot2;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
            else if (this.buttons.GetValueOrDefault("startButton3").HasBeenClicked())
            {
                this.Game.SaveSlot = Save.Slot3;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // settings - fullscreen
            if (Controls.Keyboard.HasBeenPressed(Keys.F) || Controls.Keyboard.HasBeenPressed(Keys.F11) || this.buttons.GetValueOrDefault("fullscreenButton").HasBeenClicked())
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();
                this.SaveSettings();
            }

            // settings - music
            if (Controls.Keyboard.HasBeenPressed(Keys.M) || this.buttons.GetValueOrDefault("musicButton").HasBeenClicked())
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                this.SaveSettings();
            }

            // settings - sounds
            if (Controls.Keyboard.HasBeenPressed(Keys.S) || this.buttons.GetValueOrDefault("soundsButton").HasBeenClicked())
            {
                if (SoundEffect.MasterVolume == 0)
                {
                    SoundEffect.MasterVolume = 1;
                    Audio.PlayRandomSound("Jumps");
                }
                else
                {
                    SoundEffect.MasterVolume = 0;
                }
                this.SaveSettings();
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

            // open save folder (only if DEBUG)
#if DEBUG
            if (this.buttons.GetValueOrDefault("openFolderButton").HasBeenClicked())
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = this.settingsFile.GetFolder(),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
#endif
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Start the game", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

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
                        button.Value.Data[0],
                        new Vector2(2 * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y),
                        MyColor.White);
                    this.Game.SpriteBatch.DrawString(
                        Assets.Fonts["Small"],
                        button.Value.Data[1],
                        new Vector2(2 * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y + 10),
                        MyColor.White);
                    this.Game.SpriteBatch.DrawString(
                        Assets.Fonts["Small"],
                        button.Value.Data[2],
                        new Vector2(2 * Offset.MenuX + button.Value.Hitbox.Width, button.Value.Hitbox.Y + 20),
                        MyColor.White);
                }
                i++;
            }

            // version
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "v" + Version.Number, new Vector2(Offset.MenuX, Offset.MenuFooter), MyColor.Gray1);
#if DEBUG
            // save path
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], this.settingsFile.GetPath(), new Vector2(Offset.MenuX + 40, Offset.MenuFooter), MyColor.Gray1);
#endif
            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            this.Game.DrawEnd();
        }

        private void SaveSettings()
        {
            this.settingsFile.Save(new
            {
                fullscreen = this.Game.Graphics.IsFullScreen,
                musicMuted = MediaPlayer.IsMuted,
                soundsVolume = SoundEffect.MasterVolume,
            });
        }

        private void LoadSettings()
        {
            dynamic settings = this.settingsFile.Load();
            if (settings == null)
            {
                return;
            }

            if (settings.ContainsKey("fullscreen") && this.Game.Graphics.IsFullScreen != (bool)settings.fullscreen)
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();
            }
            else
            {
                // default values differs on debug mode
#if DEBUG
                this.Game.Graphics.IsFullScreen = false;
#else
                this.Game.Graphics.IsFullScreen = true;
#endif
                this.Game.Graphics.ApplyChanges();
            }

            // music
            if (settings.ContainsKey("musicMuted"))
            {
                MediaPlayer.IsMuted = (bool)settings.musicMuted;
            }
            else
            {
                // default on
                MediaPlayer.IsMuted = false;
            }

            // sounds
            if (settings.ContainsKey("soundsVolume"))
            {
                SoundEffect.MasterVolume = (int)settings.soundsVolume;
            }
            else
            {
                // default on
                SoundEffect.MasterVolume = 1;
            }
        }
    }
}
