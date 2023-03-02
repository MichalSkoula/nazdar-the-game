using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Shared;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    // treasure is already built and has a health
    public class Treasure : BaseBuilding
    {
        public const string Name = "Golden Treasure";

        public Treasure(int x, int y, int health = 100) : base()
        {
            this.Sprite = Assets.Images["Treasure1"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Health = health;
            this.Type = Building.Type.Treasure;
            this.Status = Building.Status.Built;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch, true);
        }

        public new void Update(float deltaTime)
        {
            if (this.Health >= 75)
            {
                this.Sprite = Assets.Images["Treasure1"];
            }
            else if (this.Health >= 50)
            {
                this.Sprite = Assets.Images["Treasure2"];
            }
            else if (this.Health >= 25)
            {
                this.Sprite = Assets.Images["Treasure3"];
            }
            else if (this.Health >= 0)
            {
                this.Sprite = Assets.Images["Treasure4"];
            }
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Health
            };
        }

        public bool CoinTheft(int caliber)
        {
            // must be lower
            caliber /= 16;

            // 20% random +/-
            caliber += Tools.GetRandom((int)(caliber * 0.2f)) * (Tools.GetRandom(2) * 2 - 1);

            if (this.Health - caliber > 0)
            {
                this.Health -= caliber;
                return true;
            }

            this.Health = 0;

            return false;
        }
    }
}
