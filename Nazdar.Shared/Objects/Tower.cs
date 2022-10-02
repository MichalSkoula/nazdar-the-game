using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Tower : BaseBuilding
    {
        public const int Cost = 5;
        public const string Name = "Tower";
        public bool CanShoot { get; set; } = false;
        private int shootPower = 1;
        private int shootRate = 90; // 0 fastest, 100 slowest

        public List<Bullet> Bullets { get; private set; }

        public Tower(int x, int y, Building.Status status)
        {
            this.Sprite = Assets.Images["Tower"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.Bullets = new List<Bullet>();
            this.TimeToBuilt = 5;
            this.Caliber = 35;
            this.Type = Building.Type.Tower;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);

            // bullets
            foreach (var bullet in this.Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void PrepareToShoot(Direction direction, int enemyX, int range)
        {
            this.Direction = direction;

            float howFar = Math.Abs(enemyX - this.X) / (float)range;
            this.shootPower = (int)(howFar * 10) / 2; // convert 0-1 float to 1-4 int
            if (this.shootPower == 0)
            {
                this.shootPower = 1;
            }
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (this.CanShoot && Game1.GlobalTimer % this.shootRate == 0)
            {
                this.Bullets.Add(new Bullet(
                    this.X + this.Width / 2,
                    this.Y + this.Height / 4,
                    this.Direction,
                    this.Caliber,
                    BulletType.Cannonball,
                    this.shootPower
                ));
            }

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }
            this.Bullets.RemoveAll(p => p.ToDelete);
        }
    }
}
