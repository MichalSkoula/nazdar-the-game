using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SiberianAnabasis.Controls;
using SiberianAnabasis.Shared;
using System.Collections.Generic;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private FileIO saveFile = new FileIO();

        public override void Initialize()
        {
            buttons.Add("startButton", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Start", true));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Main Menu"));

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            this.Load();

            base.Initialize();
        }

        private void Load()
        {
            dynamic saveData = this.saveFile.Load();
            if (saveData == null)
            {
                return;
            }

            // focus on current village & active accessed villages
            if (saveData.ContainsKey("village"))
            {
                this.Game.Village = (int)saveData.village;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Update();
            }

            // iterate through buttons up/down
            if (Controls.Keyboard.HasBeenPressed(Keys.Down) || Gamepad.HasBeenPressed(Buttons.DPadDown) || Gamepad.HasBeenPressedThumbstick(Direction.Down))
            {
                Tools.ButtonsIterateWithKeys(Direction.Down, this.buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up) || Gamepad.HasBeenPressed(Buttons.DPadUp) || Gamepad.HasBeenPressedThumbstick(Direction.Up))
            {
                Tools.ButtonsIterateWithKeys(Direction.Up, this.buttons);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter) || Gamepad.HasBeenPressed(Buttons.A))
            {
                foreach (KeyValuePair<string, Button> button in this.buttons)
                {
                    if (button.Value.Focus)
                    {
                        button.Value.Clicked = true;
                        break;
                    }
                }
            }

            // start game - villages
            if (this.buttons.GetValueOrDefault("startButton").HasBeenClicked() || Controls.Gamepad.HasBeenPressed(Buttons.Start))
            {
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }

            // back to main menu
            if (this.buttons.GetValueOrDefault("menuButton").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "Map", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
