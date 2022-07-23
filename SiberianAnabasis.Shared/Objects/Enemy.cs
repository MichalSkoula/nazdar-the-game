using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using SiberianAnabasis.Shared;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Enemy : BaseObject
    {
        private int speed = 110;

        private Animation anim;

        private List<Bullet> bullets = new List<Bullet>();

        private List<Animation> animations = new List<Animation>()
        {
            new Animation(Assets.EnemyRight, 4, 10),
            new Animation(Assets.EnemyRight, 4, 10),
            new Animation(Assets.EnemyLeft, 4, 10),
            new Animation(Assets.EnemyLeft, 4, 10),
        };

        public Enemy(int x, int y, Direction direction, int health = 100, int caliber = 10)
        {
            this.anim = this.animations[(int)Direction.Left];
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            this.Direction = direction;
            this.Health = health;
            this.Caliber = caliber;

            this.particleBlood = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Hitbox.Width / 2, this.Hitbox.Height / 2),
                Direction.Down,
                Color.Red,
                Color.DarkRed
            );
        }

        public override void Update(float deltaTime)
        {
            // is enemy moving?
            bool isMoving = true;
            if (this.Direction == Direction.Right)
            {
                this.X += (int)(deltaTime * this.speed);
            }
            else if (this.Direction == Direction.Left)
            {
                this.X -= (int)(deltaTime * this.speed);
            }
            else
            {
                isMoving = false;
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

            // out of game map
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ToDelete = true;
            }

            // particles
            this.particleBlood.Update(deltaTime, new Vector2(this.X, this.Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White);
            this.DrawHealth(spriteBatch);

            // bullets
            foreach (var bullet in this.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // particles
            this.particleBlood.Draw(spriteBatch);
        }
    }
}
