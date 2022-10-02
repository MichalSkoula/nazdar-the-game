using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
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

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Siberian Anabasis", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Keyboard", new Vector2(Offset.MenuX, Offset.MenuY + 55), Color.White);

            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardLeft"], new Vector2(Offset.MenuX, 120), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardRight"], new Vector2(Offset.MenuX + 60, 120), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "movement", new Vector2(Offset.MenuX + 130, 135), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardSpace"], new Vector2(Offset.MenuX, 180), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "shoot", new Vector2(Offset.MenuX + 130, 195), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardUp"], new Vector2(Offset.MenuX, 240), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "jump", new Vector2(Offset.MenuX + 130, 255), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["KeyboardDown"], new Vector2(Offset.MenuX, 300), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "action", new Vector2(Offset.MenuX + 130, 315), Color.White);

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Gamepad", new Vector2(Offset.MenuX + 300, Offset.MenuY + 55), Color.White);

            this.Game.SpriteBatch.Draw(Assets.Images["GamepadLeftStick"], new Vector2(Offset.MenuX + 300, 116), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "movement", new Vector2(Offset.MenuX + 370, 131), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadX"], new Vector2(Offset.MenuX + 304, 180), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "shoot", new Vector2(Offset.MenuX + 370, 195), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadY"], new Vector2(Offset.MenuX + 304, 240), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "jump", new Vector2(Offset.MenuX + 370, 255), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["GamepadA"], new Vector2(Offset.MenuX + 304, 300), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "action", new Vector2(Offset.MenuX + 370, 315), Color.White);

            this.Game.DrawEnd();
        }
    }
}
