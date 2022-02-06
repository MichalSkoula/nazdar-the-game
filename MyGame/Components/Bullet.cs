namespace MyGame.Components
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MyGame.Screens;

    public class Bullet : Component
    {
        private int speed = 1000;

        public int Caliber { get; set; }

        public bool ToDelete { get; set; }

        public Bullet(int x, int y, Enums.Direction direction, int caliber)
        {
            this.Sprite = Assets.Bullet;
            this.Hitbox = new Rectangle(x, y - (this.Sprite.Height / 2), this.Sprite.Width, this.Sprite.Height);
            this.Direction = direction;
            this.Caliber = caliber;

            Assets.Blip.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, Color.White);
        }

        public override void Update(float deltaTime)
        {
            // move it
            Rectangle newHitbox = this.Hitbox;
            if (this.Direction == Enums.Direction.Left)
            {
                newHitbox.X -= (int)(deltaTime * this.speed);
            }
            else if (this.Direction == Enums.Direction.Right)
            {
                newHitbox.X += (int)(deltaTime * this.speed);
            }

            this.Hitbox = newHitbox;

            // out of game map
            if (this.Hitbox.X < 0 || this.Hitbox.X > MapScreen.MapWidth)
            {
                this.ToDelete = true;
            }
        }
    }
}
