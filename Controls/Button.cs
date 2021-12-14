using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.Controls
{
    enum ButtonSize
    {
        small = 30,
        medium = 45,
        large = 60
    }

    class Button
    {
        private Texture2D _staticTexture;
        private Texture2D _clickedTexture;
        private Texture2D _hoverTexture;
        private Texture2D _texture { get; set; }
        private int _padding;
        private SpriteFont _font;
        public bool Active { get; set; }
        public string Text { get; set; }
        public int AnimationTime { get; set; }
        public string Data { get; set; }
        public Rectangle Hitbox;

        public Button(int x, int y, int? width, ButtonSize size, string text, bool active = true, string data = "")
        {
            _staticTexture = Assets.button;
            _clickedTexture = Assets.buttonPressed;
            _hoverTexture = Assets.buttonHover;
            _texture = _staticTexture;
            _padding = 5;
            Data = data;
            Text = text;
            AnimationTime = 0;
            Active = active;

            // add font
            switch (size)
            {
                case ButtonSize.small:
                    _font = Assets.fontSmall;
                    break;
                case ButtonSize.medium:
                    _font = Assets.fontMedium;
                    break;
                case ButtonSize.large:
                    _font = Assets.fontLarge;
                    break;
            }

            // calculate size 
            Hitbox = new Rectangle(
                x, y, 
                (width.HasValue ? (int)width : _calculateButtonSize()) + _padding * 2, (int)size
            );            
        }

        private int _calculateButtonSize()
        {
            Vector2 textSize = _font.MeasureString(Text);
            return (int)textSize.X;
        }

        public bool HasBeenClicked()
        {
            if (Active && Controls.Mouse.HasBeenPressed(true) && Hitbox.Contains(Controls.Mouse.Position)) { 
                // perform click animation & return
                AnimationTime = 30;
                _texture = _clickedTexture;
                return true;
            }

            return false;
        }

        public void Update()
        {

            if (Active && Hitbox.Contains(Controls.Mouse.Position))
            {
                _texture = _hoverTexture;
            }
            else
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
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            if (!Active)
            {
                return;
            }

            SpriteBatch.Draw(_texture, Hitbox, Color.White);
            SpriteBatch.DrawString(_font, Text, new Vector2(Hitbox.X + _padding, Hitbox.Y + _padding), Color.Black);
        }
    }
}
