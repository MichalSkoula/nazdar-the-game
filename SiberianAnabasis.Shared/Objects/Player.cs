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
    public class Player : BasePerson
    {
        private int speed = 121;

        private Animation anim;

        public List<Bullet> Bullets
        {
            get;
            private set;
        }

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["PlayerRight"], 4, 10),
            new Animation(Assets.Images["PlayerRight"], 4, 10),
            new Animation(Assets.Images["PlayerLeft"], 4, 10),
            new Animation(Assets.Images["PlayerLeft"], 4, 10),
        };

        public int Money { get; set; }
        public int Days { get; set; }

        public Enums.PlayerAction? Action { get; set; }

        private ParticleSource particleSmoke;

        public Player(int x, int y)
        {
            this.anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Health = 100;
            this.Caliber = 30;
            this.Days = 0;

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Down,
                2,
                Assets.ParticleTextureRegions["Blood"]
            );

            this.particleSmoke = new ParticleSource(
               new Vector2(this.X, this.Y),
               new Tuple<int, int>(0, this.Height / 2),
               Direction.Up,
               0.5f,
               Assets.ParticleTextureRegions["Smoke"]
            );
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

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // moving?
            bool isMoving = false;
            if ((Controls.Keyboard.IsPressed(Keys.Right) || Controls.Gamepad.Thumbstick(Direction.Right)) && this.X < VillageScreen.MapWidth - this.Width)
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
                Audio.PlaySound("GunFire");
                this.Bullets.Add(new Bullet(this.X + (this.Width / 2), this.Y + (this.Height / 2), this.Direction, this.Caliber));
                this.particleSmoke.Run(50);
            }
            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }
            this.Bullets.RemoveAll(p => p.ToDelete);

            // particles
            this.particleBlood.Update(deltaTime, this.Position);
            this.particleSmoke.Update(deltaTime, new Vector2(this.X + (this.Direction == Direction.Left ? 0 : this.Width), this.Y));
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
            this.particleBlood.Draw(spriteBatch);
            this.particleSmoke.Draw(spriteBatch);

            // some action?
            switch (this.Action)
            {
                case Enums.PlayerAction.Build:
                    spriteBatch.DrawCircle(this.X + this.Width / 2, this.Y - 10, 5, 100, Color.Brown);
                    break;
                case Enums.PlayerAction.Hire:
                    spriteBatch.DrawCircle(this.X + this.Width / 2, this.Y - 10, 5, 100, Color.Yellow);
                    break;
                default:
                    break;

            }
        }

        private async void Jump()
        {
            Audio.PlaySound("Jump");
            int h0 = Enums.Offset.Floor;
            int v0 = 25;
            int g = 10;
            float timeDivider = 5;

            for (int i = 0; i < 200; i++) 
            {
                float t = i / timeDivider;
                int newY = h0 - (int)(v0 * t - 0.5 * g * Math.Pow(t, 2)); 
                if (newY > h0)
                {
                    this.Y = h0;
                    break;
                }

                this.Y = newY;

                await Task.Delay(20);
            }
        }
    }
}
