using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Ship : BaseObject
    {
        public const int Cost = 48;
        public const string Name = "Roma Ship";

        public Ship(int x, int y) : base()
        {
            this.Sprite = Assets.Images["Ship"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {

            base.Update(deltaTime);
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
            };
        }
    }
}
