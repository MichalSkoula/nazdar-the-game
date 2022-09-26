using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
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
        private BulletType type;

        // 1 close, 4 faaar - bigger shootPower => farther it falls (it falls less often)
        // only for cannonbal
        private int shootPower = 1;

        public Bullet(int x, int y, Direction direction, int caliber, BulletType type = BulletType.Bullet, int shootPower = 1)
        {
            this.Direction = direction;
            this.Sprite = direction == Direction.Left ? Assets.Images["BulletLeft"] : Assets.Images["BulletRight"];
            this.Hitbox = new Rectangle(direction == Direction.Left ? x - this.Sprite.Width : x, y, this.Sprite.Width, this.Sprite.Height);
            this.Caliber = caliber;
            this.type = type;

            this.h0 = y;
            this.t0 = 0;
            this.shootPower = shootPower;

            this.ColorDead = Color.Gray;
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
            if (this.type == BulletType.Bullet)
            {
                this.t0 += deltaTime / 1.5f;
                this.Y = (int)(this.h0 + 0.5f * g * Math.Pow(this.t0, 2));
            }
            else if (this.type == BulletType.Cannonball)
            {
                // bigger shootPower => farther it falls (it falls less often)
                if (Game1.GlobalTimer % this.shootPower == 0)
                {
                    this.Y += 1;
                }
            }

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
