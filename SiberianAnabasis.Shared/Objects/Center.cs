﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Center : BaseBuilding
    {
        public const int Cost = 1;
        public const string Name = "Center";

        public Center(int x, int y, Building.Status status)
        {
            this.Sprite = Assets.Images["Center"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuilt = 10;
            this.Type = Building.Type.Center;
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