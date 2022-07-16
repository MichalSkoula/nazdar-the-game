using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SiberianAnabasis.Objects
{
    public abstract class BaseObject
    {
        public int Health { get; set; }

        public int Caliber { get; set; }

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

        protected Texture2D Sprite { get; set; }

        public Enums.Direction Direction { get; protected set; }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(float deltaTime);

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            float alpha = 1f;
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
                this.Health -= caliber;
                return true;
            }

            this.Health = 0;

            return false;
        }
    }
}
