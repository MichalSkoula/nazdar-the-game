using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class GameOverScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public GameOverScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private string[] saveDataLines;

        public override void Initialize()
        {
            buttons.Add("load", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Load last save", true));
            buttons.Add("new", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "New game"));
            buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu"));

            this.Load();

            Audio.SongVolume = 0.25f;

            base.Initialize();
        }

        private void Load()
        {
            // try to get current stats to show
            this.saveDataLines = Tools.ParseSaveData(Game1.SaveTempData);
        }

        public override void Update(GameTime gameTime)
        {
            Button.UpdateButtons(this.buttons);

            // load game
            if (this.buttons.GetValueOrDefault("load").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }

            // main menu
            if (this.buttons.GetValueOrDefault("menu").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // new game - to confirm screen
            if (this.buttons.GetValueOrDefault("new").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.GameOverScreenNewGame));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "GAME OVER", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            // save data
            int i = 0;
            foreach (string line in this.saveDataLines)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 100 + 28 * i), MyColor.White);

            }

            this.Game.DrawEnd();
        }
    }
}
