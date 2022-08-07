using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class BuildingSpot : BaseBuilding
    {
        public Building.Type Type;
        public int Cost;

        public BuildingSpot(int x, int y, int width, int height, string type)
        {
            this.Hitbox = new Rectangle(x, y, width, height);
            
            switch (type)
            {
                case "Center":
                    this.Type = Building.Type.Center;
                    this.Cost = Center.Cost;
                    break;
                case "Armory":
                    this.Type = Building.Type.Armory;
                    this.Cost = Armory.Cost;
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
