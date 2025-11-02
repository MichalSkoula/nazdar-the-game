using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Center : BaseBuilding
    {
        public const int Cost = 2;
        public const int CostMax = 32;
        public const int CenterRadius = 96;
        public bool HasBeenUpgradedToday = false;

        public Center(int x, int y, Building.Status status, float ttb = 10, bool hasBeenUpgradedToday = false) : base()
        {
            this.Sprite = Assets.Images["Center"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Center;
            this.HasBeenUpgradedToday = hasBeenUpgradedToday;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Assets.Fonts["Small"], "Lvl" + Game1.CenterLevel, new Vector2(this.X + 5, this.Y - 10), this.FinalColor);
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
                this.TimeToBuild,
                this.HasBeenUpgradedToday
            };
        }

        public override string Name => Nazdar.Shared.Translation.Translation.Get("building.base");
    }
}
