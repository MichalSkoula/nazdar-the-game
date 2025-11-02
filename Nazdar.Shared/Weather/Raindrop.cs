using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Objects;
using Nazdar.Shared;

namespace Nazdar.Weather
{
    public class Raindrop : BaseObject, IDrop
    {
        private readonly int speed;

        public Raindrop(int x, int y)
        {
            this.Hitbox = new Rectangle(x, y, Tools.RandomChance(2) ? 1 : 2, 6);
            this.Alpha = 0.5f;
            this.Color = MyColor.White;
            this.speed = 5 + Tools.GetRandom(3);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, this.FinalColor, 1);
        }

        public void Fall()
        {
            this.Y += this.speed;
        }
    }
}
