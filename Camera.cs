using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Objects.Player target)
        {
            Matrix offset = Matrix.CreateTranslation(
                -target.Hitbox.X - (target.Hitbox.Width / 2),
                0,
                0
            );
                
            Matrix position = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                0,
                0
            );

            Transform = position * offset;
        }
    }
}
