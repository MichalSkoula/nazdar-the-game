using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;
using Mouse = Nazdar.Controls.Mouse;

namespace Nazdar.Screens
{
    public class CreditsScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private double timer = 30;

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private readonly string[] intro =
        {
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
            "The Czechoslovak Legion were volunteer armed forces",
        };
        private float descriptionY = 350;
        private readonly int descriptionSpeed = 20;

        public CreditsScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            Audio.SongTransition(0.25f, "Menu");

            if (Game1.CurrentPlatform != Platform.UWP)
            {
                buttons.Add("skoula", new Button(Offset.MenuX, Offset.MenuY + 92, null, ButtonSize.Small, "skoula.cz/nazdar"));
            }
            buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu", true));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.HasBeenPressed(Keys.Escape) || Gamepad.HasBeenPressed(Buttons.B))
            {
                // skip to menu screen
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            Button.UpdateButtons(this.buttons);

            // main menu
            if (this.buttons.GetValueOrDefault("menu").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // www skoula
            if (this.buttons.ContainsKey("skoula") && this.buttons.GetValueOrDefault("skoula").HasBeenClicked())
            {
                Tools.OpenLinkAsync("https://skoula.cz/nazdar");
            }

            timer -= Game.DeltaTime;
            if (timer < 0)
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // move description
            this.descriptionY -= this.Game.DeltaTime * descriptionSpeed;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Nazdar!", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "The Game", new Vector2(Offset.MenuX, Offset.MenuY + 30), MyColor.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "Credits", new Vector2(Offset.MenuX, Offset.MenuY + 53), MyColor.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "Links", new Vector2(Offset.MenuX, Offset.MenuY + 80), MyColor.White);

            // intro up
            int i = 0;
            foreach (string line in this.intro)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 210, descriptionY + 18 * i), MyColor.White);
            }

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}
