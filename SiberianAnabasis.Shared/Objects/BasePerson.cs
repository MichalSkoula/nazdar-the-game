using SiberianAnabasis.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SiberianAnabasis.Shared;

namespace SiberianAnabasis.Objects
{
    public abstract class BasePerson : BaseObject
    {
        protected Animation Anim { get; set; }
        protected int Speed { get; set; }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            if (this.Dead)
            {
                return;
            }

            // border
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 6, this.Width, 4), Color.Black * this.Alpha);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Width - 2));
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 5, inside, 2), Color.Green * this.Alpha);
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
