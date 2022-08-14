using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using SiberianAnabasis.Shared;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Peasant : BasePerson
    {
        private bool isFast = false;
        private int centerRadius = 96;
        public Rectangle? IsBuildingHere = null;
        public Rectangle? IsRunningForItem = null;

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["PeasantRight"], 4, 10),
            new Animation(Assets.Images["PeasantRight"], 4, 10),
            new Animation(Assets.Images["PeasantLeft"], 4, 10),
            new Animation(Assets.Images["PeasantLeft"], 4, 10),
        };

        public Peasant(int x, int y, Direction direction, int caliber = 2)
        {
            this.Anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.Anim.FrameWidth, this.Anim.FrameHeight);
            this.Direction = direction;
            this.Health = 100;
            this.Alpha = 1f;
            this.Caliber = caliber;
            this.Speed = 61;

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

            // is he moving?
            bool isMoving = false;
            if (Tools.GetRandom(8) < 2 || this.isFast)
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

            
            if (this.IsBuildingHere.HasValue)
            {
                // 1/ is he building something? should run there
                if (this.X < IsBuildingHere.GetValueOrDefault().X)
                {
                    this.Direction = Direction.Right;
                }
                if (this.X >= (IsBuildingHere.GetValueOrDefault().X + IsBuildingHere.GetValueOrDefault().Width - this.Width))
                {
                    this.Direction = Direction.Left;
                }
            }
            else if (this.IsRunningForItem.HasValue)
            {
                // 2/ is he going to get weapon etc? 
                if (this.X < IsRunningForItem.GetValueOrDefault().X)
                {
                    this.Direction = Direction.Right;
                }
                if (this.X >= (IsRunningForItem.GetValueOrDefault().X + IsRunningForItem.GetValueOrDefault().Width - this.Width))
                {
                    this.Direction = Direction.Left;
                }
            }
            else
            {
                // 3/ otherwise always run towards town center
                if (this.X < VillageScreen.MapWidth / 2 - this.centerRadius)
                {
                    this.Direction = Direction.Right;
                }
                else if (this.X > VillageScreen.MapWidth / 2 + this.centerRadius)
                {
                    this.Direction = Direction.Left;
                }

                // when near the base, can be slow and randomly change direction
                if (this.X < VillageScreen.MapWidth / 2 + this.centerRadius && this.X > VillageScreen.MapWidth / 2 - this.centerRadius)
                {
                    this.isFast = false;
                    if (Tools.GetRandom(128) < 2)
                    {
                        this.ChangeDirection();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            //this.DrawHealth(spriteBatch);
            // particles
            this.particleBlood.Draw(spriteBatch);
        }
    }
}
