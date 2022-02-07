namespace MyGame.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using MonoGame.Extended.Screens;
    using MyGame.Controls;
    using static MyGame.Enums;

    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MenuScreen(Game1 game) : base(game) { }

        Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            // add buttons
            buttons.Add("startButton1", new Button(20, 60, null, ButtonSize.Large, "Slot #1", true));
            buttons.Add("startButton2", new Button(20, 100, null, ButtonSize.Large, "Slot #2"));
            buttons.Add("startButton3", new Button(20, 140, null, ButtonSize.Large, "Slot #3"));
            buttons.Add("fullscreenButton", new Button(20, 240, null, ButtonSize.Medium, "Toggle Fullscreen"));
            buttons.Add("exitButton", new Button(20, 280, null, ButtonSize.Medium, "Exit"));

            // play song
            MediaPlayer.Play(Assets.Nature);

            // load and apply settings from json file
            // we load this also in splashscreen
            Settings.LoadSettings(Game);

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

            // fullscreen
            if (Controls.Keyboard.HasBeenPressed(Keys.F) || this.buttons.GetValueOrDefault("fullscreenButton").HasBeenClicked())
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();

                Settings.SaveSettings(Game);
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
    }
}
