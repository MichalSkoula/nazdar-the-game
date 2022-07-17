using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Homeless : BaseObject
    {
        private int speed = 61;

        private Animation anim;

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.HomelessRight, 4, 10),
            new Animation(Assets.HomelessRight, 4, 10),
            new Animation(Assets.HomelessLeft, 4, 10),
            new Animation(Assets.HomelessLeft, 4, 10),
        };

        private readonly Random rand = new Random();

        public bool ToDelete { get; set; }

        public Homeless(int x, int y, Direction direction, int health = 100)
        {
            this.anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Direction = direction;
            this.Health = health;
        }

        public override void Update(float deltaTime)
        {
            // is he moving?
            bool isMoving = false;
            if (rand.Next(4) < 2 /*Game1.GlobalTimer % 2 == 0*/)
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

            // out of game map - change direction
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ChangeDirection();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White * 0.75f);
            this.DrawHealth(spriteBatch, 0.75f);
        }
    }
}
