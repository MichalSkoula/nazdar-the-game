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
using SiberianAnabasis.Controls;
using Keyboard = SiberianAnabasis.Controls.Keyboard;

namespace SiberianAnabasis.Objects
{
    public class Player : BasePerson
    {
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
        public int ActionCost { get; set; }

        private ParticleSource particleSmoke;

        public Player(int x, int y)
        {
            this.Anim = this.animations[(int)Direction.Right];
            this.Direction = Direction.Right;
            this.Bullets = new List<Bullet>();
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Health = 100;
            this.Caliber = 30;
            this.Days = 0;
            this.Speed = 121;

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
            this.Hitbox = new Rectangle((int)saveData.Hitbox.X, this.Y, this.Anim.FrameWidth, this.Anim.FrameHeight);
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
            if ((Keyboard.IsPressed(Keys.Right) || Gamepad.Thumbstick(Direction.Right)) && this.X < VillageScreen.MapWidth - this.Width)
            {
                this.X += (int)(deltaTime * this.Speed);
                this.Direction = Direction.Right;
                isMoving = true;
            }
            else if ((Keyboard.IsPressed(Keys.Left) || Gamepad.Thumbstick(Direction.Left)) && this.X > 0)
            {
                this.X -= (int)(deltaTime * this.Speed);
                this.Direction = Direction.Left;
                isMoving = true;
            }

            if (isMoving)
            {
                this.Anim.Loop = true;
                this.Anim = this.animations[(int)this.Direction];
            }
            else
            {
                this.Anim.Loop = false;
                this.Anim.ResetLoop();
            }

            this.Anim.Update(deltaTime);

            // jump?
            if ((Keyboard.IsPressed(Keys.Space) || Keyboard.IsPressed(Keys.C) || Gamepad.HasBeenPressed(Buttons.A)) && this.Y == Enums.Offset.Floor)
            {
                this.Jump();
                isMoving = true;
            }

            // bullets
            if (Keyboard.HasBeenPressed(Keys.X) || Gamepad.HasBeenPressed(Buttons.X) || Gamepad.HasBeenPressed(Buttons.RightTrigger))
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
            this.Anim.Draw(spriteBatch, this.Hitbox);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // particles
            this.particleBlood.Draw(spriteBatch);
            this.particleSmoke.Draw(spriteBatch);

            // some action?
            if (this.Action != null)
            {
                for (int i = 0; i < this.ActionCost; i++)
                {
                    spriteBatch.DrawCircle(this.X + this.Width / 2, this.Y - 5 - 10 * i, 4, 50, Color.Yellow);
                }
                spriteBatch.DrawString(Assets.Fonts["Small"], this.Action.ToString(), new Vector2(this.X + this.Width + 5, this.Y - 10), Color.White);
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
