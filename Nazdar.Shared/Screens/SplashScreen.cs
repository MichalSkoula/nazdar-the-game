using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
{
    public class SplashScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 35;

        private readonly string[] intro =
        {
            "The Czechoslovak Legion were volunteer armed forces of Czech and Slovaks, fighting ",
            "on the side of the Allied Powers during World War I, to win the support of the Allied",
            "Powers for the independence of Czechoslovakia from the Austro-Hungarian Empire.",
            "",
            "Czechoslovak Legion fought in the Russian Civil War against Bolshevik authorities,",
            "beginning in May 1918, clearing Bolshevik forces from the entire Trans-Siberian ",
            "Railway by September 1918 and persisting through evacuation to Europe in 1920.",
        };
        private float descriptionY = 350;
        private readonly int descriptionYStop = 190;
        private readonly int descriptionSpeed = 25;

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
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            timer -= Game.DeltaTime;
            if (timer < 0)
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // move description
            if (this.descriptionY > descriptionYStop)
            {
                this.descriptionY -= this.Game.DeltaTime * descriptionSpeed;
            }
            else
            {
                // to be precise
                this.descriptionY = descriptionYStop;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            string title = "NAZDAR";
            int textWidthTitle = (int)Assets.Fonts["Largest"].MeasureString(title).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Largest"], title + "!", new Vector2((Enums.Screen.Width - textWidthTitle) / 2, 50), MyColor.Green);

            string subtitle = "The Game";
            int textWidthSubtitle = (int)Assets.Fonts["Large"].MeasureString(subtitle).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], subtitle, new Vector2((Enums.Screen.Width - textWidthSubtitle) / 2, 90), MyColor.Purple);

            string action = Game1.CurrentPlatform switch
            {
                Platform.GL => "Press ENTER to start the game",
                Platform.UWP => "Press button A to start the game",
                Platform.Android => "Touch to start the game",
                _ => throw new System.NotImplementedException(),
            };

            int textWidth = (int)Assets.Fonts["Medium"].MeasureString(action).X;
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], action, new Vector2((Enums.Screen.Width - textWidth) / 2, 140), MyColor.White);

            // intro up
            int i = 0;
            foreach (string line in this.intro)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX, descriptionY + 18 * i), MyColor.Gray1);
            }

            this.Game.DrawEnd();
        }
    }
}
