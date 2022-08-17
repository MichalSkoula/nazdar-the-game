using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using SiberianAnabasis.Shared;
using System;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Bullet : BaseObject
    {
        private readonly int speed = 400;
        private readonly int g = 10;
        private int h0;
        private float t0;

        public Bullet(int x, int y, Direction direction, int caliber)
        {
            this.Direction = direction;
            this.Sprite = direction == Direction.Left ? Assets.Images["BulletLeft"] : Assets.Images["BulletRight"];
            this.Hitbox = new Rectangle(direction == Direction.Left ? x - this.Sprite.Width : x, y, this.Sprite.Width, this.Sprite.Height);
            this.Caliber = caliber;

            this.h0 = y;
            this.t0 = 0;
            this.ColorDead = Color.Gray;

            Audio.PlaySound("Blip");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (this.Dead)
            {
                return;
            }

            // move it
            if (this.Direction == Direction.Left)
            {
                this.X -= (int)(deltaTime * this.speed);
            }
            else if (this.Direction == Direction.Right)
            {
                this.X += (int)(deltaTime * this.speed);
            }

            // fall
            this.t0 += deltaTime / 1.5f;
            this.Y = (int)(this.h0 + 0.5f * g * Math.Pow(this.t0, 2));

            // out of game map
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ToDelete = true;
            }
            
            // felt down - keep it for some time
            if ((this.Y - this.Height) > Offset.Floor2)
            {
                this.Dead = true;
            }
        }
    }
}
