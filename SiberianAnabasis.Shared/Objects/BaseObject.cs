using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SiberianAnabasis.Shared;

namespace SiberianAnabasis.Objects
{
    public abstract class BaseObject
    {
        public int Health { get; set; }

        public int Caliber { get; set; }

        public bool ToDelete { get; set; }

        // draw colors and alpha
        public Color Color { private get; set; } = Color.White;
        public float Alpha { private get; set; } = 1;
        public Color FinalColor
        {
            get
            {
                return this.Color * this.Alpha;
            }
        }

        // direction, position, hitbox, dimensions
        public Enums.Direction Direction { get; protected set; }
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

        // assets
        protected Texture2D Sprite { get; set; }
        protected ParticleSource particleBlood;

        public abstract void Draw(SpriteBatch spriteBatch);

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
