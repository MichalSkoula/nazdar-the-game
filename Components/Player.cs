namespace MyGame.Components
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MyGame.Screens;

    public class Player : Component
    {
        private int speed = 150;
        private int caliber;

        private Animation anim;

        private Enums.Direction direction;

        public List<Bullet> Bullets
        {
            get;
            private set;
        }

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.PlayerUp, 4, 10),
            new Animation(Assets.PlayerRight, 4, 10),
            new Animation(Assets.PlayerDown, 4, 10),
            new Animation(Assets.PlayerLeft, 4, 10),
        };

        public Player(int x, int y)
        {
            this.anim = this.animations[(int)Enums.Direction.Right];
            this.direction = Enums.Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.caliber = 30;
        }

        public void Load(dynamic data)
        {
            // only change X
            this.Hitbox = new Rectangle((int)data.Hitbox.X, this.Hitbox.Y, this.anim.FrameWidth, this.anim.FrameHeight);
        }

        public override void Update(float deltaTime)
        {
            // is player moving?
            bool isMoving = true;
            Rectangle newHitbox = this.Hitbox;
            if (Controls.Keyboard.IsPressed(Keys.Right) && this.Hitbox.X < MapScreen.MapWidth - this.Hitbox.Width)
            {
                newHitbox.X += (int)(deltaTime * this.speed);
                this.direction = Enums.Direction.Right;
            }
            else if (Controls.Keyboard.IsPressed(Keys.Left) && this.Hitbox.X > 0)
            {
                newHitbox.X -= (int)(deltaTime * this.speed);
                this.direction = Enums.Direction.Left;
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                this.Hitbox = newHitbox;
                this.anim.Loop = true;
                this.anim = this.animations[(int)this.direction];
            }
            else
            {
                this.anim.Loop = false;
                this.anim.ResetLoop();
            }

            this.anim.Update(deltaTime);

            // bullets
            if (Controls.Keyboard.HasBeenPressed(Keys.Space))
            {
                this.Bullets.Add(new Bullet(this.Hitbox.X, this.Hitbox.Y + (this.Hitbox.Height / 2), this.direction, this.caliber));
            }

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }

            this.Bullets.RemoveAll(p => p.ToDelete);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
