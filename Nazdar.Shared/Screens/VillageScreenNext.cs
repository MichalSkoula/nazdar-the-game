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
        public void CheckIfWeCanGoToAnotherVillageOrWeWon()
        {
            // locomotive built - start animation ---------------------------------
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
                    // to another village
                    Game1.MessageBuffer.DeleteSuperMessage();
                    this.ToAnotherVillage();
                }
            }

            // ship bought? -------------------------------------------------------
            if (this.ship?.Status == Building.Status.Built)
            {
                Game1.MessageBuffer.SetSuperMessage("We did it! Nazdar!", 30);
                this.ship.Status = Building.Status.Finished;
            }

            Game1.WonAnimation = false;
            if (this.ship?.Status == Building.Status.Finished)
            {
                Game1.WonAnimation = true;

                // everyone aboard?
                if (this.allLegionnaires.Count == 0)
                {
                    if (!this.won)
                    {
                        this.won = true;
                        this.Won();
                    }
                }
            }
        }

        private void ToAnotherVillage()
        {
            Game1.MessageBuffer.AddMessage("Lets go to another village!", MessageType.Success);

            this.Game.Village++;

            // save score from previous levels
            this.player.BaseScore += Tools.GetScore(this.player.Days, this.player.Money, this.peasants.Count, this.soldiers.Count, this.player.Kills, Game1.CenterLevel);

            // enable sparking homelesses and coins at the start of next village
            this.Game.FirstRun = true;

            // reset some things
            Game1.CenterLevel = 1;
            Game1.TowersLevel = 1;
            this.locomotive = null;
            this.treasure = null;
            this.enemies.Clear();
            this.pigs.Clear();
            this.armories.Clear();
            this.arsenals.Clear();
            this.farms.Clear();
            this.hospitals.Clear();
            this.towers.Clear();
            this.markets.Clear();
            this.homelesses.Clear();
            this.coins.Clear();

            // player
            this.player.Kills = 0;
            this.player.Caliber = Player.DefaultCaliber;
            this.player.Bullets.Clear();
            this.player.Health += 25;

            // set new center based on new map size
            MapWidth = Assets.TilesetGroups["village" + this.Game.Village].GetTilesetMapWidth();
            this.player.X = MapWidth / 2;
            this.center.X = MapWidth / 2 - this.center.Width / 2; // center is always in the centre

            // take every other person with me + reset caliber + heal a bit
            this.peasants = this.peasants.Where((x, i) => i % 2 == 0).ToList();
            foreach (var p in this.peasants)
            {
                p.Caliber = Peasant.DefaultCaliber;
                p.Health += 25;
            }

            this.soldiers = this.soldiers.Where((x, i) => i % 2 == 0).ToList();
            foreach (var s in this.soldiers)
            {
                s.Caliber = Soldier.DefaultCaliber;
                s.Bullets.Clear();
                s.Health += 25;
            }

            this.farmers = this.farmers.Where((x, i) => i % 2 == 0).ToList();
            foreach (var f in this.farmers)
            {
                f.Caliber = Farmer.DefaultCaliber;
                f.Health += 25;
            }

            this.medics = this.medics.Where((x, i) => i % 2 == 0).ToList();
            foreach (var m in this.medics)
            {
                m.Caliber = Medic.DefaultCaliber;
                m.Health += 50;
            }

            // reset dayphase
            dayPhase = DayPhase.Day;
            dayPhaseTimer = (int)DayNightLength.Day;
            this.center.HasBeenUpgradedToday = false;

            // save
            this.saveFile.Save(this.GetSaveData());
            Game1.MessageBuffer.AddMessage("Game saved", MessageType.Info);

            // back to map menu
            this.Game.LoadScreen(typeof(Screens.MapScreen));
        }

        private void Won()
        {
            Game1.MessageBuffer.AddMessage("YOU WON. Beginner's luck.", MessageType.Success);

            // Outro
            this.Game.LoadScreen(typeof(Screens.GameFinishedScreen), true, true);
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