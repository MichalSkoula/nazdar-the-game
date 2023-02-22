using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Objects;
using Nazdar.Shared;

namespace Nazdar.Weather
{
    public class Raindrop : BaseObject, IDrop
    {
        public Raindrop(int x, int y)
        {
            this.Hitbox = new Rectangle(x, y, Tools.GetRandom(2) == 0 ? 1 : 2, 6);
            this.Alpha = 0.5f;
            this.Color = MyColor.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, this.FinalColor, 1);
        }

        public void Fall()
        {
            this.Y += 6;
        }
    }
}
