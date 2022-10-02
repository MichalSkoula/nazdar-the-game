using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Objects
{
    public class Weapon : BaseObject
    {
        public const string Name = "Weapon";

        public Weapon()
        {
            this.Sprite = Assets.Images["Crate"];
            this.Hitbox = new Rectangle(0, 0, this.Sprite.Width, this.Sprite.Height);
        }

        public void SetPosition(int x, int y)
        {
            this.Position = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, new Rectangle(this.X, this.Y, this.Hitbox.Width, this.Hitbox.Height), Color.White);
        }
    }
}
