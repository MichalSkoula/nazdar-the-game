using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
{
    public class SplashScreen2 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 16;

        private string[] GetIntro()
        {
            return new string[]
            {
                Translation.Get("splash2.line1"),
                Translation.Get("splash2.line2"),
                Translation.Get("splash2.line3"),
                Translation.Get("splash2.line4"),
                Translation.Get("splash2.line5"),
                Translation.Get("splash2.line6"),
                Translation.Get("splash2.line7"),
                Translation.Get("splash2.line8"),
                Translation.Get("splash2.line9"),
                "",
                Translation.Get("splash2.disclaimer")
            };
        }

        private float descriptionY = 350;
        private readonly int descriptionYStop = 100;
        private readonly int descriptionSpeed = 25;

        public SplashScreen2(Game1 game) : base(game) { }

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

            // intro up
            int i = 0;
            string[] intro = GetIntro();
            foreach (string line in intro)
            {
                i++;

                int textWidthLine = (int)Assets.Fonts["Small"].MeasureString(line).X;

                this.Game.SpriteBatch.DrawString(
                    Assets.Fonts["Small"],
                    line,
                    new Vector2((Enums.Screen.Width - textWidthLine) / 2, descriptionY + (18 * i)),
                    i < intro.Length ? MyColor.Gray1 : MyColor.White
                );

            }

            this.Game.DrawEnd();
        }
    }
}
