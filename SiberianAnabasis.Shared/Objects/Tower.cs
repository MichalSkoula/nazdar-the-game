using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static SiberianAnabasis.Enums;
using Microsoft.Xna.Framework;

namespace SiberianAnabasis.Objects
{
    public class Tower : BaseBuilding
    {
        public const int Cost = 3;
        public const string Name = "Tower";

        public Tower(int x, int y, Building.Status status)
        {
            this.Sprite = Assets.Images["Tower"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 5;
            this.Type = Building.Type.Tower;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
