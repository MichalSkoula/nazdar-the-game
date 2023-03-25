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
                    "May 1918. We were attacked by the Hungarians",
                    "loyal to the Central Powers! Defend the train!",
                    "",
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                }
            },
            {
                2, new string[] {
                    "May 1918. The damned Bolsheviks attacked",
                    "the Legion train at the station! Defend!",
                    "",
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Hospital",
                }
            },
            {
                3, new string[] {
                    "May 1918. The damned Bolsheviks blew up the rails!",
                    "",
                    "MISSION GOALS",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Defense Tower",
                    "Absence of an Armory will make you defend your",
                    "people on your own.",
                }
            },
            {
                4, new string[] {
                    "June 1918. A great battle is coming. You must ",
                    "capture the city of Lipjag, where a large number",
                    "of those Bolsheviks are gathered.",
                    "",
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "Cholera epidemic struck! Make sure to have",
                    "a lot of medics.",
                }
            },
            {
                5, new string[] {
                    "June 1918. You must capture the city of Ufa and ",
                    "take control over near villages.",
                    "",
                    "MISSION GOALS",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "New wagon - Market",
                    "A hard winter struck, you will have to find other",
                    "sources of money than farming.",
                }
            },
            {
                6, new string[] {
                    "July 1918. You arrived late. Only a week earlier,",
                    "the Bolsheviks had murdered the Russian Tsar and",
                    "his entire family.",
                    "",
                    "MISSION GOALS",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "Cannot build Arsenal - your future depends on your",
                    "heroic soldiers.",
                    "Cholera epidemic struck! Make sure to have",
                    "lot of medics.",
                }
            },
            {
                7, new string[] {
                    "August 1918. You must conquer the city of Kazan and",
                    "defend the Russian Golden Treasure. It will serve",
                    "the White Guards to finance the fight.",
                    "",
                    "MISSION GOALS",
                    "Defend the Golden Treasure",
                    "Repair damaged rails",
                    "Repair the locomotive and head east",
                    "",
                    "TIPS",
                    "The Golden Treasure cannot be lost!!",
                }
            },
            {
                8, new string[] {
                    "January 1919. Time to go home. You took control the",
                    "entire Trans-Siberian Railway, but the promised help",
                    "from the Allied Powers did not come.",
                    "",
                    "MISSION GOALS",
                    "Buy the ship and go home",
                    "",
                    "TIPS",
                    "Damned Bolsheviks will attact only from the left side.",
                    "With all they got. Even with Mechanized Lenins.",
                    "Cholera epidemic struck! Make sure to have",
                    "lot of medics.",
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

            if (saveData != null && saveData.ContainsKey("village"))
            {
                this.Game.Village = (int)saveData.village;
            }
            else
            {
                buttons.GetValueOrDefault("deleteButton").Active = false;
                this.Game.Village = 1;
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
