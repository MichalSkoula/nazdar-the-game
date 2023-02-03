using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Objects;

namespace Nazdar.Shared.Parallax
{
    internal class Layer : BaseObject
    {
        private readonly Texture2D texture;
        private Vector2 positionOld = Vector2.Zero;
        private Vector2 position;
        private readonly float speed;
        public readonly int Depth;
        private int mapWidth;
        private float defaultSpeed;

        public Layer(Texture2D texture, int depth, float speed, float defaultSpeed, int mapWidth, float alpha)
        {
            this.texture = texture;
            Depth = depth;
            this.speed = speed;
            this.mapWidth = mapWidth;
            this.Color = Color.White;
            this.Alpha = alpha;
            this.defaultSpeed = defaultSpeed;
        }

        public void Update(int playerPosition, Color parallaxColor)
        {
            this.Color = parallaxColor;

            if (this.positionOld.X != playerPosition || this.defaultSpeed != 0)
            {
                this.position.X += (this.positionOld.X - playerPosition + this.defaultSpeed) * this.speed;
                //Tools.Dump((this.positionOld.X - playerPosition) * this.speed);
            }

            // too much right?
            if (this.position.X > this.texture.Width)
            {
                this.position.X -= this.texture.Width;
            }

            this.positionOld.X = playerPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = -1; i <= this.mapWidth / this.texture.Width + 1; i++)
            {
                spriteBatch.Draw(this.texture, new Vector2((int)(this.position.X + this.texture.Width * i), this.position.Y), this.FinalColor);
            }
        }
    }
}
