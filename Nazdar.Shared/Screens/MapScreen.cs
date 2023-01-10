using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private string[] saveDataLines;

        // every mission (village) should have a description
        private float descriptionY = 300;
        private readonly int descriptionYStop = 50;
        private readonly int descriptionSpeed = 40;
        private readonly Dictionary<int, string[]> villageDescriptions = new Dictionary<int, string[]>()
        {
            { 
                1, new string[] {
                    "May 14 1918, Chelyabinsk. An eastbound train ",
                    "bearing Legion forces encountered a westbound",
                    "train bearing Hungarians, who were loyal to the",
                    "Central Powers and who regarded Legion troops ",
                    "as traitors. An armed conflict ensued at close",
                    "range. The Legion defeated the Hungarians. In ",
                    "response, local Bolsheviks intervened, arrested",
                    "some Legion troops, who then fight back, ",
                    "storming the railway station, freeing their men,",
                    "and effectively taking over the city of ",
                    "Chelyabinsk while cutting the Bolshevik rail link",
                    "to Siberia."
                }
            },
            {
                2, new string[] {
                    "May 25 1918, Omsk. The Bolsheviks attacked the ",
                    "legion train at the Maryanovka station. At night,",
                    "the legionnaires made a successful counterattack",
                    "and captured the armaments."
                }
            },
            {
                3, new string[] {
                    "May 30 1918, Penza."
                }
            },
            {
                4, new string[] {
                    "June 8 1918, Samara."
                }
            },
            {
                5, new string[] {
                    "June 23 1918, Ufa."
                }
            },
            {
                6, new string[] {
                    "July 25 1918, Yekaterinburg. (týden předem zabili cara)"
                }
            },
            {
                7, new string[] {
                    "August 6 1918, Kazan (zlaty poklad)."
                }
            },
            {
                8, new string[] {
                    "? 1918, Vladivostok."
                }
            },

        };

        public override void Initialize()
        {
            buttons.Add("startButton", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Start", true));
            buttons.Add("deleteButton", new Button(Offset.MenuX, 100, null, ButtonSize.Medium, "Delete save"));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu"));

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

            // Back to Menu
            if (this.buttons.GetValueOrDefault("menuButton").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
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

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], this.Game.Village + ". " + (Villages)this.Game.Village, new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            // save data - show only village and score
            int i = 0;
            foreach (string line in this.saveDataLines)
            {
                i++;
                if (i == 1 || i > 3)
                {
                    continue;
                }
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 72 + 28 * i), MyColor.White);
            }

            // descriptions
            i = 0;
            foreach (string line in this.villageDescriptions[this.Game.Village])
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 260, Offset.MenuY + descriptionY + 18 * i), MyColor.White);
            }

            this.Game.DrawEnd();
        }
    }
}
