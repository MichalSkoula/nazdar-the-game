using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SiberianAnabasis.Screens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SiberianAnabasis.Enums;
using SiberianAnabasis.Shared;
using MonoGame.Extended;

namespace SiberianAnabasis.Objects
{
    public class Player : BaseObject
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

        private ParticleSource particleSource;
        public int Money { get; set; }
        public int Days { get; set; }

        public bool ActiveButton { get; set; }

        public Player(int x, int y)
        {
            this.anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.Caliber = 30;
            this.Days = 0;

            this.particleSource = new ParticleSource(new Vector2(this.X, this.Y));
        }

        public void Load(dynamic saveData)
        {
            // load player
            this.Hitbox = new Rectangle((int)saveData.Hitbox.X, this.Y, this.anim.FrameWidth, this.anim.FrameHeight);
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
            if ((Controls.Keyboard.IsPressed(Keys.Right) || Controls.Gamepad.Thumbstick(Direction.Right)) && this.X < VillageScreen.MapWidth - this.Hitbox.Width)
            {
                this.X += (int)(deltaTime * this.speed);
                this.Direction = Direction.Right;
                isMoving = true;
            }
            else if ((Controls.Keyboard.IsPressed(Keys.Left) || Controls.Gamepad.Thumbstick(Direction.Left)) && this.X > 0)
            {
                this.X -= (int)(deltaTime * this.speed);
                this.Direction = Direction.Left;
                isMoving = true;
            }

            if (isMoving)
            {
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
            if ((Controls.Keyboard.IsPressed(Keys.Up) || Controls.Gamepad.HasBeenPressed(Buttons.A)) && this.Y == Enums.Offset.Floor)
            {
                this.Jump();
                isMoving = true;
            }

            // bullets
            if (Controls.Keyboard.HasBeenPressed(Keys.Space) || Controls.Gamepad.HasBeenPressed(Buttons.X) || Controls.Gamepad.HasBeenPressed(Buttons.RightTrigger))
            {
                this.Bullets.Add(new Bullet(this.X, this.Y + (this.Hitbox.Height / 2), this.Direction, this.Caliber));
            }

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }

            this.Bullets.RemoveAll(p => p.ToDelete);

            // particles
            this.particleSource.Update(deltaTime, new Vector2(this.X, this.Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // particles
            this.particleSource.Draw(spriteBatch);

            // active button?
            if (this.ActiveButton)
            {
                spriteBatch.DrawCircle(this.X + this.Hitbox.Width / 2, this.Y - 10, 5, 100, Color.Yellow);
            }
        }

        private async void Jump()
        {
            int h0 = Enums.Offset.Floor;
            int v0 = 25;
            int g = 10;
            float timeDivider = 5;

            this.particleSource.Start();

            for (int i = 0; i < 200; i++) 
            {
                float t = i / timeDivider;
                int newY = h0 - (int)(v0 * t - 0.5 * g * Math.Pow(t, 2)); // https://cs.wikipedia.org/wiki/Vrh_svisl%C3%BD
                // System.Diagnostics.Debug.WriteLine(newY);
                if (newY > h0)
                {
                    this.Y = h0;
                    this.particleSource.Stop();
                    break;
                }

                this.Y = newY;

                await Task.Delay(20);
            }
        }
    }
}
