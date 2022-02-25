using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyGame.Controls;
using MyGame.Shared;
using System.Collections.Generic;
using static MyGame.Enums;

namespace MyGame.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private FileIO saveFile = new FileIO();

        public override void Initialize()
        {
            buttons.Add("village1", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Village 1", true));
            buttons.Add("village2", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "Village 2", active: false));
            buttons.Add("village3", new Button(Offset.MenuX, 140, null, ButtonSize.Large, "Village 3", active: false));
            buttons.Add("village4", new Button(Offset.MenuX, 180, null, ButtonSize.Large, "Village 4", active: false));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Small, "Back to Main Menu"));

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
            if (saveData.ContainsKey("village") && saveData.ContainsKey("villageAccess"))
            {
                int i = 0;
                foreach (KeyValuePair<string, Button> button in this.buttons)
                {
                    i++;

                    if (button.Key == "village" + saveData.village)
                    {
                        button.Value.Focus = true;
                    }
                    else
                    {
                        button.Value.Focus = false;
                    }

                    // access villages
                    if ((int)saveData.villageAccess >= i)
                    {
                        button.Value.Active = true;
                    }
                } 
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
            if (Controls.Keyboard.HasBeenPressed(Keys.Down))
            {
                Tools.ButtonsIterateWithKeys(Direction.Down, this.buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up))
            {
                Tools.ButtonsIterateWithKeys(Direction.Up, this.buttons);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
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
            if (this.buttons.GetValueOrDefault("village1").HasBeenClicked())
            {
                this.Game.Village = 1;
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }
            if (this.buttons.GetValueOrDefault("village2").HasBeenClicked())
            {
                this.Game.Village = 2;
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }
            if (this.buttons.GetValueOrDefault("village3").HasBeenClicked())
            {
                this.Game.Village = 3;
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }
            if (this.buttons.GetValueOrDefault("village4").HasBeenClicked())
            {
                this.Game.Village = 4;
                this.Game.LoadScreen(typeof(Screens.VillageScreen));
            }

            // back to main menu
            if (this.buttons.GetValueOrDefault("menuButton").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "Map", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}
