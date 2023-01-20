using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Shared;
using System;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Tower : BaseBuilding
    {
        public const int Cost = 8;
        public const string Name = "Armored Railcar";
        private int shootPower = 1;
        private int shootRate = 95; // 0 fastest, 100 slowest
        public bool CanFire { get; set; }

        public Tower(int x, int y, Building.Status status, float ttb = 4) : base()
        {
            this.Sprite = Assets.Images["Tower"];
            this.Anim = new Animation(Assets.Images["TowerFiring"], 4, 6);
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Caliber = 20;
            this.Type = Building.Type.Tower;

            this.particleSmoke = new ParticleSource(
               new Vector2(this.X, this.Y),
               new Tuple<int, int>(0, this.Height / 2),
               Direction.Up,
               0.5f,
               Assets.ParticleTextureRegions["Smoke"]
            );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw sprite and on that, draw animation?
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            if (this.CanFire)
            {
                this.Anim.Draw(spriteBatch, this.Hitbox);
            }

            this.particleSmoke.Draw(spriteBatch);

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

            if (Game1.GlobalTimer % this.shootRate == 1)
            {
                this.Bullets.Add(new Bullet(
                    this.X + (int)(this.Width * (this.Direction == Direction.Left ? 0.25f : 0.75f)),
                    this.Y + this.Height / 4,
                    this.Direction,
                    this.Caliber,
                    BulletType.Cannonball,
                    this.shootPower
                ));

                Audio.PlaySound("CannonShoot");
                this.particleSmoke.Run(75);
            }
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            foreach (var bullet in this.Bullets)
            {
                bullet.Update(deltaTime);
            }
            this.Bullets.RemoveAll(p => p.ToDelete);

            // anim change frame
            if (this.CanFire)
            {
                this.Anim.Loop = true;
            }
            else
            {
                this.Anim.Loop = false;
                this.Anim.ResetLoop();
            }
            this.Anim.Update(deltaTime);

            this.particleSmoke.Update(deltaTime, new Vector2(this.X + (this.Direction == Direction.Left ? 0 : this.Width), this.Y));
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Status,
                Bullets = this.Bullets.Select(b => new { b.Hitbox, b.Direction, b.Caliber }).ToList(),
                this.TimeToBuild
            };
        }
    }
}
