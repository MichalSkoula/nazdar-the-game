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

        private Camera _camera = new Camera();

        private Player _player = new Player(Assets.player, 50, 50);

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // back to menu
                Game.LoadMenuScreen();
            }

            _player.Update(Game.deltaTime);

            _camera.Follow(_player);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart(_camera.Transform);

            Game.SpriteBatch.Draw(Assets.background, Vector2.Zero, Color.White); // background
            Game.SpriteBatch.DrawString(Assets.fontLarge, "this is game you can escape to menu", new Vector2(0 - _camera.Transform.Translation.X, 0 - _camera.Transform.Translation.Y), Color.White); // minus translation to stay in place
            _player.Draw(Game.SpriteBatch); // camera follows

            Game.DrawEnd();
        }
    }
}
