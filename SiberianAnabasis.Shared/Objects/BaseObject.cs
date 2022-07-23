using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SiberianAnabasis.Shared;
using System.Threading.Tasks;

namespace SiberianAnabasis.Objects
{
    public abstract class BaseObject
    {
        public int Health { get; set; }

        public int Caliber { get; set; }

        public Rectangle Hitbox { get; protected set; }

        public bool ToDelete { get; set; }
        public int X
        {
            get
            {
                return Hitbox.X;
            }
            protected set
            {
                var temp = this.Hitbox;
                temp.X = value;
                this.Hitbox = temp;
            }
        }
        public int Y
        {
            get
            {
                return Hitbox.Y;
            }
            protected set
            {
                var temp = this.Hitbox;
                temp.Y = value;
                this.Hitbox = temp;
            }
        }

        protected Texture2D Sprite { get; set; }

        public Enums.Direction Direction { get; protected set; }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(float deltaTime);

        protected ParticleSource particleBlood;

        public void DrawHealth(SpriteBatch spriteBatch, float alpha = 1)
        {
            /*
            if (this.Health == 100)
            {
                alpha = 0.25f;
            }
            */

            // border
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 6, this.Hitbox.Width, 4), Color.Black * alpha);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Hitbox.Width - 2));
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 5, inside, 2), Color.Green * alpha);
        }

        // returns true if it can take hit
        // returns false if it should die
        public bool TakeHit(int caliber)
        {
            if (this.Health - caliber > 0)
            {
                this.Bleed();
                this.Health -= caliber;
                return true;
            }

            this.Health = 0;

            return false;
        }

        public void ChangeDirection()
        {
            if (this.Direction == Enums.Direction.Left)
            {
                this.Direction = Enums.Direction.Right;
            }
            else if (this.Direction == Enums.Direction.Right)
            {
                this.Direction = Enums.Direction.Left;
            }
        }
        private async void Bleed()
        {
            if (this.particleBlood == null)
            {
                return;
            }

            this.particleBlood.Start();
            await Task.Delay(100);
            this.particleBlood.Stop();
        }
    }
}
