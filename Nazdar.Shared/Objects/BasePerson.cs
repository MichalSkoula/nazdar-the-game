using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nazdar.Shared;

namespace Nazdar.Objects
{
    public abstract class BasePerson : BaseObject
    {
        protected Animation Anim { get; set; }
        protected int Speed { get; set; }

        public int? DeploymentX { get; set; } = null;

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            if (this.Dead)
            {
                return;
            }

            // border
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 14, this.Width, 4), MyColor.Black * this.Alpha);

            // background 
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 13, this.Width - 2, 2), MyColor.White * this.Alpha);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Width - 2));
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 13, inside, 2), MyColor.Green * this.Alpha);
        }

        public void DrawCaliber(SpriteBatch spriteBatch)
        {
            if (this.Dead)
            {
                return;
            }

            spriteBatch.DrawString(Assets.Fonts["Small"], this.Caliber.ToString(), new Vector2(this.X, this.Y - 8), MyColor.White);
        }

        protected ParticleSource particleBlood;

        // returns true if it can take hit
        // returns false if it should die
        public bool TakeHit(int caliber)
        {
            this.particleBlood.Run(100);

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
