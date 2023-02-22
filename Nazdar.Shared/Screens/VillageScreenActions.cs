using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
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
            building.WorkingPeasant = null;

            // if the air is clean, he could do something
            if (this.dayPhase == DayPhase.Day && this.allEnemies.Where(enemy => enemy.Dead == false).Count() == 0)
            {
                foreach (var peasant in this.peasants.Where(p => p.Hitbox.Intersects(building.Hitbox)))
                {
                    building.WorkingPeasant = peasant;
                    peasant.IsBuildingHere = building.Hitbox;
                    break;
                }

                // no one is building it
                if (building.WorkingPeasant == null && this.peasants.Count > 0)
                {
                    var nearestPeasant = this.peasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                    nearestPeasant.IsBuildingHere = building.Hitbox;
                }
            }
        }

        private void Pick(BaseBuilding building, bool always, int things)
        {
            if (this.peasants.Count > 0 && (always || (this.dayPhase == DayPhase.Day && this.allEnemies.Where(enemy => enemy.Dead == false).Count() == 0)))
            {
                var nearestPeasants = this.peasants.OrderBy(p => Math.Abs(p.X - building.X));
                int i = 0;
                foreach (var peasant in nearestPeasants)
                {
                    if (i > things)
                    {
                        break;
                    }
                    i++;
                    peasant.IsRunningForItem = building.Hitbox;
                }
            }
        }

        private void CreateHomeless()
        {
            var randomSlum = this.slums[Tools.GetRandom(this.slums.Count)];
            this.homelesses.Add(new Homeless(
                randomSlum.X + randomSlum.Width / 2,
                Offset.Floor,
                Direction.Right
            ));
        }

        private void EnemyDie(Enemy enemy)
        {
            // maybe drop something?
            if (Tools.GetRandom(this.enemyDropProbability) == 1)
            {
                this.coins.Add(new Coin(enemy.X, Offset.Floor2));
            }

            Audio.PlayRandomSound("EnemyDeaths");
            this.player.Kills++;
            enemy.Dead = true;
        }

        private void PigDie(Pig pig)
        {
            // maybe drop something?
            if (Tools.GetRandom(this.pigDropProbability) == 1)
            {
                this.coins.Add(new Coin(pig.X, Offset.Floor2));
            }

            Audio.PlayRandomSound("PigDeaths");
            this.player.Kills++;
            pig.Dead = true;
        }

        private int GetUpgradeAttackAddition()
        {
            return Game1.CenterLevel * 2;
        }

        private int GetUpgradeAttackAdditionTowers()
        {
            return Game1.TowersLevel * 2;
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

            // upgrade player
            this.player.Caliber = Player.DefaultCaliber + this.GetUpgradeAttackAddition();

            // only one upgrade a day
            this.center.HasBeenUpgradedToday = true;

            // also towers
            foreach (Tower tower in this.towers)
            {
                tower.Caliber = Tower.DefaultCaliber + this.GetUpgradeAttackAdditionTowers();
            }
        }
    }
}
