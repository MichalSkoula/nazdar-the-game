using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class BuildingSpot : BaseBuilding
    {
        public BuildingSpot(int x, int y, int width, int height, string type)
        {
            this.Hitbox = new Rectangle(x, y, width, height);
            
            switch (type)
            {
                case "Center":
                    this.Type = Building.Type.Center;
                    break;
                case "Armory":
                    this.Type = Building.Type.Armory;
                    break;
                case "Tower":
                    this.Type = Building.Type.Tower;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(this.Hitbox, Color.Wheat * 0.1f);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
