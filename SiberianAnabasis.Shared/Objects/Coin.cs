using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SiberianAnabasis.Objects
{
    public class Coin : BaseObject
    {
        private static int moneyRowLimit = 30;
        private Animation anim;

        public Coin(int x, int y)
        {
            this.anim = new Animation(Assets.Images["Coin"], 4, 10);
            this.Hitbox = new Rectangle(x, y, this.anim.FrameWidth, this.anim.FrameHeight);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            this.anim.Loop = true;
            //this.Anim.ResetLoop();
            this.anim.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White);
        }

        public static void DrawStatic(SpriteBatch spriteBatch, int cost, int x, int y, float alpha = 1)
        {
            for (int i = 0; i < cost; i++)
            {
                spriteBatch.Draw(
                    Assets.Images["Coin"],
                    new Rectangle(
                        x + (Assets.Images["Coin"].Height + 1) * (i - (i >= moneyRowLimit ? moneyRowLimit : 0)),
                        y - Assets.Images["Coin"].Height * 2 + (i >= moneyRowLimit ? Assets.Images["Coin"].Height : 0),
                        Assets.Images["Coin"].Height,
                        Assets.Images["Coin"].Height
                    ),
                    new Rectangle(0, 0, Assets.Images["Coin"].Height, Assets.Images["Coin"].Height),
                    Color.White * alpha
                );
            }
        }
    }
}
