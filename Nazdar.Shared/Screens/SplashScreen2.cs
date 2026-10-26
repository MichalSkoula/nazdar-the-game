using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using System;
using System.Collections.Generic;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
{
    public class SplashScreen2 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 16;

        private string[] intro;

        // the intro paragraph is wrapped to the panel width
        private string[] GetIntro()
        {
            string paragraph = Translation.Get("splash2.text");

            List<string> lines = new List<string>();
            string current = "";
            foreach (string word in paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                string candidate = current == "" ? word : current + " " + word;
                if (current != "" && Assets.Fonts["Small"].MeasureString(candidate).X > IntroTextWidth)
                {
                    lines.Add(current);
                    current = word;
                }
                else
                {
                    current = candidate;
                }
            }
            lines.Add(current);

            lines.Add("");
            lines.Add(Translation.Get("splash2.disclaimer"));

            return lines.ToArray();
        }

        // intro revealed line by line in a centered panel (same style as mission descriptions)
        private const int IntroPadding = 14;
        private const int IntroTextWidth = Enums.Screen.Width - 80 - (2 * IntroPadding);
        private const int IntroLineHeight = 18;
        private const float IntroLineDelay = 1f;
        private const float IntroLineFade = 0.4f;
        private float introTimer;

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

            this.introTimer += this.Game.DeltaTime;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            string[] intro = this.intro ??= GetIntro();
            float lineHeight = Assets.Fonts["Small"].MeasureString("A").Y;
            // panel almost as wide as the screen, height fitted to the text, centered
            int panelWidth = IntroTextWidth + (2 * IntroPadding);
            int panelHeight = (int)((IntroLineHeight * (intro.Length - 1)) + lineHeight) + (2 * IntroPadding);
            Rectangle panel = new Rectangle((Enums.Screen.Width - panelWidth) / 2, (Enums.Screen.Height - panelHeight) / 2, panelWidth, panelHeight);
            this.Game.SpriteBatch.FillRectangle(panel, MyColor.DarkViolet * 0.5f);
            this.Game.SpriteBatch.DrawRectangle(panel, MyColor.Gray4);

            // lines appear one by one, the last one (disclaimer) is highlighted
            for (int i = 0; i < intro.Length; i++)
            {
                float alpha = MathHelper.Clamp((this.introTimer - (i * IntroLineDelay)) / IntroLineFade, 0, 1);
                if (alpha <= 0)
                {
                    break;
                }

                int textWidthLine = (int)Assets.Fonts["Small"].MeasureString(intro[i]).X;
                this.Game.SpriteBatch.DrawString(
                    Assets.Fonts["Small"],
                    intro[i],
                    new Vector2((Enums.Screen.Width - textWidthLine) / 2, panel.Y + IntroPadding + (IntroLineHeight * i)),
                    (i < intro.Length - 1 ? MyColor.Gray1 : MyColor.White) * alpha
                );
            }

            this.Game.DrawEnd();
        }
    }
}
