using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class BuildingSpot : BaseBuilding
    {
        public bool Hide { get; set; }

        public BuildingSpot(int x, int y, int width, int height, string type, bool hide = false)
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
                "Market" => Building.Type.Market,
                "Rails" => Building.Type.Rails,
                "Ship" => Building.Type.Ship,
                _ => throw new ArgumentException(),
            };
            this.Hide = hide;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Hide)
            {
                return;
            }

            string image = this.Type.ToString();

            spriteBatch.Draw(
                Assets.Images.ContainsKey(image + "Ruins") ? Assets.Images[image + "Ruins"] : Assets.Images[image],
                this.Hitbox,
                this.Type == Building.Type.Slum ? Color.White : this.FinalColor
            );
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
