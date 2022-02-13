using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Controls;
using MyGame.Shared;
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
            buttons.Add("village2", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "Village 2"));

            buttons.Add("menuButton", new Button(Offset.MenuX, 320, null, ButtonSize.Small, "Menu"));

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

            // TODO load detail - show where on map we are
            if (saveData.ContainsKey("detail"))
            {
                // this.player.Load(saveData.GetValue("player"));
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

            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "select map", new Vector2(Offset.MenuX, Offset.MenuY), Color.Red);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}
