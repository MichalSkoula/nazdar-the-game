using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class ControlsScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public ControlsScreen(Game1 game) : base(game) { }

        private readonly Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, Translation.Get("menu.backToMenu"), true));

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

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], Translation.Get("controls.title"), new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            DrawScheme(this.Game.SpriteBatch, Offset.MenuX, 70);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }

        // keyboard and gamepad scheme, also shown over the village before the first mission
        public static void DrawScheme(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(Assets.Images["KeyboardLeft"], new Vector2(x, y), Color.White);
            spriteBatch.Draw(Assets.Images["KeyboardRight"], new Vector2(x + 60, y), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.movement"), new Vector2(x + 182, y + 15), MyColor.White);
            spriteBatch.Draw(Assets.Images["KeyboardSpace"], new Vector2(x, y + 60), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.shoot"), new Vector2(x + 182, y + 75), MyColor.White);
            spriteBatch.Draw(Assets.Images["KeyboardUp"], new Vector2(x, y + 120), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.jump"), new Vector2(x + 182, y + 135), MyColor.White);
            spriteBatch.Draw(Assets.Images["KeyboardDown"], new Vector2(x, y + 180), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], "/", new Vector2(x + 57, y + 195), MyColor.White);
            spriteBatch.Draw(Assets.Images["KeyboardCtrl"], new Vector2(x + 72, y + 180), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.action"), new Vector2(x + 182, y + 195), MyColor.White);

            spriteBatch.Draw(Assets.Images["GamepadLeftStick"], new Vector2(x + 320, y - 4), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.movement"), new Vector2(x + 390, y + 11), MyColor.White);
            spriteBatch.Draw(Assets.Images["GamepadX"], new Vector2(x + 324, y + 60), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.shoot"), new Vector2(x + 390, y + 75), MyColor.White);
            spriteBatch.Draw(Assets.Images["GamepadY"], new Vector2(x + 324, y + 120), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.jump"), new Vector2(x + 390, y + 135), MyColor.White);
            spriteBatch.Draw(Assets.Images["GamepadA"], new Vector2(x + 324, y + 180), Color.White);
            spriteBatch.DrawString(Assets.Fonts["Medium"], Translation.Get("controls.action"), new Vector2(x + 390, y + 195), MyColor.White);
        }
    }
}
