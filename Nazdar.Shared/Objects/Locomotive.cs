using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Locomotive : BaseBuilding
    {
        // 24 in the first mission (just above the coins lying around at start), +4 per mission (48 in mission 7, same as the ship)
        public const int Cost = 24;
        public const int CostPerVillage = 4;

        public static int GetCost(int village)
        {
            return Cost + ((village - 1) * CostPerVillage);
        }
        public override string Name => Nazdar.Shared.Translation.Translation.Get("building.locomotive");

        public Locomotive(int x, int y, Building.Status status, float ttb = 20) : base()
        {
            this.Sprite = Assets.Images["Locomotive"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Locomotive;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            base.Draw(spriteBatch);
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
                this.TimeToBuild
            };
        }
    }
}
