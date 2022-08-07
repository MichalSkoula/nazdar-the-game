using SiberianAnabasis.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SiberianAnabasis.Objects
{
    public abstract class BasePerson : BaseObject
    {
        // dying ... ttd - time to die
        private float ttd = 0f;
        private bool dead = false;
        public bool Dead
        {
            get
            {
                return this.dead;
            }
            set
            {
                this.dead = value;
                if (this.dead == true && this.ttd == 0)
                {
                    this.Color = Color.Red;
                    this.Alpha = 0.2f;
                    this.ttd = 3;
                }
            }
        }

        // base update method - should be called as base.Update() 
        public void Update(float deltaTime)
        {
            if (this.dead)
            {
                this.ttd -= deltaTime;
                if (this.ttd <= 0)
                {
                    this.ToDelete = true;
                }
            }
        }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            if (this.Dead)
            {
                return;
            }

            // border
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 6, this.Width, 4), this.FinalColor);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Width - 2));
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 5, inside, 2), this.FinalColor);
        }
    }
}
