using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class ControlsScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public ControlsScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu", true));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Button.UpdateButtons(this.buttons);

            // main menu
            if (this.buttons.GetValueOrDefault("menu").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Controls", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            //this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Keyboard", new Vector2(Offset.MenuX, Offset.MenuY + 55), MyColor.White);

            int topOffset = -50;

            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardLeft"], new Vector2(Offset.MenuX, 120 + topOffset), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardRight"], new Vector2(Offset.MenuX + 60, 120 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "movement", new Vector2(Offset.MenuX + 130, 135 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardSpace"], new Vector2(Offset.MenuX, 180 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "shoot", new Vector2(Offset.MenuX + 130, 195 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardUp"], new Vector2(Offset.MenuX, 240 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "jump", new Vector2(Offset.MenuX + 130, 255 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardDown"], new Vector2(Offset.MenuX, 300 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "action", new Vector2(Offset.MenuX + 130, 315 + topOffset), MyColor.White);

            //this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Gamepad", new Vector2(Offset.MenuX + 300, Offset.MenuY + 55), MyColor.White);

            this.Game.SpriteBatch.Draw(Assets.Images["GamepadLeftStick"], new Vector2(Offset.MenuX + 300, 116 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "movement", new Vector2(Offset.MenuX + 370, 131 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadX"], new Vector2(Offset.MenuX + 304, 180 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "shoot", new Vector2(Offset.MenuX + 370, 195 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadY"], new Vector2(Offset.MenuX + 304, 240 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "jump", new Vector2(Offset.MenuX + 370, 255 + topOffset), MyColor.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadA"], new Vector2(Offset.MenuX + 304, 300 + topOffset), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "action", new Vector2(Offset.MenuX + 370, 315 + topOffset), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}
