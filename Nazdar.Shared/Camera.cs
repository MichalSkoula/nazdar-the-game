using Microsoft.Xna.Framework;
using Nazdar.Shared;

namespace Nazdar
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Objects.Player target)
        {
            Matrix offset = Matrix.CreateTranslation(
                -target.Hitbox.X - (target.Hitbox.Width / 2) + (MyShake.Active ? Tools.GetRandom(4) - 2 : 0),
                0 + (MyShake.Active ? Tools.GetRandom(4 - 2) : 0),
                0
            );

            Matrix position = Matrix.CreateTranslation(
                Enums.Screen.Width / 2,
                0,
                0
            );

            this.Transform = position * offset;
        }
    }
}
