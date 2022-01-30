namespace MyGame.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoGame.Extended;

    class Button
    {
        private int padding;
        private SpriteFont font;
        private Enums.ButtonState state;

        public bool Active { get; set; }

        public string Text { get; set; }

        public int AnimationTime { get; set; }

        private Rectangle hitbox;

        public Button(int x, int y, int? width, Enums.ButtonSize size, string text, bool active = true)
        {
            this.state = Enums.ButtonState.StaticState;
            this.padding = 5;
            this.Text = text;
            this.AnimationTime = 0;
            this.Active = active;

            // add font
            switch (size)
            {
                case Enums.ButtonSize.Small:
                    this.font = Assets.FontSmall;
                    break;
                case Enums.ButtonSize.Medium:
                    this.font = Assets.FontMedium;
                    break;
                case Enums.ButtonSize.Large:
                    this.font = Assets.FontLarge;
                    break;
            }

            // calculate size
            this.hitbox = new Rectangle(
                x, y, (width.HasValue ? (int)width : this.CalculateButtonSize()) + (this.padding * 2), (int)size
            );
        }

        public bool HasBeenClicked()
        {
            if (this.Active && Controls.Mouse.HasBeenPressed(true) && this.hitbox.Contains(Controls.Mouse.Position))
            {
                // perform click animation & return
                this.AnimationTime = 30;
                this.state = Enums.ButtonState.ClickedState;
                return true;
            }

            return false;
        }

        public void Update()
        {
            if (this.Active && this.hitbox.Contains(Controls.Mouse.Position))
            {
                this.state = Enums.ButtonState.HoverState;
            }
            else
            {
                if (this.AnimationTime > 0)
                {
                    this.AnimationTime--;
                }

                if (this.AnimationTime == 0)
                {
                    this.state = Enums.ButtonState.StaticState;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!this.Active)
            {
                return;
            }

            var bgColor = Color.Green;
            if (this.state == Enums.ButtonState.ClickedState)
            {
                bgColor = Color.DarkGreen;
            }
            else if (this.state == Enums.ButtonState.HoverState)
            {
                bgColor = Color.LightGreen;
            }

            spriteBatch.DrawRectangle(this.hitbox, bgColor, (this.hitbox.Height / 2) + 5);
            spriteBatch.DrawString(this.font, this.Text, new Vector2(this.hitbox.X + this.padding, this.hitbox.Y + this.padding), Color.Black);
        }

        private int CalculateButtonSize()
        {
            Vector2 textSize = this.font.MeasureString(this.Text);
            return (int)textSize.X;
        }
    }
}
