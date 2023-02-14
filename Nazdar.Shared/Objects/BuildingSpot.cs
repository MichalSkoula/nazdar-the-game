using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Shared;
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

            if (this.Type == Building.Type.Slum)
            {
                this.Sprite = Assets.Images["Slum" + (Tools.GetRandom(2) + 1)];
            }
            else
            {
                this.Sprite = Assets.Images.ContainsKey(type + "Ruins") ? Assets.Images[type + "Ruins"] : Assets.Images[type];
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Hide)
            {
                return;
            }

            spriteBatch.Draw(
                this.Sprite,
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
