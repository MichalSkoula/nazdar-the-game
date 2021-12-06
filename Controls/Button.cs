using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame
{
    class Button
    {
        private Texture2D _staticTexture;
        private Texture2D _clickedTexture;
        private Texture2D _texture { get; set; }
        private int _padding;
        public Vector2 Position { get; set; }
        public Point Dimensions { get; set; }
        public string Text { get; set; }
        public int AnimationTime { get; set; }

        public Button(Vector2 position, Point dimensions, string text, bool autoresize = true)
        {
            _staticTexture = Assets.button;
            _clickedTexture = Assets.buttonPressed;
            _texture = _staticTexture;
            _padding = 5;
            Dimensions = dimensions;
            Position = position;
            Text = text;
            AnimationTime = 0;

            // resize button to fit text
            if (autoresize)
            {
                Vector2 textSize = Assets.font.MeasureString(Text);
                Dimensions = new Point((int)textSize.X + _padding * 2, (int)textSize.Y + _padding * 2);
            }
        }

        public bool HasBeenClicked()
        {
            if (!Controls.Mouse.HasBeenPressed(true))
            {
                return false;
            }

            if (Controls.Mouse.Position.X >= Position.X && Controls.Mouse.Position.X <= (Position.X + Dimensions.X))
            {
                if (Controls.Mouse.Position.Y >= Position.Y && Controls.Mouse.Position.Y <= (Position.Y + Dimensions.Y))
                {
                    // perform click animation & return
                    AnimationTime = 30;
                    _texture = _clickedTexture;
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            if (AnimationTime > 0)
            {
                AnimationTime--;
            }

            if (AnimationTime == 0)
            {
                _texture = _staticTexture;
            }
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y), Color.White);
            SpriteBatch.DrawString(Assets.font, Text, new Vector2(Position.X + _padding, Position.Y + _padding), Color.Black);
        }
    }
}
