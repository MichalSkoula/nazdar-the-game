using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Objects;

namespace Nazdar.Weather
{
    public class Raindrop : BaseObject
    {
        public Raindrop(int x, int y)
        {
            this.Hitbox = new Rectangle(x, y, 5, 10);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, MyColor.White, 10);
        }

        public void Fall()
        {
            this.Y++;
        }
    }
}
