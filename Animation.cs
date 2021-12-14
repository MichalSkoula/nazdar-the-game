using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame
{
    class Animation
    {
        private int _count;
        private int _step;
        private int _fps;
        private float _timeElapsed;
        private float _timeToUpdate;
        private Texture2D _tiles;
        public bool Loop
        {
            get; set;
        }

        public int FrameWidth {
            get { return _tiles.Width / _count; }
        }
        public int FrameHeight { 
            get { return _tiles.Height; }
        }
        
        public Animation(Texture2D tiles, int count, int fps, bool loop = true)
        {
            _count = count;
            _tiles = tiles;
            _step = 0;
            _fps = fps;
            _timeToUpdate = (1f / _fps);
            Loop = Loop;
        }

        public void ResetLoop()
        {
            _step = 0;
        }

        public void Draw(SpriteBatch SpriteBatch, Rectangle position)
        {
            SpriteBatch.Draw(_tiles, position, new Rectangle(_step * FrameWidth, 0, FrameWidth, FrameHeight), Color.White);
        }

        public void Update(float deltaTime)
        {
            if (!Loop)
            {
                return;
            }

            _timeElapsed += deltaTime;
            if (_timeElapsed > _timeToUpdate)
            {
                _timeElapsed -= _timeToUpdate;

                if (_step + 1 < _count)
                {
                    _step++;
                }
                else if (Loop)
                {
                    _step = 0;
                }  
            }
        }
    }
}
