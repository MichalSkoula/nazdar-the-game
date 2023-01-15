using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Center : BaseBuilding
    {
        public const int Cost = 2;
        public const int CostMax = 32;
        public const string Name = "Center";
        public const int MaxCenterLevel = 4;
        public const int CenterRadius = 96;
        public bool HasBeenUpgradedToday = false;

        public Center(int x, int y, Building.Status status, int level = 1, float ttb = 10, bool hasBeenUpgradedToday = false) : base()
        {
            this.Sprite = Assets.Images["Center"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Center;
            this.Level = level;
            this.HasBeenUpgradedToday = hasBeenUpgradedToday;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            spriteBatch.DrawString(Assets.Fonts["Small"], "Lvl" + this.Level, new Vector2(this.X + 5, this.Y - 10), this.FinalColor);
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
                this.Status,
                this.Level,
                this.TimeToBuild,
                this.HasBeenUpgradedToday
            };
        }
    }
}
