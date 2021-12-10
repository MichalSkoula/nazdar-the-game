using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace MyGame.Screens
{
    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public MenuScreen(Game1 game) : base(game) { }
        private Button startButton;

        public override void Initialize()
        {
            startButton = new Button(10, 100, 150, 60, "Start");
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // exit game from menu
                Game.Exit();
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.LeftAlt))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 640;
                Game.graphics.PreferredBackBufferHeight = 360;
                Game.graphics.ApplyChanges();
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.LeftControl))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 1280;
                Game.graphics.PreferredBackBufferHeight = 720;
                Game.graphics.ApplyChanges();
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
            {
                // to game
                Game.LoadScreen1();
            }

            startButton.Update();
            if (startButton.HasBeenClicked())
            {
                // to game
                Game.LoadScreen1();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart();

            Game.SpriteBatch.DrawString(Assets.font, "MENU; press enter or click button to start; press ctrl for 720p; press alt for 360p", Vector2.Zero, Color.White);
            startButton.Draw(Game.SpriteBatch);

            Game.DrawEnd();
        }
    }
}
