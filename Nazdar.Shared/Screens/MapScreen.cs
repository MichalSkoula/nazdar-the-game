using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private readonly Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private string[] saveDataLines;

        // every mission (village) should have a description
        private float descriptionY = 300;
        private readonly int descriptionYStop = 62;
        private readonly int descriptionSpeed = 40;
        private Dictionary<int, string[]> villageDescriptions;

        public override void Initialize()
        {
            // Initialize mission descriptions with translations
            villageDescriptions = new Dictionary<int, string[]>()
            {
                {
                    1, new string[] {
                        Translation.Get("mission1.line1"),
                        Translation.Get("mission1.line2"),
                        Translation.Get("mission1.line3"),
                        "",
                        Translation.Get("mission1.goals"),
                        Translation.Get("mission1.goal1"),
                    }
                },
                {
                    2, new string[] {
                        Translation.Get("mission2.line1"),
                        Translation.Get("mission2.line2"),
                        "",
                        Translation.Get("mission2.goals"),
                        Translation.Get("mission2.goal1"),
                        "",
                        Translation.Get("mission2.tips"),
                        Translation.Get("mission2.tip1"),
                    }
                },
                {
                    3, new string[] {
                        Translation.Get("mission3.line1"),
                        Translation.Get("mission3.line2"),
                        "",
                        Translation.Get("mission3.goals"),
                        Translation.Get("mission3.goal1"),
                        Translation.Get("mission3.goal2"),
                        "",
                        Translation.Get("mission3.tips"),
                        Translation.Get("mission3.tip1"),
                        Translation.Get("mission3.tip2"),
                        Translation.Get("mission3.tip3"),
                    }
                },
                {
                    4, new string[] {
                        Translation.Get("mission4.line1"),
                        Translation.Get("mission4.line2"),
                        Translation.Get("mission4.line3"),
                        Translation.Get("mission4.line4"),
                        "",
                        Translation.Get("mission4.goals"),
                        Translation.Get("mission4.goal1"),
                        "",
                        Translation.Get("mission4.tips"),
                        Translation.Get("mission4.tip1"),
                        Translation.Get("mission4.tip2"),
                    }
                },
                {
                    5, new string[] {
                        Translation.Get("mission5.line1"),
                        Translation.Get("mission5.line2"),
                        "",
                        Translation.Get("mission5.goals"),
                        Translation.Get("mission5.goal1"),
                        Translation.Get("mission5.goal2"),
                        "",
                        Translation.Get("mission5.tips"),
                        Translation.Get("mission5.tip1"),
                        Translation.Get("mission5.tip2"),
                        Translation.Get("mission5.tip3"),
                    }
                },
                {
                    6, new string[] {
                        Translation.Get("mission6.line1"),
                        Translation.Get("mission6.line2"),
                        Translation.Get("mission6.line3"),
                        "",
                        Translation.Get("mission6.goals"),
                        Translation.Get("mission6.goal1"),
                        "",
                        Translation.Get("mission6.tips"),
                        Translation.Get("mission6.tip1"),
                        Translation.Get("mission6.tip2"),
                        Translation.Get("mission6.tip3"),
                        Translation.Get("mission6.tip4"),
                    }
                },
                {
                    7, new string[] {
                        Translation.Get("mission7.line1"),
                        Translation.Get("mission7.line2"),
                        Translation.Get("mission7.line3"),
                        Translation.Get("mission7.line4"),
                        "",
                        Translation.Get("mission7.goals"),
                        Translation.Get("mission7.goal1"),
                        Translation.Get("mission7.goal2"),
                        Translation.Get("mission7.goal3"),
                        "",
                        Translation.Get("mission7.tips"),
                        Translation.Get("mission7.tip1"),
                    }
                },
                {
                    8, new string[] {
                        Translation.Get("mission8.line1"),
                        Translation.Get("mission8.line2"),
                        Translation.Get("mission8.line3"),
                        Translation.Get("mission8.line4"),
                        "",
                        Translation.Get("mission8.goals"),
                        Translation.Get("mission8.goal1"),
                        "",
                        Translation.Get("mission8.tips"),
                        Translation.Get("mission8.tip1"),
                        Translation.Get("mission8.tip2"),
                        Translation.Get("mission8.tip3"),
                        Translation.Get("mission8.tip4"),
                        Translation.Get("mission8.tip5"),
                    }
                },
            };

            buttons.Add("startButton", new Button(Offset.MenuX, 60, null, ButtonSize.Large, Translation.Get("map.start"), true));
            buttons.Add("deleteButton", new Button(Offset.MenuX, 100, null, ButtonSize.Medium, Translation.Get("map.deleteSave")));
            buttons.Add("menuButton", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, Translation.Get("menu.backToMenu")));

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
                // last village? play different sound (last village = villages count - 1 because of survival)
                if (this.Game.Village == Game1.MaxVillage)
                {
                    Audio.PlaySound("FinalRound");
                }
                // not last village - play only ready sound
                else
                {
                    Audio.PlaySound("Ready");
                }
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
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 72 + (28 * i)), MyColor.White);
            }

            // descriptions
            i = 0;
            foreach (string line in this.villageDescriptions[this.Game.Village])
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 225, Offset.MenuY + descriptionY + (18 * i)), MyColor.Gray1);
            }

            this.Game.DrawEnd();
        }
    }
}
