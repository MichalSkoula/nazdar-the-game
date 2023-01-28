using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Arsenal : BaseBuilding
    {
        public const int Cost = 4;
        public const string Name = "Arsenal";
        public const string CartridgeName = "Cartridge";

        public const int CartridgesCost = 3;
        public const int CartridgesCount = 6;

        public Arsenal(int x, int y, Building.Status status, float ttb = 5) : base()
        {
            this.Sprite = Assets.Images["Arsenal"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Arsenal;
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
                this.Status,
                this.TimeToBuild
            };
        }
    }
}
