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

        public bool IsBeingBuilt { get; set; }

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

        public abstract void Draw(SpriteBatch spriteBatch);

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
