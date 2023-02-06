using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Nazdar.Shared;
using System.Collections.Generic;

namespace Nazdar.Controls
{
    public class Button
    {
        private readonly int padding;
        private readonly SpriteFont font;
        private Enums.ButtonState state;

        public string Text { get; set; }
        private string DefaultText { get; set; }

        public Rectangle Hitbox { get; private set; }

        public bool Focus = false;

        private bool clicked = false;
        public bool Clicked
        {
            get
            {
                return clicked;
            }
            set
            {
                Audio.PlayRandomSound("Clicks", 0.25f);
                clicked = value;
            }
        }

        public bool Active { get; set; }

        public string[] Data { get; set; }

        public Button(int x, int y, int? width, Enums.ButtonSize size, string defaultText, bool focus = false, string[] data = null, bool active = true, string text = "")
        {
            this.state = Enums.ButtonState.StaticState;
            this.padding = 5;
            this.Text = text;
            this.DefaultText = defaultText;
            this.Focus = focus;
            this.Data = data;
            this.Active = active;

            // add font
            switch (size)
            {
                case Enums.ButtonSize.Small:
                    this.font = Assets.Fonts["Small"];
                    break;
                case Enums.ButtonSize.Medium:
                    this.font = Assets.Fonts["Medium"];
                    break;
                case Enums.ButtonSize.Large:
                    this.font = Assets.Fonts["Large"];
                    break;
            }

            // calculate size
            this.Hitbox = new Rectangle(
                x, y, (width.HasValue ? (int)width : this.CalculateButtonSize()) + (this.padding * 2), (int)size
            );
        }

        public bool HasBeenClicked()
        {
            if (this.Active == false)
            {
                return false;
            }

            if ((Mouse.HasBeenPressed(true) && this.Hitbox.Contains(Mouse.Position)) || this.Clicked)
            {
                this.Clicked = false;
                return true;
            }

            if (Touch.HasBeenPressed(this.Hitbox) || this.Clicked)
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
                this.state = Enums.ButtonState.HoverState;
            }
            else if (this.Focus)
            {
                this.state = Enums.ButtonState.HoverState;
            }
            else
            {
                this.state = Enums.ButtonState.StaticState;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var bgColor = MyColor.Purple;

            if (this.Active == false)
            {
                bgColor = MyColor.Gray3;
            }
            else if (this.state == Enums.ButtonState.HoverState)
            {
                bgColor = MyColor.White;
            }

            spriteBatch.DrawRectangle(this.Hitbox, bgColor, (this.Hitbox.Height / 2) + 5);
            spriteBatch.DrawString(this.font, this.DefaultText + (this.Text.Length > 0 ? " " + this.Text : ""), new Vector2(this.Hitbox.X + this.padding, this.Hitbox.Y + this.padding), MyColor.Black);
        }

        private int CalculateButtonSize()
        {
            Vector2 textSize = this.font.MeasureString(this.DefaultText + (this.Text.Length > 0 ? this.Text + " " : ""));
            return (int)textSize.X;
        }

        /// <summary>
        /// Updates, iterates and "click" buttons
        /// </summary>
        public static void UpdateButtons(Dictionary<string, Button> buttons)
        {
            // update buttons
            foreach (KeyValuePair<string, Button> button in buttons)
            {
                button.Value.Update();
            }

            // iterate through buttons up/down
            if (Controls.Keyboard.HasBeenPressed(Keys.Down) || Controls.Gamepad.HasBeenPressed(Buttons.DPadDown) || Controls.Gamepad.HasBeenPressedThumbstick(Enums.Direction.Down))
            {
                Tools.ButtonsIterateWithKeys(Enums.Direction.Down, buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up) || Controls.Gamepad.HasBeenPressed(Buttons.DPadUp) || Controls.Gamepad.HasBeenPressedThumbstick(Enums.Direction.Up))
            {
                Tools.ButtonsIterateWithKeys(Enums.Direction.Up, buttons);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter) || Controls.Gamepad.HasBeenPressed(Buttons.A))
            {
                foreach (KeyValuePair<string, Button> button in buttons)
                {
                    if (button.Value.Focus)
                    {
                        button.Value.Clicked = true;
                        break;
                    }
                }
            }
        }
    }
}
