using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Building : BaseObject
    {
        public Building(int x, int y, int width, int height)
        {
            //this.Sprite = Assets.Building;
            //this.Hitbox = new Rectangle(x, y - (this.Sprite.Height / 2), this.Sprite.Width, this.Sprite.Height);
            this.Hitbox = new Rectangle(x, y, width, height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, Color.Wheat);
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
