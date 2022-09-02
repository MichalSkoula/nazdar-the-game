using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SiberianAnabasis.Controls;
using static SiberianAnabasis.Enums;
using Keyboard = SiberianAnabasis.Controls.Keyboard;
using Mouse = SiberianAnabasis.Controls.Mouse;

namespace SiberianAnabasis.Screens
{
    public class SplashScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 5;

        public SplashScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.HasBeenPressed(Keys.Escape) || Keyboard.HasBeenPressed(Keys.Enter) || Gamepad.HasBeenPressed(Buttons.A) || Mouse.HasBeenPressed())
            {
                // skip splash screen
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            timer -= Game.DeltaTime;
            if (timer < 0)
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "SiberianAnabasis", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "Keyboard", new Vector2(Offset.MenuX, Offset.MenuY + 55), Color.White);

            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardLeft"], new Vector2(Offset.MenuX, 120), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardRight"], new Vector2(Offset.MenuX + 30, 120), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "movement", new Vector2(Offset.MenuX + 70, 125), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardUp"], new Vector2(Offset.MenuX, 150), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "jump", new Vector2(Offset.MenuX + 70, 155), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardDown"], new Vector2(Offset.MenuX, 180), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "action", new Vector2(Offset.MenuX + 70, 185), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardSpace"], new Vector2(Offset.MenuX, 210), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "shoot", new Vector2(Offset.MenuX + 70, 215), Color.White);

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "Gamepad", new Vector2(Offset.MenuX + 300, Offset.MenuY + 55), Color.White);

            this.Game.SpriteBatch.Draw(Assets.Images["GamepadLeftStick"], new Vector2(Offset.MenuX + 300, 120), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "movement", new Vector2(Offset.MenuX + 340, 127), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadA"], new Vector2(Offset.MenuX + 302, 154), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "jump", new Vector2(Offset.MenuX + 340, 161), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadY"], new Vector2(Offset.MenuX + 302, 184), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "action", new Vector2(Offset.MenuX + 340, 191), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadX"], new Vector2(Offset.MenuX + 302, 214), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "shoot", new Vector2(Offset.MenuX + 340, 221), Color.White);

            this.Game.DrawEnd();
        }
    }
}
