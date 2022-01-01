namespace MyGame.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    enum ButtonSize
    {
        Small = 30,
        Medium = 45,
        Large = 60,
    }

    class Button
    {
        private Texture2D staticTexture;
        private Texture2D clickedTexture;
        private Texture2D hoverTexture;

        private Texture2D Texture { get; set; }

        private int padding;
        private SpriteFont font;

        public bool Active { get; set; }

        public string Text { get; set; }

        public int AnimationTime { get; set; }

        public string Data { get; set; }

        public Rectangle Hitbox;

        public Button(int x, int y, int? width, ButtonSize size, string text, bool active = true, string data = "")
        {
            this.staticTexture = Assets.Button;
            this.clickedTexture = Assets.ButtonPressed;
            this.hoverTexture = Assets.ButtonHover;
            this.Texture = this.staticTexture;
            this.padding = 5;
            this.Data = data;
            this.Text = text;
            this.AnimationTime = 0;
            this.Active = active;

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
            if (this.Active && Controls.Mouse.HasBeenPressed(true) && this.Hitbox.Contains(Controls.Mouse.Position))
            {
                // perform click animation & return
                this.AnimationTime = 30;
                this.Texture = this.clickedTexture;
                return true;
            }

            return false;
        }

        public void Update()
        {

            if (this.Active && this.Hitbox.Contains(Controls.Mouse.Position))
            {
                this.Texture = this.hoverTexture;
            }
            else
            {
                if (this.AnimationTime > 0)
                {
                    this.AnimationTime--;
                }

                if (this.AnimationTime == 0)
                {
                    this.Texture = this.staticTexture;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!this.Active)
            {
                return;
            }

            spriteBatch.Draw(this.Texture, this.Hitbox, Color.White);
            spriteBatch.DrawString(this.font, this.Text, new Vector2(this.Hitbox.X + this.padding, this.Hitbox.Y + this.padding), Color.Black);
        }

        private int CalculateButtonSize()
        {
            Vector2 textSize = this.font.MeasureString(this.Text);
            return (int)textSize.X;
        }
    }
}
