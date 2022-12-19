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

        public Arsenal(int x, int y, Building.Status status, float ttb = 5)
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

        public static void DrawCartridgesStatic(SpriteBatch spriteBatch, int howMany, int x, int y, float alpha = 1)
        {
            for (int i = 0; i < howMany; i++)
            {
                spriteBatch.Draw(
                    Assets.Images["BulletStatic"],
                    new Rectangle(
                        x + (Assets.Images["BulletStatic"].Width + 1) * (i % Enums.Offset.RowLimit),
                        y - Assets.Images["BulletStatic"].Height * 2 + (i / Enums.Offset.RowLimit * Assets.Images["BulletStatic"].Height),
                        Assets.Images["BulletStatic"].Width,
                        Assets.Images["BulletStatic"].Height
                    ),
                    Color.White * alpha
                );
            }
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
