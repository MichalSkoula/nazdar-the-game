using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
using Newtonsoft.Json.Linq;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        public void CheckIfWeCanGoToAnotherVillage()
        {
            if (this.locomotive?.Status == Building.Status.Built)
            {
                Game1.MessageBuffer.SetSuperMessage("Off we go!", 30);
                this.locomotive.Status = Building.Status.Finished;
            }

            Game1.NextLevelAnimation = false;
            if (this.locomotive?.Status == Building.Status.Finished)
            {
                Game1.NextLevelAnimation = true;

                // go to the locomotive
                if (this.player.X < this.locomotive.X)
                {
                    this.player.Direction = Direction.Right;
                }
                else if (this.player.X > this.locomotive.X + this.locomotive.Width - this.player.Width)
                {
                    this.player.Direction = Direction.Left;
                }
                else
                {
                    Game1.MessageBuffer.DeleteSuperMessage();

                    // near locomotive? end this
                    if (this.Game.Village == MaxVillage)
                    {
                        if (!this.won)
                        {
                            this.won = true;
                            this.Won();
                        }
                    }
                    else
                    {
                        this.ToAnotherVillage();
                    }
                }
            }
        }

        private void ToAnotherVillage()
        {
            Game1.MessageBuffer.AddMessage("Locomotive repaired!", MessageType.Success);
            Game1.MessageBuffer.AddMessage("Lets go to another village!", MessageType.Success);

            this.Game.Village++;

            // save score from previous levels
            this.player.BaseScore += Tools.GetScore(this.player.Days, this.player.Money, this.peasants.Count, this.soldiers.Count, this.player.Kills, this.center != null ? this.center.Level : 0);

            // enable sparking homelesses and coins at the start of next village
            this.Game.FirstRun = true;

            // reset some things
            this.center.Level = 1;
            this.locomotive = null;
            this.enemies.Clear();
            this.armories.Clear();
            this.arsenals.Clear();
            this.farms.Clear();
            this.hospitals.Clear();
            this.towers.Clear();
            this.homelesses.Clear();
            this.coins.Clear();
            this.player.Kills = 0;
            this.player.Caliber = Player.DefaultCaliber;
            this.player.Bullets.Clear();

            // set new center based on new map size
            MapWidth = Assets.TilesetGroups["village" + this.Game.Village].GetTilesetMapWidth();
            this.player.X = MapWidth / 2;
            this.center.X = MapWidth / 2 - this.center.Width / 2; // center is always in the centre

            // take every other person with me + reset caliber
            this.peasants = this.peasants.Where((x, i) => i % 2 == 0).ToList();
            foreach (var p in this.peasants)
            {
                p.Caliber = Peasant.DefaultCaliber;
            }

            this.soldiers = this.soldiers.Where((x, i) => i % 2 == 0).ToList();
            foreach (var s in this.soldiers)
            {
                s.Caliber = Soldier.DefaultCaliber;
                s.Bullets.Clear();
            }

            this.farmers = this.farmers.Where((x, i) => i % 2 == 0).ToList();
            foreach (var f in this.farmers)
            {
                f.Caliber = Farmer.DefaultCaliber;
            }

            this.medics = this.medics.Where((x, i) => i % 2 == 0).ToList();
            foreach (var m in this.medics)
            {
                m.Caliber = Medic.DefaultCaliber;
            }

            // reset dayphase
            dayPhase = DayPhase.Day;
            dayPhaseTimer = (int)DayNightLength.Day;

            // save
            this.saveFile.Save(this.GetSaveData());
            Game1.MessageBuffer.AddMessage("Game saved", MessageType.Info);

            // back to map menu
            this.Game.LoadScreen(typeof(Screens.MapScreen));
        }

        private void Won()
        {
            Game1.MessageBuffer.AddMessage("YOU WON. Beginner's luck.", MessageType.Success);

            // Back to Menu
            this.Game.LoadScreen(typeof(Screens.GameFinishedScreen));
        }

        private void GameOver()
        {
            // save current state to global static variable as json
            // to be able to show it on game over screen
            // the same way as if it was from a file
            Game1.SaveTempData = JObject.FromObject(this.GetSaveData());

            // load screen
            this.Game.LoadScreen(typeof(Screens.GameOverScreen));
        }
    }
}