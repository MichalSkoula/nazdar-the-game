using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class BuildingSpot : BaseBuilding
    {
        public BuildingSpot(int x, int y, int width, int height, string type)
        {
            this.Hitbox = new Rectangle(x, y, width, height);
            this.Type = type switch
            {
                "Center" => Building.Type.Center,
                "Armory" => Building.Type.Armory,
                "Arsenal" => Building.Type.Arsenal,
                "Tower" => Building.Type.Tower,
                "Farm" => Building.Type.Farm,
                "Slum" => Building.Type.Slum,
                "Hospital" => Building.Type.Hospital,
                "Locomotive" => Building.Type.Locomotive,
                _ => throw new ArgumentException(),
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.Images[this.Type.ToString()], this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
