using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Screens;
using static MyGame.Enums;

namespace MyGame.Components
{
    public class Player : Component
    {
        private int speed = 150;
        private int caliber;

        private Animation anim;

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
            this.anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.caliber = 30;
        }

        public void Load(dynamic saveData)
        {
            // only change X
            this.Hitbox = new Rectangle((int)saveData.Hitbox.X, this.Hitbox.Y, this.anim.FrameWidth, this.anim.FrameHeight);

            // load bullets
            if (saveData.ContainsKey("Bullets"))
            {
                foreach (var bullet in saveData.GetValue("Bullets"))
                {
                    this.Bullets.Add(new Bullet((int)bullet.Hitbox.X, (int)bullet.Hitbox.Y, (Direction)bullet.Direction, (int)bullet.Caliber));
                }
            }
        }

        public override void Update(float deltaTime)
        {
            // is player moving?
            bool isMoving = true;
            Rectangle newHitbox = this.Hitbox;
            if (Controls.Keyboard.IsPressed(Keys.Right) && this.Hitbox.X < MapScreen.MapWidth - this.Hitbox.Width)
            {
                newHitbox.X += (int)(deltaTime * this.speed);
                this.Direction = Direction.Right;
            }
            else if (Controls.Keyboard.IsPressed(Keys.Left) && this.Hitbox.X > 0)
            {
                newHitbox.X -= (int)(deltaTime * this.speed);
                this.Direction = Direction.Left;
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                this.Hitbox = newHitbox;
                this.anim.Loop = true;
                this.anim = this.animations[(int)this.Direction];
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
                this.Bullets.Add(new Bullet(this.Hitbox.X, this.Hitbox.Y + (this.Hitbox.Height / 2), this.Direction, this.caliber));
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
