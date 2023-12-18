using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Lenin : BasePerson
    {
        public const int DefaultHealth = 100;
        public const int DefaultCaliber = 48;

        private List<Animation> animations = new List<Animation>();

        protected ParticleSource particleShit;

        public Lenin(int x, int y, Direction direction, int health = DefaultHealth, int caliber = DefaultCaliber) : base()
        {
            this.Direction = direction;
            this.Health = health;
            this.Caliber = caliber;
            this.Speed = 61;
            this.Name = "Lenin Tractor";

            this.animations.Add(new Animation(Assets.Images["LeninRight"], 3, 8));
            this.animations.Add(new Animation(Assets.Images["LeninRight"], 3, 8));
            this.animations.Add(new Animation(Assets.Images["LeninLeft"], 3, 8));
            this.animations.Add(new Animation(Assets.Images["LeninLeft"], 3, 8));
            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                Direction.Down,
                2,
                Assets.ParticleTextureRegions["Blood"]
            );

            this.particleShit = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>((int)(this.Width * 0.8f), (int)(this.Height * 0.25f)),
                Direction.Up,
                2,
                Assets.ParticleTextureRegions["SmokeDark"]

            );
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // particles
            this.particleBlood.Update(deltaTime, new Vector2(this.X, this.Y));
            this.particleShit.Update(deltaTime, new Vector2(this.X, this.Y));

            if (this.Dead)
            {
                return;
            }

            // poo
            if (Tools.RandomChance(40))
            {
                this.particleShit.Run(60);
            }

            // is enemy moving?
            bool isMoving = true;
            if (this.Direction == Direction.Right)
            {
                this.X += (int)(deltaTime * this.Speed);
            }
            else if (this.Direction == Direction.Left)
            {
                this.X -= (int)(deltaTime * this.Speed);
            }
            else
            {
                isMoving = false;
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

            // out of game map
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ToDelete = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch);
            // this.DrawCaliber(spriteBatch);

            // particles
            this.particleBlood.Draw(spriteBatch);
            this.particleShit.Draw(spriteBatch);
        }
    }
}
