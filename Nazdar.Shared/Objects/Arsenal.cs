using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Arsenal : BaseBuilding
    {
        public const int Cost = 3;
        public const string Name = "Arsenal";
        public const string CartridgeName = "Cartridge";

        public const int CartridgesCost = 1;
        public const int CartridgesCount = 5;

        public Arsenal(int x, int y, Building.Status status)
        {
            this.Sprite = Assets.Images["Arsenal"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 5;
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
    }
}
