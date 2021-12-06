using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MyGame.Objects;

namespace MyGame.Screens
{
    public class Screen1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Screen1(Game1 game) : base(game) { }

        private Player player = new Player();

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // back to menu
                Game.LoadMenuScreen();
            }
            
            player.Update(Game.deltaTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart();

            Game.SpriteBatch.DrawString(Assets.font, "this is game you can escape to menu", Vector2.Zero, Color.White);
            player.Draw(Game.SpriteBatch);

            Game.DrawEnd();
        }
    }
}
