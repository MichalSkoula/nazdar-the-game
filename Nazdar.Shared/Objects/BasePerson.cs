using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Shared;
using System.Linq;

namespace Nazdar.Objects
{
    public abstract class BasePerson : BaseObject
    {
        public BasePerson()
        {
            this.Color = MyColor.UniversalColors[Tools.GetRandom(MyColor.UniversalColors.Length)];
        }

        protected int Speed { get; set; }

        public BasePerson DeploymentPerson = null;
        public BaseBuilding DeploymentBuilding = null;
        public int? DeploymentX = null;

        public void DrawCaliber(SpriteBatch spriteBatch)
        {
            if (this.Dead)
            {
                return;
            }

            spriteBatch.DrawString(Assets.Fonts["Small"], this.Caliber.ToString(), new Vector2(this.X, this.Y - 16), MyColor.White);
        }

        protected ParticleSource particleBlood;

        // returns true if it can take hit
        // returns false if it should die
        public bool TakeHit(int caliber)
        {
            // 20% random +/-
            caliber += Tools.GetRandom((int)(caliber * 0.2f)) * (Tools.GetRandom(2) * 2 - 1);

            this.particleBlood.Run(100);

            if (this.Health - caliber > 0)
            {
                this.Health -= caliber;
                return true;
            }

            this.Health = 0;

            return false;
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Direction,
                this.Health,
                this.Caliber,
                Bullets = this.Bullets.Select(b => new { b.Hitbox, b.Direction, b.Caliber }).ToList()
            };
        }
    }
}
