using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;

namespace Nazdar.Screens
{
    public class CreditsScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private const string wwwSkoula = "https://skoula.cz/nazdar";
        private const string wwwSkoulaShort = "skoula.cz/nazdar";

        private const string wwwCoffee = "https://www.buymeacoffee.com/mskoula";
        private const string wwwCoffeeShort = "buymeacoffee.com/mskoula";

        private readonly string[] intro =
        {
            "A GAME BY",
            "Michal Skoula",
            "",
            "ART",
            "0x72 (Robert Norenberg)",
            "Essssam",
            "Sirnosir",
            "eggboycolor (Andrew G. Crowell)",
            "Dream Mix (Justin Sink)",
            "Vnitti (Vicente Nitti)",
            "Ansimuz",
            "Abetusk",
            "Trix",
            "JohnMartPixel25",
            "Anokolisa",
            "Disven",
            "Michal Skoula",
            "",
            "MUSIC",
            "Tad Miller",
            "Omfgdude",
            "Zander Noriega",
            "Rizy",
            "The Real Monoton Artist",
            "Tri-Tachyon",
            "GTDStudio aka Palrom",
            "",
            "SOUNDS",
            "LordTomorrow",
            "Spookymodem",
            "Iwan Gabovitch",
            "Ogrebane",
            "rubberduck",
            "Thimras (Eike Germann)",
            "ChibiBagu",
            "Vinrax (Vladislav Krotov)",
            "j1987",
            "",
            "FONT",
            "Roberto Mocci",
            "",
            "TECH",
            "Monogame Framework",
            ".NET 6"
        };
        private float descriptionY = 350;
        private readonly int descriptionSpeed = 25;

        public CreditsScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            Audio.SongTransition(0.25f, "Menu");

            if (Game1.CurrentPlatform != Platform.UWP)
            {
                buttons.Add("skoula", new Button(Offset.MenuX, Offset.MenuY + 120, null, ButtonSize.Medium, "Official page"));
                buttons.Add("coffee", new Button(Offset.MenuX, Offset.MenuY + 120 + 27, null, ButtonSize.Medium, "Buy me a coffee"));
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

            // www 
            if (this.buttons.ContainsKey("skoula") && this.buttons.GetValueOrDefault("skoula").HasBeenClicked())
            {
                Tools.OpenLinkAsync(wwwSkoula);
            }
            if (this.buttons.ContainsKey("coffee") && this.buttons.GetValueOrDefault("coffee").HasBeenClicked())
            {
                Tools.OpenLinkAsync(wwwCoffee);
            }

            // move description
            this.descriptionY -= this.Game.DeltaTime * descriptionSpeed;
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "NAZDAR!", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.Green);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "The Game", new Vector2(Offset.MenuX, Offset.MenuY + 30), MyColor.Purple);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], "Credits", new Vector2(Offset.MenuX, Offset.MenuY + 53), MyColor.White);
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], "Links", new Vector2(Offset.MenuX, Offset.MenuY + 105), MyColor.White);

            if (Game1.CurrentPlatform == Platform.UWP)
            {
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], wwwSkoulaShort, new Vector2(Offset.MenuX, Offset.MenuY + 120), MyColor.White);
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], wwwCoffeeShort, new Vector2(Offset.MenuX, Offset.MenuY + 135), MyColor.White);
            }

            // intro up
            int i = 0;
            foreach (string line in this.intro)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 300, descriptionY + 18 * i), MyColor.White);
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
