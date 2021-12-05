using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace ScreenTest.Screens
{
    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public MenuScreen(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.HasBeenPressed(Keys.Escape))
            {
                // exit game from menu
                Game.Exit();
            }
            else if (Keyboard.HasBeenPressed(Keys.LeftAlt))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 640;
                Game.graphics.PreferredBackBufferHeight = 360;
                Game.graphics.ApplyChanges();
            }
            else if (Keyboard.HasBeenPressed(Keys.LeftControl))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 1280;
                Game.graphics.PreferredBackBufferHeight = 720;
                Game.graphics.ApplyChanges();
            }
            else if (Keyboard.HasBeenPressed(Keys.Enter))
            {
                // to game
                Game.LoadScreen1();
            }

            if (Mouse.HasBeenPressed(true))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 640;
                Game.graphics.PreferredBackBufferHeight = 360;
                Game.graphics.ApplyChanges();
            }
            else if (Mouse.HasBeenPressed(false))
            {
                // change resolution
                Game.graphics.PreferredBackBufferWidth = 1280;
                Game.graphics.PreferredBackBufferHeight = 720;
                Game.graphics.ApplyChanges();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart();

            Game.SpriteBatch.DrawString(Assets.font, "MENU press enter to start; press ctrl for 720p; press alt for 360p", Vector2.Zero, Color.Blue);

            Game.DrawEnd();
        }
    }
}
