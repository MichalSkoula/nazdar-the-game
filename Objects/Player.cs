using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ScreenTest.Objects
{
    class Player
    {
        private float posX = 0;
        private int speed = 1000;

        public void Update(float deltaTime)
        {
            if (Keyboard.IsPressed(Keys.Right))
            {
                posX += deltaTime * speed;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Assets.player, new Vector2(posX, 20), Color.White);
        }
    }
}
