using Microsoft.Xna.Framework;
using Nazdar.Shared;

namespace Nazdar
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }

        public void Follow(Objects.Player target)
        {
            Position = new Vector2(target.Hitbox.X + (target.Hitbox.Width / 2), 0);

            Matrix offset = Matrix.CreateTranslation(
                -Position.X + (MyShake.Active ? Tools.GetRandom(4) - 2 : 0),
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
