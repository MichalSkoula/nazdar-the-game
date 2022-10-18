using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Center : BaseBuilding
    {
        public const int Cost = 1;
        public const string Name = "Center";

        public Center(int x, int y, Building.Status status, int level = 1)
        {
            this.Sprite = Assets.Images["Center"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 10;
            this.Type = Building.Type.Center;
            this.Level = level;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            spriteBatch.DrawString(Assets.Fonts["Small"], "Lvl" + this.Level, new Vector2(this.X + 5, this.Y + 5), this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
