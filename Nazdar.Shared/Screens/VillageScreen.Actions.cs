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
        // Allows daytime work on a cleared side of the base (or map midpoint); enemies at the base block both sides.
        // Work at the base requires both sides clear; an optional worker area also checks their starting side.
        private bool CanWorkAt(Microsoft.Xna.Framework.Rectangle workArea, Microsoft.Xna.Framework.Rectangle? workerArea = null)
        {
            if (this.dayPhase != DayPhase.Day)
            {
                return false;
            }

            int centerLeft = this.center?.X ?? MapWidth / 2;
            int centerRight = this.center?.Hitbox.Right ?? centerLeft + 1;
            bool needsLeftSide = workArea.Center.X < centerLeft;
            bool needsRightSide = workArea.Center.X >= centerRight;

            // Work at the base itself requires both approaches to be clear.
            if (!needsLeftSide && !needsRightSide)
            {
                needsLeftSide = needsRightSide = true;
            }

            // A worker may leave the base, but must not cross an uncleared side to reach the job.
            if (workerArea.HasValue)
            {
                needsLeftSide |= workerArea.Value.Center.X < centerLeft;
                needsRightSide |= workerArea.Value.Center.X >= centerRight;
            }

            return !this.allEnemies.Any(enemy => !enemy.Dead && (
                // Enemies inside the base threaten work on either side.
                (enemy.Hitbox.Right > centerLeft && enemy.X < centerRight) ||
                (needsLeftSide && enemy.X < centerLeft) ||
                (needsRightSide && enemy.Hitbox.Right > centerRight)));
        }

        private void Build(BaseBuilding building)
        {
            // is someone building it?
            building.WorkingPeasant = null;

            // The opposite side may still be fighting while this construction site is safe.
            if (this.CanWorkAt(building.Hitbox))
            {
                var availablePeasants = this.peasants.Where(p => !p.Dead && this.CanWorkAt(building.Hitbox, p.Hitbox));
                foreach (var peasant in availablePeasants.Where(p => p.Hitbox.Intersects(building.Hitbox)))
                {
                    building.WorkingPeasant = peasant;
                    peasant.IsBuildingHere = building.Hitbox;
                    break;
                }

                // no one is building it
                if (building.WorkingPeasant == null)
                {
                    var nearestPeasant = availablePeasants.OrderBy(p => Math.Abs(p.X - building.X)).FirstOrDefault();
                    if (nearestPeasant != null)
                    {
                        nearestPeasant.IsBuildingHere = building.Hitbox;
                    }
                }
            }
        }

        private void Pick(BaseBuilding building, bool always, int things, Microsoft.Xna.Framework.Rectangle? itemHitbox = null)
        {
            Microsoft.Xna.Framework.Rectangle targetHitbox = itemHitbox ?? building.Hitbox;
            if (this.peasants.Count > 0 && (always || this.CanWorkAt(targetHitbox)))
            {
                var nearestPeasants = this.peasants
                    .Where(p => !p.Dead && (always || this.CanWorkAt(targetHitbox, p.Hitbox)))
                    .OrderBy(p => Math.Abs(p.X - building.X));
                int i = 0;
                foreach (var peasant in nearestPeasants)
                {
                    if (i > things)
                    {
                        break;
                    }
                    i++;
                    peasant.IsRunningForItem = targetHitbox;
                }
            }
        }

        private bool CreateHomeless()
        {
            if (this.slums.Count == 0)
            {
                return false;
            }

            var waitingHomelesses = this.homelesses.Where(h => !h.Dead && !h.ToDelete).ToList();
            // Count home-cave residents, not nearby positions: caves can have overlapping walking areas.
            var availableSlums = this.slums.Where(slum =>
                waitingHomelesses.Count(h => h.DeploymentX == slum.X) < HomelessPerSlumLimit).ToList();
            if (availableSlums.Count == 0)
            {
                return false;
            }

            var randomSlum = availableSlums[Tools.GetRandom(availableSlums.Count)];
            this.homelesses.Add(new Homeless(
                randomSlum.X + (randomSlum.Width / 2),
                Offset.Floor,
                Direction.Right,
                randomSlum.X
            ));
            return true;
        }

        private void UniEnemyDie(BasePerson enemy)
        {
            if (enemy is Enemy)
            {
                this.EnemyDie((Enemy)enemy);
            }
            else if (enemy is Pig)
            {
                this.PigDie((Pig)enemy);
            }
            else if (enemy is Lenin)
            {
                this.LeninDie((Lenin)enemy);
            }
        }

        private void EnemyDie(Enemy enemy)
        {
            // maybe drop something?
            if (Tools.RandomChance(this.enemyDropProbability))
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
            if (Tools.RandomChance(this.pigDropProbability))
            {
                this.coins.Add(new Coin(pig.X, Offset.Floor2));
            }

            Audio.PlayRandomSound("PigDeaths");
            this.player.Kills++;
            pig.Dead = true;
        }

        private void LeninDie(Lenin lenin)
        {
            // maybe drop something?
            if (Tools.RandomChance(this.leninDropProbability))
            {
                this.coins.Add(new Coin(lenin.X, Offset.Floor2));
            }

            Audio.PlaySound("LeninDeath");
            this.player.Kills++;
            lenin.Dead = true;
        }

        private int GetUpgradeAttackAddition()
        {
            return (Game1.CenterLevel - 1) * 2;
        }

        private void UpgradeCenter()
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
        }
    }
}
