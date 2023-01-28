using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Objects
{
    public class Coin : BaseObject
    {
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
            this.anim.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.anim.Draw(spriteBatch, this.Hitbox, Color.White);
        }

        public static void DrawStatic(SpriteBatch spriteBatch, int howMany, int x, int y, float alpha = 1, bool goUpIfTooMuchCoins = false)
        {
            int sign = goUpIfTooMuchCoins ? -1 : 1;

            for (int i = 0; i < howMany; i++)
            {
                spriteBatch.Draw(
                    Assets.Images["CoinStatic"],
                    new Rectangle(
                        x + (Assets.Images["CoinStatic"].Width + 1) * (i % Enums.Offset.RowLimit),
                        y - Assets.Images["CoinStatic"].Height * 2 + sign * (i / Enums.Offset.RowLimit * Assets.Images["CoinStatic"].Height) + Game1.Salt[i],
                        Assets.Images["CoinStatic"].Width,
                        Assets.Images["CoinStatic"].Height
                    ),
                    Color.White * alpha
                );
            }
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
