using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Soldier : BasePerson
    {
        private bool isFast = true;
        private int shootRate = 70; // 0 fastest, 100 slowest

        public const int DefaultHealth = 100;
        public const int DefaultCaliber = 16;

        public List<Bullet> Bullets { get; private set; }

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["SoldierRight"], 4, 10),
            new Animation(Assets.Images["SoldierRight"], 4, 10),
            new Animation(Assets.Images["SoldierLeft"], 4, 10),
            new Animation(Assets.Images["SoldierLeft"], 4, 10),
        };

        public Soldier(int x, int y, Direction direction, int health = DefaultHealth, int caliber = DefaultCaliber)
        {
            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Direction = direction;
            this.Health = health;
            this.Caliber = caliber;
            this.Speed = 100;
            this.Bullets = new List<Bullet>();
            this.Color = MyColor.UniversalColors[Tools.GetRandom(MyColor.UniversalColors.Length)];

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Down,
                2,
                Assets.ParticleTextureRegions["Blood"]
            );
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // particles
            this.particleBlood.Update(deltaTime, new Vector2(this.X, this.Y));

            if (this.Dead)
            {
                return;
            }

            // is soldier moving?
            bool isMoving = false;
            if (Tools.GetRandom(4) == 1 || this.isFast)
            {
                if (this.Direction == Direction.Right)
                {
                    this.X += (int)(deltaTime * this.Speed);
                    isMoving = true;
                }
                else if (this.Direction == Direction.Left)
                {
                    this.X -= (int)(deltaTime * this.Speed);
                    isMoving = true;
                }
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

            this.isFast = true;

            if (this.DeploymentBuilding == null)
            {
                // run towards town center
                if (this.X < VillageScreen.MapWidth / 2 - Center.CenterRadius)
                {
                    this.Direction = Direction.Right;
                }
                else if (this.X > VillageScreen.MapWidth / 2 + Center.CenterRadius)
                {
                    this.Direction = Direction.Left;
                }
                else
                {
                    this.isFast = false;
                    if (Tools.GetRandom(128) == 1)
                    {
                        this.ChangeDirection();
                    }
                }
            }
            else
            {
                // soldier is deployed somewhere
                if (this.X < this.DeploymentBuilding.X - this.DeploymentBuilding.Width / 2)
                {
                    this.Direction = Direction.Right;
                }
                else if (this.X > this.DeploymentBuilding.X + this.DeploymentBuilding.Width + this.DeploymentBuilding.Width / 2)
                {
                    this.Direction = Direction.Left;
                }
                else
                {
                    this.isFast = false;
                    if (Tools.GetRandom(128) == 1)
                    {
                        this.ChangeDirection();
                    }
                }
            }

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }
            this.Bullets.RemoveAll(p => p.ToDelete);
        }

        public void PrepareToShoot(Direction direction)
        {
            this.isFast = false;
            this.Direction = direction;

            if (Game1.GlobalTimer % this.shootRate == 0)
            {
                Audio.PlaySound("GunFire");
                this.Bullets.Add(new Bullet(
                    this.X + this.Width / 2,
                    this.Y + this.Height / 4,
                    this.Direction,
                    this.Caliber
                ));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch);
            this.DrawCaliber(spriteBatch);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // particles
            this.particleBlood.Draw(spriteBatch);
        }
    }
}
