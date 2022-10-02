using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private string[] saveDataLines;

        public override void Initialize()
        {
            buttons.Add("startButton", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Start", true));
            buttons.Add("deleteButton", new Button(Offset.MenuX, 100, null, ButtonSize.Medium, "Delete save"));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Main Menu"));

            this.Load();

            base.Initialize();
        }

        private void Load()
        {
            FileIO saveFile = new FileIO(Game.SaveSlot);

            dynamic saveData = saveFile.Load();
            this.saveDataLines = Tools.ParseSaveData(saveData);

            if (saveData != null)
            {
                // focus on current village & active accessed villages
                if (saveData.ContainsKey("village"))
                {
                    this.Game.Village = (int)saveData.village;
                }
            }
            else
            {
                buttons.GetValueOrDefault("deleteButton").Active = false;
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

            // delete save
            if (this.buttons.GetValueOrDefault("deleteButton").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.MapScreenDeleteSave));
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

            // save data
            int i = 0;

            foreach (string line in this.saveDataLines)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 90 + 30 * i), Color.White);

            }

            this.Game.DrawEnd();
        }
    }
}
