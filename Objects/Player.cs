using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Objects
{
    class Player
    {
        private float posX = 0;
        private int speed = 1000;

        public void Update(float deltaTime)
        {
            if (Controls.Keyboard.IsPressed(Keys.Right))
            {
                posX += deltaTime * speed;
            }
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Assets.player, new Vector2(posX, 20), Color.White);
        }
    }
}
