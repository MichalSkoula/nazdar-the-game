using Microsoft.Xna.Framework;
using Nazdar.Shared;
using System.Threading.Tasks;
using System;

namespace Nazdar
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        private bool shaking { get; set; }

        public void Follow(Objects.Player target)
        {
            Matrix offset = Matrix.CreateTranslation(
                -target.Hitbox.X - (target.Hitbox.Width / 2) + (this.shaking ? Tools.GetRandom(4) - 2 : 0),
                0 + (this.shaking ? Tools.GetRandom(4 - 2) : 0),
                0
            );

            Matrix position = Matrix.CreateTranslation(
                Enums.Screen.Width / 2,
                0,
                0
            );

            this.Transform = position * offset;
        }

        public async void Shake()
        {
            if (this.shaking)
            {
                return;
            }

            this.shaking = true;
            await Task.Delay(250);
            this.shaking = false;
        }
    }
}
