using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using static MyGame.Enums;

namespace MyGame.Controls
{
    public class Button
    {
        private int padding;
        private SpriteFont font;
        private ButtonState state;

        public string Text { get; set; }

        public Rectangle Hitbox { get; private set; }

        public bool Focus = false;

        public bool Clicked = false;

        public string[] Data { get; set; }

        public Button(int x, int y, int? width, ButtonSize size, string text, bool focus = false, string[] data = null)
        {
            this.state = ButtonState.StaticState;
            this.padding = 5;
            this.Text = text;
            this.Focus = focus;
            this.Data = data;

            // add font
            switch (size)
            {
                case ButtonSize.Small:
                    this.font = Assets.FontSmall;
                    break;
                case ButtonSize.Medium:
                    this.font = Assets.FontMedium;
                    break;
                case ButtonSize.Large:
                    this.font = Assets.FontLarge;
                    break;
            }

            // calculate size
            this.Hitbox = new Rectangle(
                x, y, (width.HasValue ? (int)width : this.CalculateButtonSize()) + (this.padding * 2), (int)size
            );
        }

        public bool HasBeenClicked()
        {
            if ((Mouse.HasBeenPressed(true) && this.Hitbox.Contains(Mouse.Position)) || this.Clicked)
            {
                this.Clicked = false;
                return true;
            }

            return false;
        }

        public void Update()
        {
            if (this.Hitbox.Contains(Mouse.Position))
            {
                this.state = ButtonState.HoverState;
            }
            else if (this.Focus) 
            {
                this.state = ButtonState.HoverState;
            }
            else
            {
                this.state = ButtonState.StaticState;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var bgColor = Color.Green;
            if (this.state == ButtonState.HoverState)
            {
                bgColor = Color.LightGreen;
            }

            spriteBatch.DrawRectangle(this.Hitbox, bgColor, (this.Hitbox.Height / 2) + 5);
            spriteBatch.DrawString(this.font, this.Text, new Vector2(this.Hitbox.X + this.padding, this.Hitbox.Y + this.padding), Color.Black);
        }

        private int CalculateButtonSize()
        {
            Vector2 textSize = this.font.MeasureString(this.Text);
            return (int)textSize.X;
        }
    }
}
