using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Rails : BaseBuilding
    {
        public const int Cost = 8;
        public const string Name = "Rails";

        public Rails(int x, int y, Building.Status status, float ttb = 6) : base()
        {
            this.Sprite = Assets.Images["RailsRuins"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Rails;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // construction tape
            base.Draw(spriteBatch);
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
