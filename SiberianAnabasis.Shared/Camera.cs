using Microsoft.Xna.Framework;

namespace SiberianAnabasis
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Components.Player target)
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

            this.Transform = position * offset;
        }
    }
}
