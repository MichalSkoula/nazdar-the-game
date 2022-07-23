using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Objects
{
    public class Coin : BaseObject
    {
        private Animation anim;

        public Coin(int x, int y)
        {
            this.anim = new Animation(Assets.Images["Coin"], 4, 10);
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
            
        }

        public override void Update(float deltaTime)
        {
            this.anim.Loop = true;
            //this.anim.ResetLoop();
            this.anim.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White);
        }
    }
}
