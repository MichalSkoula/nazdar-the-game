using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Controls;
using static MyGame.Enums;

namespace MyGame.Screens
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
            buttons.Add("startButton1", new Button(20, 60, null, ButtonSize.Large, "Slot #1", true));
            buttons.Add("startButton2", new Button(20, 100, null, ButtonSize.Large, "Slot #2"));
            buttons.Add("startButton3", new Button(20, 140, null, ButtonSize.Large, "Slot #3"));
            buttons.Add("fullscreenButton", new Button(20, 250, null, ButtonSize.Small, "Toggle Fullscreen"));
            buttons.Add("musicButton", new Button(20, 270, null, ButtonSize.Small, "Toggle Music"));
            buttons.Add("soundsButton", new Button(20, 290, null, ButtonSize.Small, "Toggle Sounds"));
            buttons.Add("exitButton", new Button(20, 310, null, ButtonSize.Small, "Exit"));

            // load settings
            this.LoadSettings();

            // play song
            MediaPlayer.Play(Assets.Nature);

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
            if (Controls.Keyboard.HasBeenPressed(Keys.Down))
            {
                this.ButtonsIterateWithKeys(Direction.Down);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up))
            {
                this.ButtonsIterateWithKeys(Direction.Up);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
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
                this.Game.Slot = 1;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            } 
            else if (this.buttons.GetValueOrDefault("startButton2").HasBeenClicked())
            {
                this.Game.Slot = 2;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            } 
            else if (this.buttons.GetValueOrDefault("startButton3").HasBeenClicked())
            {
                this.Game.Slot = 3;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // settings - fullscreen
            if (Controls.Keyboard.HasBeenPressed(Keys.F) || this.buttons.GetValueOrDefault("fullscreenButton").HasBeenClicked())
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
                    Assets.Blip.Play();
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
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "Start the game", new Vector2(20, 20), Color.White);
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }
            this.Game.SpriteBatch.DrawString(Assets.FontSmall, this.settingsFile.GetPath(), new Vector2(20, 340), Color.White);

            this.Game.DrawEnd();
        }

        private void ButtonsIterateWithKeys(Direction direction)
        {
            bool focusNext = false;
            int i = 0;
            foreach (KeyValuePair<string, Button> button in (direction == Direction.Up) ? this.buttons.Reverse() : this.buttons)
            {
                if (focusNext)
                {
                    button.Value.Focus = true;
                    focusNext = false;
                    break;
                }

                if (button.Value.Focus && i < this.buttons.Count - 1)
                {
                    button.Value.Focus = false;
                    focusNext = true;
                }

                i++;
            }
        }

        private void SaveSettings()
        {
            this.settingsFile.Save(new {
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

            if (settings.ContainsKey("musicMuted"))
            {
                MediaPlayer.IsMuted = (bool)settings.musicMuted;
            }

            if (settings.ContainsKey("soundsVolume"))
            {
                SoundEffect.MasterVolume = (int)settings.soundsVolume;
            }
        }
    }
}
