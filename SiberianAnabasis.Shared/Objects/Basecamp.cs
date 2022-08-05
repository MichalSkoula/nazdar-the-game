using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static SiberianAnabasis.Enums;
using Microsoft.Xna.Framework;

namespace SiberianAnabasis.Objects
{
    public class Basecamp : BaseObject
    {
        public const int Cost = 1;
        public float TimeToBuilt = 10;
        public Building.Type Type = Building.Type.Basecamp;
        public Building.Status Status = Building.Status.InProcess;

        public Basecamp(int x, int y, Building.Status status)
        {
            this.Sprite = Assets.Images["Basecamp"];
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
        }

        public new void Update(float deltaTime)
        {
            this.Alpha = this.Status == Building.Status.InProcess ? 0.25f : 1;

            if (this.TimeToBuilt > 0 && this.IsBeingBuilt)
            {
                this.TimeToBuilt -= deltaTime;
            }

            if (this.TimeToBuilt <= 0)
            {
                this.Status = Building.Status.Built;
            }

            base.Update(deltaTime);            
        }
    }
}
