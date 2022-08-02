using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using SiberianAnabasis.Shared;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Peasant : BaseObject
    {
        private bool isFast = false;
        private int speed = 61;
        private int basecampRadius = 128;

        private Animation anim;

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.Images["PeasantRight"], 4, 10),
            new Animation(Assets.Images["PeasantRight"], 4, 10),
            new Animation(Assets.Images["PeasantLeft"], 4, 10),
            new Animation(Assets.Images["PeasantLeft"], 4, 10),
        };

        public Peasant(int x, int y, Direction direction, int health = 100)
        {
            this.anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Direction = direction;
            this.Health = health;
            this.Alpha = 1f;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // is he moving?
            bool isMoving = false;
            if (Tools.GetRandom(8) < 2 || this.isFast)
            {
                if (this.Direction == Direction.Right)
                {
                    this.X += (int)(deltaTime * this.speed);
                    isMoving = true;
                }
                else if (this.Direction == Direction.Left)
                {
                    this.X -= (int)(deltaTime * this.speed);
                    isMoving = true;
                }
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

            // always run towards basecamp
            if (this.X < VillageScreen.MapWidth / 2 - this.basecampRadius)
            {
                this.Direction = Direction.Right;
            } 
            else if (this.X > VillageScreen.MapWidth / 2 + this.basecampRadius)
            {
                this.Direction = Direction.Left;
            }

            // fast, but when near the base, can be slow and randomly change direction
            if (this.X < VillageScreen.MapWidth / 2 + this.basecampRadius && this.X > VillageScreen.MapWidth / 2 - this.basecampRadius)
            {
                this.isFast = false;
                if (Tools.GetRandom(128) < 2)
                {
                    this.ChangeDirection();
                }
            } 
            else
            {
                this.isFast = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, this.FinalColor);
            this.DrawHealth(spriteBatch);
        }
    }
}
