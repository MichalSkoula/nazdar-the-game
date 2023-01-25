using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Medic : BasePerson
    {
        private bool isFast = true;

        public const int DefaultHealth = 100;
        public const int DefaultCaliber = 4;

        protected ParticleSource particleHeal;

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["MedicRight"], 4, 10),
            new Animation(Assets.Images["MedicRight"], 4, 10),
            new Animation(Assets.Images["MedicLeft"], 4, 10),
            new Animation(Assets.Images["MedicLeft"], 4, 10),
        };

        public Medic(int x, int y, Direction direction, int health = DefaultHealth, int caliber = DefaultCaliber) : base()
        {
            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Direction = direction;
            this.Health = health;
            this.Speed = 100;
            this.Caliber = caliber;

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Down,
                2,
                Assets.ParticleTextureRegions["Blood"]
            );

            this.particleHeal = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Up,
                1,
                Assets.ParticleTextureRegions["Heal"]
            );
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // particles
            this.particleBlood.Update(deltaTime, new Vector2(this.X, this.Y));
            this.particleHeal.Update(deltaTime, new Vector2(this.X, this.Y));

            if (this.Dead)
            {
                return;
            }

            // is he moving?
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

            // go somewhere
            if (this.DeploymentPerson == null)
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
                    if (Tools.GetRandom(128) < 2)
                    {
                        this.ChangeDirection();
                    }
                }
            }
            else
            {
                // is deployed on someone
                if (this.X < this.DeploymentPerson.X - this.DeploymentPerson.Width / 4)
                {
                    this.Direction = Direction.Right;
                }
                else if (this.X > this.DeploymentPerson.X + this.DeploymentPerson.Width / 4)
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

                    // heal?
                    if (Game1.GlobalTimer % 30 == 0)
                    {
                        this.DeploymentPerson.Health++;
                        this.particleHeal.Run(50);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch);
            this.DrawCaliber(spriteBatch);

            // particles
            this.particleBlood.Draw(spriteBatch);
            this.particleHeal.Draw(spriteBatch);
        }
    }
}
