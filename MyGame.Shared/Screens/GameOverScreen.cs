using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Controls;

namespace MyGame.Screens
{
    public class GameOverScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public GameOverScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Keyboard.HasBeenPressed(Keys.Enter) || Controls.Mouse.HasBeenPressed())
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "GAME OVER", new Vector2(20, 140), Color.White);

            this.Game.DrawEnd();
        }
    }
}
