using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar
{
    public class Animation
    {
        private readonly int count;
        private int step;
        private readonly int fps;
        private float timeElapsed;
        private readonly float timeToUpdate;
        private readonly Texture2D tiles;

        public bool Loop
        {
            get; set;
        }

        public int FrameWidth
        {
            get { return this.tiles.Width / this.count; }
        }

        public int FrameHeight
        {
            get { return this.tiles.Height; }
        }

        public Animation(Texture2D tiles, int count, int fps, bool loop = true)
        {
            this.count = count;
            this.tiles = tiles;
            this.step = 0;
            this.fps = fps;
            this.timeToUpdate = 1f / this.fps;
            this.Loop = this.Loop;
        }

        public void ResetLoop()
        {
            this.step = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle position, Color? color = null)
        {
            spriteBatch.Draw(this.tiles, position, new Rectangle(this.step * this.FrameWidth, 0, this.FrameWidth, this.FrameHeight), color.HasValue ? (Color)color : Color.White);
        }

        public void Update(float deltaTime)
        {
            if (!this.Loop)
            {
                return;
            }

            this.timeElapsed += deltaTime;
            if (this.timeElapsed > this.timeToUpdate)
            {
                this.timeElapsed -= this.timeToUpdate;

                if (this.step + 1 < this.count)
                {
                    this.step++;
                }
                else if (this.Loop)
                {
                    this.step = 0;
                }
            }
        }
    }
}
