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

        

        public Bullet(int x, int y, Direction direction, int caliber)
        {
            this.Direction = direction;
            this.Sprite = direction == Direction.Left ? Assets.Images["BulletLeft"] : Assets.Images["BulletRight"];
            this.Hitbox = new Rectangle(direction == Direction.Left ? x - this.Sprite.Width : x, y, this.Sprite.Width, this.Sprite.Height);
            this.Caliber = caliber;

            Assets.Sounds["Blip"].Play();

           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, Color.White);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

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
        }
    }
}
