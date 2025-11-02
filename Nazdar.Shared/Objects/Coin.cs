using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Objects
{
    public class Coin : BaseObject
    {
        private readonly Animation anim;

        public Coin(int x, int y)
        {
            this.anim = new Animation(Assets.Images["Coin"], 4, 10);
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            this.anim.Loop = true;
            this.anim.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White);
        }

        public object GetSaveData()
        {
            return new
            {
                this.Hitbox
            };
        }
    }
}
