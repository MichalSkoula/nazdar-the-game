using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyGame.Controls;
using MyGame.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using static MyGame.Enums;

namespace MyGame.Screens
{
    class GameOverScreenNewGame : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public GameOverScreenNewGame(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            buttons.Add("yes", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Yes", true));
            buttons.Add("no", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "No"));

            base.Initialize();
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
                Tools.ButtonsIterateWithKeys(Direction.Down, this.buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up))
            {
                Tools.ButtonsIterateWithKeys(Direction.Up, this.buttons);
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

            // main menu - NO
            if (this.buttons.GetValueOrDefault("no").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                this.Game.LoadScreen(typeof(Screens.GameOverScreen));
            }

            // new game -YES
            if (this.buttons.GetValueOrDefault("yes").HasBeenClicked())
            {
                // delete save
                FileIO saveFile = new FileIO(Game.SaveSlot);
                saveFile.Delete();

                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "Start a new game?", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}
