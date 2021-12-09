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
        public string Text { get; set; }
        public int AnimationTime { get; set; }
        public Rectangle Hitbox;

        public Button(int x, int y, int width, int height, string text, bool autoresize = true)
        {
            _staticTexture = Assets.button;
            _clickedTexture = Assets.buttonPressed;
            _texture = _staticTexture;
            _padding = 5;
            Text = text;
            AnimationTime = 0;
            Hitbox = new Rectangle(x, y, width, height);

            // resize button to fit text
            if (autoresize)
            {
                Vector2 textSize = Assets.font.MeasureString(Text);
                Hitbox.Width = (int)textSize.X + _padding * 2;
                Hitbox.Height = (int)textSize.Y + _padding * 2;
            }
        }

        public bool HasBeenClicked()
        {
            if (Controls.Mouse.HasBeenPressed(true) && Hitbox.Contains(new Point(Controls.Mouse.Position.X, Controls.Mouse.Position.Y))) { 
                // perform click animation & return
                AnimationTime = 30;
                _texture = _clickedTexture;
                return true;
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
            SpriteBatch.Draw(_texture, Hitbox, Color.White);
            SpriteBatch.DrawString(Assets.font, Text, new Vector2(Hitbox.X + _padding, Hitbox.Y + _padding), Color.Black);
        }
    }
}
