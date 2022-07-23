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

        public bool ToDelete { get; set; }

        public Rectangle Hitbox { get; protected set; }

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

        public int Width
        {
            get
            {
                return Hitbox.Width;
            }
        }

        public int Height
        {
            get
            {
                return Hitbox.Height;
            }
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(Hitbox.X, Hitbox.Y);
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
            spriteBatch.DrawRectangle(new Rectangle(this.X, this.Y - 6, this.Width, 4), Color.Black * alpha);

            // inside
            int inside = (int)((this.Health / 100f) * (this.Width - 2));
            spriteBatch.DrawRectangle(new Rectangle(this.X + 1, this.Y - 5, inside, 2), Color.Green * alpha);
        }

        // returns true if it can take hit
        // returns false if it should die
        public bool TakeHit(int caliber)
        {
            if (this.Health - caliber > 0)
            {
                this.particleBlood.Run(100);
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
    }
}
