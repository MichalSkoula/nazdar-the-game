using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class BuildingSpot : BaseObject
    {
        public Building.Type Type;
        public int Cost;

        public BuildingSpot(int x, int y, int width, int height, string type)
        {
            this.Hitbox = new Rectangle(x, y, width, height);
            
            switch (type)
            {
                case "Basecamp":
                    this.Type = Building.Type.Basecamp;
                    this.Cost = Basecamp.Cost;
                    break;
                case "Armory":
                    this.Type = Building.Type.Armory;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle(this.Hitbox, Color.Wheat * 0.2f);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
