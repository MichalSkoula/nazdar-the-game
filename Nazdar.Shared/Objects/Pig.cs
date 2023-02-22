using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Pig : BasePerson
    {
        public const int DefaultHealth = 100;
        public const int DefaultCaliber = 32;

        private List<Animation> animations = new List<Animation>();

        protected ParticleSource particleShit;

        public Pig(int x, int y, Direction direction, int health = DefaultHealth, int caliber = DefaultCaliber) : base()
        {
            this.Direction = direction;
            this.Health = health;
            this.Caliber = caliber;
            this.Speed = 110;

            this.animations.Add(new Animation(Assets.Images["PigRight"], 6, 8));
            this.animations.Add(new Animation(Assets.Images["PigRight"], 6, 8));
            this.animations.Add(new Animation(Assets.Images["PigLeft"], 6, 8));
            this.animations.Add(new Animation(Assets.Images["PigLeft"], 6, 8));
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
                new Tuple<int, int>(this.Direction == Direction.Right ? 5 : this.Width - 5, this.Height - 5),
                Direction.Down,
                1,
                Assets.ParticleTextureRegions["Shit"],
                MyColor.LighterBrown,
                MyColor.LightBrown,
                gravity: 60
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
            if (Tools.GetRandom(60) == 0)
            {
                this.particleShit.Run(50);
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
