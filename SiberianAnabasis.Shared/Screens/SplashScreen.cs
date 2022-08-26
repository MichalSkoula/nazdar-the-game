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
            this.Game.SpriteBatch.Draw(Assets.Images["Keyboard"], new Vector2(Offset.MenuX, 100), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "action", new Vector2(Offset.MenuX + 43, Offset.MenuY + 95), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "shoot", new Vector2(Offset.MenuX + 78, Offset.MenuY + 128), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "jump", new Vector2(Offset.MenuX + 109, Offset.MenuY + 160), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "movement", new Vector2(Offset.MenuX + 315, Offset.MenuY + 128), Color.White);

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "Gamepad", new Vector2(Offset.MenuX, Offset.MenuY + 210), Color.White);
            this.Game.SpriteBatch.Draw(Assets.Images["Gamepad"], new Vector2(Offset.MenuX, Offset.MenuY + 230), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "movement", new Vector2(Offset.MenuX + 45, Offset.MenuY + 275), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "action", new Vector2(Offset.MenuX + 210, Offset.MenuY + 250), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "shoot", new Vector2(Offset.MenuX + 195, Offset.MenuY + 275), Color.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "jump", new Vector2(Offset.MenuX + 225, Offset.MenuY + 300), Color.White);

            this.Game.DrawEnd();
        }
    }
}
