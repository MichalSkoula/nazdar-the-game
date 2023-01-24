using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Objects
{
    public class Market : BaseBuilding
    {
        public const int Cost = 10;
        public const string Name = "Market";

        public Market(int x, int y, Building.Status status, float ttb = 8) : base()
        {
            this.Sprite = Assets.Images["Market"];
            this.Anim = new Animation(Assets.Images["MarketActive"], 3, 1);
            this.Hitbox = new Rectangle(x, y, this.Sprite.Width, this.Sprite.Height);
            this.Status = status;
            this.TimeToBuild = ttb;
            this.Type = Building.Type.Market;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw sprite and on that, draw animation?
            spriteBatch.Draw(this.Sprite, this.Hitbox, this.FinalColor);
            if (this.Status == Building.Status.Built)
            {
                this.Anim.Draw(spriteBatch, this.Hitbox);
            }
        }

        public void Update(float deltaTime, List<Coin> coins)
        {
            base.Update(deltaTime);

            // anim change frame
            if (this.Status == Building.Status.Built)
            {
                this.Anim.Loop = true;
            }
            else
            {
                this.Anim.Loop = false;
                this.Anim.ResetLoop();
            }
            this.Anim.Update(deltaTime);

            // generate money
            if (this.Status == Building.Status.Built && Tools.GetRandom(VillageScreen.marketMoneyProbability) == 1)
            {
                coins.Add(new Coin(this.X + Tools.GetRandom(this.Width), Offset.Floor2));
            }
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox,
                this.Status,
                this.TimeToBuild
            };
        }
    }
}
