using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SiberianAnabasis.Screens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Components
{
    public class Player : Component
    {
        private int speed = 120;

        private Animation anim;

        public List<Bullet> Bullets
        {
            get;
            private set;
        }

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.PlayerRight, 4, 10),
            new Animation(Assets.PlayerRight, 4, 10),
            new Animation(Assets.PlayerLeft, 4, 10),
            new Animation(Assets.PlayerLeft, 4, 10),
        };

        public int Money { get; set; }
        public int Days { get; set; }

        public Player(int x, int y)
        {
            this.anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.Caliber = 30;
            this.Days = 0;
        }

        public void Load(dynamic saveData)
        {
            // load player
            this.Hitbox = new Rectangle((int)saveData.Hitbox.X, this.Hitbox.Y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Direction = (Direction)saveData.Direction;
            this.Health = (int)saveData.Health;
            this.Days = (int)saveData.Days;
            this.Money = (int)saveData.Money;

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
            // moving?
            bool isMoving = false;
            Rectangle newHitbox = this.Hitbox;
            if (Controls.Keyboard.IsPressed(Keys.Right) && this.Hitbox.X < VillageScreen.MapWidth - this.Hitbox.Width)
            {
                newHitbox.X += (int)(deltaTime * this.speed);
                this.Direction = Direction.Right;
                isMoving = true;
            }
            else if (Controls.Keyboard.IsPressed(Keys.Left) && this.Hitbox.X > 0)
            {
                newHitbox.X -= (int)(deltaTime * this.speed);
                this.Direction = Direction.Left;
                isMoving = true;
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

            // jump?
            if (Controls.Keyboard.IsPressed(Keys.Up) && this.Hitbox.Y == Enums.Offset.Floor)
            {
                this.Jump();
                isMoving = true;
            }

            // bullets
            if (Controls.Keyboard.HasBeenPressed(Keys.Space))
            {
                this.Bullets.Add(new Bullet(this.Hitbox.X, this.Hitbox.Y + (this.Hitbox.Height / 2), this.Direction, this.Caliber));
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

        private async void Jump()
        {
            int h0 = Enums.Offset.Floor;
            int v0 = 25;
            int g = 10;
            float timeDivider = 5;

            for (int i = 0; i < 200; i++) 
            {
                Rectangle newHitbox = this.Hitbox;

                float t = i / timeDivider;
                int newY = h0 - (int)(v0 * t - 0.5 * g * Math.Pow(t, 2)); // https://cs.wikipedia.org/wiki/Vrh_svisl%C3%BD
                // System.Diagnostics.Debug.WriteLine(newY);
                if (newY > h0)
                {
                    newHitbox.Y = h0;
                    this.Hitbox = newHitbox;

                    break;
                }

                newHitbox.Y = newY;
                this.Hitbox = newHitbox;

                await Task.Delay(20);
            }
        }
    }
}
