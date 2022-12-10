using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        private void Build(BaseBuilding building)
        {
            // is someone building it?
            building.IsBeingBuilt = false;
            foreach (var peasant in this.peasants.Where(p => p.Hitbox.Intersects(building.Hitbox)))
            {
                building.IsBeingBuilt = true;
                peasant.IsBuildingHere = building.Hitbox;
                break;
            }

            // no one is building it
            if (building.IsBeingBuilt == false && this.peasants.Count > 0)
            {
                var nearestPeasant = this.peasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                nearestPeasant.IsBuildingHere = building.Hitbox;
            }
        }

        private void Pick(BaseBuilding building)
        {
            if (this.peasants.Count > 0)
            {
                var nearestPeasant = this.peasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                nearestPeasant.IsRunningForItem = building.Hitbox;
            }
        }

        private void CreateHomeless()
        {
            this.homelesses.Add(new Homeless(
                this.slumXs[Tools.GetRandom(this.slumXs.Count)],
                Offset.Floor,
                Direction.Right
            ));
        }

        private void EnemyDie(Enemy enemy)
        {
            // maybe drop something?
            if (Tools.GetRandom(this.enemyDropProbability) == 1)
            {
                Game1.MessageBuffer.AddMessage("New coin! Enemy drop.", MessageType.Opportunity);
                this.coins.Add(new Coin(enemy.X, Offset.Floor2));
            }

            Audio.PlayRandomSound("EnemyDeaths");
            this.player.Kills++;
            enemy.Dead = true;
        }

        private int GetUpgradeAttackAddition()
        {
            return this.center == null ? 0 : this.center.Level * 2;
        }

        private void Upgrade()
        {
            // upgrade soldiers etc
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Caliber = Soldier.DefaultCaliber + this.GetUpgradeAttackAddition();
            }
            foreach (Peasant peasant in this.peasants)
            {
                peasant.Caliber = Peasant.DefaultCaliber + this.GetUpgradeAttackAddition();
            }
            foreach (Farmer farmer in this.farmers)
            {
                farmer.Caliber = Farmer.DefaultCaliber + this.GetUpgradeAttackAddition();
            }
            foreach (Medic medic in this.medics)
            {
                medic.Caliber = Medic.DefaultCaliber + this.GetUpgradeAttackAddition();
            }

            // upgrade and heal player
            this.player.Caliber = Player.DefaultCaliber + this.GetUpgradeAttackAddition();
            this.player.Health = 100;

            // go to the next village?
            if (this.center.Level == Center.MaxCenterLevel)
            {
                // finish the game? or another village?
                if (this.Game.Village == MaxVillage)
                {
                    this.Won();
                }
                else
                {
                    this.ToAnotherVillage();
                }
            }
        }

        private void ToAnotherVillage()
        {
            Game1.MessageBuffer.AddMessage("MAX UPGRADE!", MessageType.Success);
            Game1.MessageBuffer.AddMessage("Lets go to another village!", MessageType.Success);

            this.Game.Village++;

            // save score from previous levels
            this.player.BaseScore += Tools.GetScore(this.player.Days, this.player.Money, this.peasants.Count, this.soldiers.Count, this.player.Kills, this.center != null ? this.center.Level : 0);

            // reset some things
            this.center.Level = 1;
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

            // take every other with me + reset caliber
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

            // save
            this.saveFile.Save(this.GetSaveData());

            // back to map menu
            this.Game.LoadScreen(typeof(Screens.MapScreen));
        }

        private void Won()
        {
            Game1.MessageBuffer.AddMessage("YOU DIT IT!!!!", MessageType.Success);

            // save
            this.saveFile.Save(this.GetSaveData());

            // back to main menu
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
