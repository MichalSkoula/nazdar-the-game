using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SiberianAnabasis.Screens;
using SiberianAnabasis.Shared;
using System;
using System.Threading.Tasks;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Bullet : BaseObject
    {
        private readonly int speed = 500;

        private ParticleSource particleSmoke;

        public Bullet(int x, int y, Direction direction, int caliber)
        {
            this.Direction = direction;
            this.Sprite = direction == Direction.Left ? Assets.BulletLeft : Assets.BulletRight;
            this.Hitbox = new Rectangle(direction == Direction.Left ? x - this.Sprite.Width : x, y, this.Sprite.Width, this.Sprite.Height);
            this.Caliber = caliber;

            Assets.Blip.Play();

            this.particleSmoke = new ParticleSource(
                new Vector2(this.X, this.Y),
                new Tuple<int, int>(this.Width / 2, this.Height / 2),
                direction == Direction.Left ? Direction.Right : Direction.Left,
                Assets.ParticleTextureRegionSmoke
            );
            this.particleSmoke.Run(50);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, Color.White);

            // particles
            this.particleSmoke.Draw(spriteBatch);
        }

        public override void Update(float deltaTime)
        {
            // move it
            if (this.Direction == Direction.Left)
            {
                this.X -= (int)(deltaTime * this.speed);
            }
            else if (this.Direction == Direction.Right)
            {
                this.X += (int)(deltaTime * this.speed);
            }

            // out of game map
            if (this.X < 0 || this.X > VillageScreen.MapWidth)
            {
                this.ToDelete = true;
            }

            this.particleSmoke.Update(deltaTime, this.Position);
        }
    }
}
