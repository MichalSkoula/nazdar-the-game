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
            "The Czechoslovak Legion was a military force fighting for the Allies during WWI.",
            "The main goal was to win the support of the Allies for the independence of",
            "Czechoslovakia from the Austria-Hungary. The Legion's efforts during the ",
            "Russian Civil War included clearing the entire Trans-Siberian Railway of ",
            "Bolshevik forces. They evacuated to Europe by 1920.",
            "",
            "This game is intended for entertainment purposes and is not historically accurate."
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

                int textWidthLine = (int)Assets.Fonts["Small"].MeasureString(line).X;

                this.Game.SpriteBatch.DrawString(
                    Assets.Fonts["Small"],
                    line,
                    new Vector2((Enums.Screen.Width - textWidthLine) / 2, descriptionY + 18 * i),
                    i < this.intro.Length ? MyColor.Gray1 : MyColor.White
                );

            }

            this.Game.DrawEnd();
        }
    }
}
