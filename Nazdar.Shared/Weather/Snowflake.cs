using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Objects;
using Nazdar.Shared;

namespace Nazdar.Weather
{
    public class Snowflake : BaseObject, IDrop
    {
        public Snowflake(int x, int y)
        {
            bool small = Tools.GetRandom(2) == 0;

            this.Hitbox = new Rectangle(x, y, small ? 2 : 3, small ? 2 : 3);
            this.Alpha = 0.75f;
            this.Color = MyColor.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, this.FinalColor, 2);
        }

        public void Fall()
        {
            this.Y += 2;
        }
    }
}
