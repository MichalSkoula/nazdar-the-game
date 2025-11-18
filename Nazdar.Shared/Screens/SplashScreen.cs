using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
{
    public class SplashScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 4;

        public SplashScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            // play menu song, but silently
            Audio.CurrentSongCollection = "Menu";
            Audio.SongVolume = 0.1f;
            Audio.PlaySongCollection();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.HasBeenPressed(Keys.Escape) || Keyboard.HasBeenPressed(Keys.Enter) || Gamepad.HasBeenPressed(Buttons.A) || Mouse.HasBeenPressed() || Touch.HasBeenPressedAnywhere())
            {
                // skip splash screen
                this.Game.LoadScreen(typeof(Screens.SplashScreen2));
            }

            timer -= Game.DeltaTime;
            if (timer < 0)
            {
                this.Game.LoadScreen(typeof(Screens.SplashScreen2));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            string title = "NAZDAR";
            int textWidthTitle = (int)Assets.Fonts["Largest"].MeasureString(title).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Largest"], title + "!", new Vector2((Enums.Screen.Width - textWidthTitle) / 2, 110), MyColor.Green);

            string subtitle = "The Game";
            int textWidthSubtitle = (int)Assets.Fonts["Large"].MeasureString(subtitle).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], subtitle, new Vector2((Enums.Screen.Width - textWidthSubtitle) / 2, 150), MyColor.Purple);

            string action = Game1.CurrentPlatform switch
            {
                Platform.GL => Translation.Get("splash.pressEnter"),
                Platform.DX => Translation.Get("splash.pressEnter"),
                Platform.UWP => Translation.Get("splash.pressButtonA"),
                Platform.Android => Translation.Get("splash.touchToContinue"),
                _ => throw new System.NotImplementedException(),
            };

            int textWidth = (int)Assets.Fonts["Medium"].MeasureString(action).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], action, new Vector2((Enums.Screen.Width - textWidth) / 2, 200), MyColor.White);

            this.Game.DrawEnd();
        }
    }
}
