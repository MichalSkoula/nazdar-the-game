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
        private readonly int descriptionYStop = 62;
        private readonly int descriptionSpeed = 40;
        private readonly Dictionary<int, string[]> villageDescriptions = new Dictionary<int, string[]>()
        {
            {
                1, new string[] {
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "May 1918, Chelyabinsk. An eastbound train bearing",
                    "Legion forces encountered a westbound train",
                    "bearing Hungarians, who were loyal to the Central",
                    "Powers and who regarded Legion troops as traitors.",
                    "An armed conflict ensued at close range.",
                }
            },
            {
                2, new string[] {
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Hospital",
                    "",
                    "May 1918, Omsk. The damned Bolsheviks attacked",
                    "the Legion train at the Maryanovka station.",
                }
            },
            {
                3, new string[] {
                    "MISSION GOALS",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Defense Tower",
                    "Absence of an Armory will make this mission very",
                    "difficult - you must defend the people only with",
                    "your gun.",
                    "",
                    "May 1918, Penza. The Legion must gain control over",
                    "the Penze train station, to be able to continue.",
                }
            },
            {
                4, new string[] {
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "Cholera epidemic struck! Make sure to have",
                    "lot of medics.",
                    "",
                    "June 1918, Lipjag. A great battle is coming.",
                    "The Legion must capture the city of Lipjag,",
                    "where a large number of those damned Bolsheviks",
                    "are gathered...",
                }
            },
            {
                5, new string[] {
                    "MISSION GOALS",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Market",
                    "A hard winter struck, you will have to find other",
                    "sources of money than farming.",
                    "",
                    "June 1918, Ufa. The Legion must capture the city",
                    "of Ufa and take control over near villages.",
                }
            },
            {
                6, new string[] {
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "Cannot build Arsenal - your future depends on your",
                    "heroic soldiers.",
                    "Cholera epidemic struck! Make sure to have",
                    "lot of medics.",
                    "",
                    "July 1918, Yekaterinburg. The Legion arrived late.",
                    "Only a week earlier, the Bolsheviks had killed",
                    "the Russian Tsar and his entire family.",
                }
            },
            {
                7, new string[] {
                    "MISSION GOALS",
                    "Defend the Golden Treasure",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "Cholera epidemic struck! Make sure to have",
                    "lot of medics.",
                    "",
                    "August 6 1918, Kazan. The Legion must conquer the city",
                    "of Kazan and defend the Russian Golden Treasure. This ",
                    "will then serve the White Guards to finance the fight",
                    "against the Bolsheviks.",
                }
            },
            {
                8, new string[] {
                    "MISSION GOALS",
                    "Buy the ship and go home",
                    "",
                    "TIPS",
                    "Damned Bolsheviks will attact only from the left side.",
                    "With all they got. Even with Mechanized Lenins.",
                    "",
                    "January 1919, Vladivostok. The Legionnaires want to go",
                    "home. They control the entire Trans-Siberian Railway, ",
                    "but the promised help from the Allied Powers does not ",
                    "come. They must seize Vladivostok and make their way",
                    "home by ships.",
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
            Button.UpdateButtons(this.buttons);

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

            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], ((Villages)this.Game.Village).ToString(), new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            // save data - show only some things
            int i = 0;
            foreach (string line in this.saveDataLines)
            {
                i++;
                if (i == 1)
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
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 225, Offset.MenuY + descriptionY + 18 * i), MyColor.Gray1);
            }

            this.Game.DrawEnd();
        }
    }
}
