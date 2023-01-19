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

        // every mission (village) should have a description
        private float descriptionY = 300;
        private readonly int descriptionYStop = 110;
        private readonly int descriptionSpeed = 40;
        private readonly Dictionary<int, string[]> villageDescriptions = new Dictionary<int, string[]>()
        {
            {
                1, new string[] {
                    "May 1918, Chelyabinsk. An eastbound train bearing",
                    "Legion forces encountered a westbound train",
                    "bearing Hungarians, who were loyal to the Central",
                    "Powers and who regarded Legion troops as traitors.",
                    "An armed conflict ensued at close range.",
                }
            },
            {
                2, new string[] {
                    "May 1918, Omsk. The damned Bolsheviks attacked",
                    "the Legion train at the Maryanovka station.",
                }
            },
            {
                3, new string[] {
                    "May 1918, Penza. The Legion must gain control over",
                    "the Penze train station, to be able to continue."
                }
            },
            {
                4, new string[] {
                    "June 1918, Lipjag. A great battle is coming.",
                    "The Legion must capture the city of Lipjag,",
                    "where a large number of those damned Bolsheviks",
                    "are gathered..."
                }
            },
            {
                5, new string[] {
                    "June 1918, Ufa. The Legion must capture the city",
                    "of Ufa and take control over near villages. Maybe do",
                    "some bizz there too."
                }
            },
            {
                6, new string[] {
                    "July 1918, Yekaterinburg. The Legion arrived late.",
                    "Only a week earlier, the Bolsheviks had killed",
                    "the Russian Tsar and his entire family."
                }
            },
            {
                7, new string[] {
                    "August 6 1918, Kazan. The Legion must conquer the city",
                    "of Kazan and seize the Russian gold treasure. This will",
                    "then serve the White Guards to finance the fight against",
                    "the Bolsheviks."
                }
            },
            {
                8, new string[] {
                    "January 1919, Vladivostok. The Legionnaires want to go",
                    "home. They control the entire Trans-Siberian Railway, ",
                    "but the promised help from the Allied Powers does not ",
                    "come. They must seize Vladivostok and make their way",
                    "home by ships."
                }
            },

        };

        public override void Initialize()
        {
            buttons.Add("startButton", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Start", true));
            buttons.Add("deleteButton", new Button(Offset.MenuX, 100, null, ButtonSize.Medium, "Delete save"));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu"));

            this.Load();

            Audio.SongVolume = 0.25f;

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
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 250, Offset.MenuY + descriptionY + 18 * i), MyColor.White);
            }

            this.Game.DrawEnd();
        }
    }
}
