using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        public override void Update(GameTime gameTime)
        {
            // save & back to menu
            if (Keyboard.HasBeenPressed(Keys.Escape) || Gamepad.HasBeenPressed(Buttons.Start) || Gamepad.HasBeenPressed(Buttons.B) || TouchControls.HasBeenPressedSelect())
            {
                // save
                this.saveFile.Save(this.GetSaveData());
                Game1.MessageBuffer.AddMessage("Game saved", MessageType.Info);

                // back to menu
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // player
            this.player.Update(this.Game.DeltaTime, this.camera);
            this.camera.Follow(this.player);

            // game objects
            this.UpdateEnemies();
            this.UpdatePigs();
            this.UpdateSoldiers();
            this.UpdatePeasants();
            this.UpdateFarmers();
            this.UpdateHomelesses();
            this.UpdateMedics();
            this.UpdateCoins();

            // buildings
            this.center?.Update(this.Game.DeltaTime);
            this.locomotive?.Update(this.Game.DeltaTime);
            this.UpdateArmories();
            this.UpdateArsenals();
            this.UpdateTowers();
            this.UpdateFarms();
            this.UpdateHospitals();
            this.UpdateMarkets();

            // collisions
            this.UpdateEnemiesCollisions();
            this.UpdatePigsCollisions();
            this.UpdateActionsCollisions();
            this.UpdateThingsCollisions();

            // other
            this.UpdateDayPhase();
            this.CheckIfWeCanGoToAnotherVillage();
        }

        private void UpdateEnemies()
        {
            // create enemy?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult, every village also

            int randomBase = this.GetNewEnemyProbability();

            if (this.dayPhase == DayPhase.Night && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.GetRandom(randomBase) == 0)
            {
                Audio.PlayRandomSound("EnemySpawns");

                // every day it gets more difficult
                int newEnemyCaliber = Enemy.DefaultCaliber + this.player.Days;
                if (newEnemyCaliber > newEnemyMaxCaliber)
                {
                    newEnemyCaliber = newEnemyMaxCaliber;
                }

                // choose direction
                if (Tools.GetRandom(2) == 0)
                {
                    this.enemies.Add(new Enemy(0, Offset.Floor, Direction.Right, caliber: newEnemyCaliber, villageNumber: this.Game.Village));
                }
                else
                {
                    this.enemies.Add(new Enemy(MapWidth, Offset.Floor, Direction.Left, caliber: newEnemyCaliber, villageNumber: this.Game.Village));
                }
            }

            // update enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Update(this.Game.DeltaTime);
            }
        }

        private void UpdatePigs()
        {
            // create enemy?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult, every village also

            int randomBase = this.GetNewEnemyProbability() * 4; // for pigs

            if (this.dayPhase == DayPhase.Night && this.Game.Village >= 3 && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.GetRandom(randomBase) == 0)
            {
                Audio.PlayRandomSound("PigSpawns");

                // every day it gets more difficult
                int newPigCaliber = Pig.DefaultCaliber + this.player.Days;
                if (newPigCaliber > newPigMaxCaliber)
                {
                    newPigCaliber = newPigMaxCaliber;
                }

                // choose direction
                if (Tools.GetRandom(2) == 0)
                {
                    this.pigs.Add(new Pig(0, Offset.Floor3, Direction.Right, caliber: newPigCaliber));
                }
                else
                {
                    this.pigs.Add(new Pig(MapWidth, Offset.Floor3, Direction.Left, caliber: newPigCaliber));
                }
            }

            // update pigs
            foreach (Pig pig in this.pigs)
            {
                pig.Update(this.Game.DeltaTime);
            }
        }

        private int GetNewEnemyProbability()
        {
            int randomBase = newEnemyDefaultProbability - this.Game.Village * 32 - this.player.Days * 4;
            if (randomBase < this.newEnemyProbabilityLowLimit)
            {
                randomBase = this.newEnemyProbabilityLowLimit;
            }
            return randomBase;
        }

        private void UpdateSoldiers()
        {
            bool left = true;
            foreach (Soldier soldier in this.soldiers)
            {
                left = !left;

                soldier.DeploymentBuilding = null;
                if (left && this.leftmostTower != null)
                {
                    // leftmost tower exists?
                    soldier.DeploymentBuilding = this.leftmostTower;
                }
                else if (!left && this.rightmostTower != null)
                {
                    // rightmost tower exists?
                    soldier.DeploymentBuilding = this.rightmostTower;
                }

                // can shoot at closest enemy?
                int range = Enums.Screen.Width / 3; // third of the visible screen
                foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false).OrderBy(e => Math.Abs(e.X - soldier.X)))
                {
                    if (Math.Abs(enemy.X - soldier.X) < range)
                    {
                        soldier.PrepareToShoot((enemy.X + enemy.Width / 2) < (soldier.X + soldier.Width / 2) ? Direction.Left : Direction.Right);
                        break;
                    }
                }

                soldier.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateFarmers()
        {
            if (this.farmers.Count == 0)
            {
                return;
            }

            foreach (Farmer farmer in this.farmers)
            {
                farmer.DeploymentBuilding = null;
            }

            if (this.dayPhase == DayPhase.Day && this.enemies.Where(enemy => enemy.Dead == false).Count() == 0 && this.farms.Count > 0)
            {
                // take only built farms
                List<Farm> builtFarms = this.farms.Where(f => f.Status == Building.Status.Built).ToList();

                if (builtFarms.Count > 0)
                {
                    // distribute farmers to farms
                    int f = 0; // farm index
                    int[] farmLimitArray = new int[builtFarms.Count];
                    for (int i = 0; i < farmers.Count; i++, f++)
                    {
                        f %= builtFarms.Count;
                        if (farmLimitArray[f] < this.farmLimit)
                        {
                            this.farmers.ElementAt(i).DeploymentBuilding = builtFarms[f];
                            farmLimitArray[f]++;
                        }
                    }
                }
            }

            // update them
            foreach (Farmer farmer in this.farmers)
            {
                farmer.Update(this.Game.DeltaTime, this.coins);
            }
        }

        private void UpdateMedics()
        {
            if (this.medics.Count == 0)
            {
                return;
            }

            // remove deployment
            foreach (Medic medic in this.medics)
            {
                medic.DeploymentPerson = null;
            }

            if (this.dayPhase == DayPhase.Day && this.enemies.Where(enemy => enemy.Dead == false).Count() == 0)
            {
                // make list of all wounded
                List<BasePerson> wounded = new List<BasePerson>();
                wounded.AddRange(this.soldiers.Where(i => i.Health < 100));
                wounded.AddRange(this.farmers.Where(i => i.Health < 100));
                wounded.AddRange(this.peasants.Where(i => i.Health < 100));
                wounded = wounded.OrderBy(w => w.Health).ToList();

                if (wounded.Count > 0)
                {
                    // distribute medics to wounded
                    int w = 0; // wounded index
                    int[] woundedLimitArray = new int[wounded.Count];
                    for (int i = 0; i < medics.Count; i++, w++)
                    {
                        w %= wounded.Count;
                        if (woundedLimitArray[w] == 0)
                        {
                            this.medics.ElementAt(i).DeploymentPerson = wounded[w];
                            woundedLimitArray[w]++;
                        }
                    }
                }
            }

            // update them
            foreach (Medic medic in this.medics)
            {
                medic.Update(this.Game.DeltaTime);
            }
        }

        private void UpdatePeasants()
        {
            // reset his intentions
            foreach (Peasant peasant in this.peasants)
            {
                peasant.IsBuildingHere = null;
                peasant.IsRunningForItem = null;
            }

            // if the air is clean, he could do something
            if (this.dayPhase == DayPhase.Day && this.enemies.Where(enemy => enemy.Dead == false).Count() == 0)
            {
                // something to build?
                foreach (var armory in this.armories.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(armory);
                    break;
                }
                foreach (var hospital in this.hospitals.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(hospital);
                    break;
                }
                foreach (var arsenal in this.arsenals.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(arsenal);
                    break;
                }
                foreach (var tower in this.towers.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(tower);
                    break;
                }
                foreach (var farm in this.farms.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(farm);
                    break;
                }
                if (this.center != null && this.center.Status == Building.Status.InProcess)
                {
                    this.Build(this.center);
                }
                if (this.locomotive != null && this.locomotive.Status == Building.Status.InProcess)
                {
                    this.Build(this.locomotive);
                }
                foreach (var market in this.markets.Where(a => a.Status == Building.Status.InProcess))
                {
                    this.Build(market);
                    break;
                }

                // something to get?
                foreach (var farm in this.farms.Where(a => a.ToolsCount > 0))
                {
                    this.Pick(farm);
                    break;
                }
            }

            // there are things he can do always
            foreach (var armory in this.armories.Where(a => a.WeaponsCount > 0))
            {
                this.Pick(armory);
                break;
            }
            foreach (var hospital in this.hospitals.Where(a => a.MedicalKitsCount > 0))
            {
                this.Pick(hospital);
                break;
            }

            foreach (Peasant peasant in this.peasants)
            {
                peasant.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateCoins()
        {
            // create new?
            if (Tools.GetRandom(this.newCoinProbability) == 1)
            {
                this.coins.Add(new Coin(Tools.GetRandom(VillageScreen.MapWidth), Offset.Floor2));
            }

            // update 
            foreach (Coin coin in this.coins)
            {
                coin.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateArmories()
        {
            foreach (Armory armory in this.armories)
            {
                armory.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateHospitals()
        {
            foreach (Hospital hospital in this.hospitals)
            {
                hospital.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateMarkets()
        {
            foreach (Market market in this.markets)
            {
                market.Update(this.Game.DeltaTime, this.coins);
            }
        }

        private void UpdateArsenals()
        {
            foreach (Arsenal arsenal in this.arsenals)
            {
                arsenal.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateFarms()
        {
            foreach (Farm farm in this.farms)
            {
                farm.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateTowers()
        {
            this.leftmostTower = this.rightmostTower = null;

            foreach (Tower tower in this.towers)
            {
                // is it left or rightmost tower (to send soldiers to this tower)
                if (this.center != null)
                {
                    if (tower.X < this.center.X && (this.leftmostTower == null || tower.X < this.leftmostTower.X))
                    {
                        this.leftmostTower = tower;
                    }
                    else if (tower.X > (this.center.X + this.center.Width) && (this.rightmostTower == null || tower.X > this.rightmostTower.X))
                    {
                        this.rightmostTower = tower;
                    }
                }

                // can shoot at closest enemy?
                if (tower.Status == Building.Status.Built)
                {
                    tower.CanFire = false;
                    int range = Enums.Screen.Width / 2; // half of the visible screen
                    foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false).OrderBy(e => Math.Abs(e.X - tower.X)))
                    {
                        if (Math.Abs(enemy.X - tower.X) < range)
                        {
                            tower.CanFire = true;
                            tower.PrepareToShoot(
                                (enemy.X + enemy.Width / 2) < (tower.X + tower.Width / 2) ? Direction.Left : Direction.Right,
                                enemy.X,
                                range
                            );
                            break;
                        }
                    }
                }

                tower.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateEnemiesCollisions()
        {
            // enemies and player bullets
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Bullet bullet in this.player.Bullets.Where(bullet => bullet.ToDelete == false))
                {
                    if (enemy.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!enemy.TakeHit(bullet.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                    }
                }
            }

            // enemies and tower bullets
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Tower tower in this.towers)
                {
                    foreach (Bullet bullet in tower.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (enemy.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!enemy.TakeHit(bullet.Caliber))
                            {
                                this.EnemyDie(enemy);
                            }
                        }
                    }
                }
            }

            // enemies and soldiers bullets
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Soldier soldier in this.soldiers)
                {
                    foreach (Bullet bullet in soldier.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (enemy.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!enemy.TakeHit(bullet.Caliber))
                            {
                                this.EnemyDie(enemy);
                            }
                        }
                    }
                }
            }

            // enemies and player
            if (Game1.NextLevelAnimation == false)
            {
                foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
                {
                    if (this.player.Hitbox.Intersects(enemy.Hitbox))
                    {
                        MyShake.Shake();
                        MyVibration.Vibrate();

                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(this.player.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                        // player is mega strong
                        if (!this.player.TakeHit(enemy.Caliber / 2))
                        {
                            this.GameOver();
                        }
                    }
                }
            }

            // enemies and soldiers
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Soldier soldier in this.soldiers.Where(soldier => soldier.Dead == false))
                {
                    if (!enemy.Dead && !soldier.Dead && enemy.Hitbox.Intersects(soldier.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(soldier.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                        if (!soldier.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Heroic soldier killed by enemy", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            soldier.Dead = true;
                        }
                    }
                }
            }

            // enemies and peasants
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Peasant peasant in this.peasants.Where(peasant => peasant.Dead == false))
                {
                    if (!enemy.Dead && !peasant.Dead && enemy.Hitbox.Intersects(peasant.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(peasant.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                        if (!peasant.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent peasant killed by enemy", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            peasant.Dead = true;
                        }
                    }
                }
            }

            // enemies and farmers
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Farmer farmer in this.farmers.Where(farmer => farmer.Dead == false))
                {
                    if (!enemy.Dead && !farmer.Dead && enemy.Hitbox.Intersects(farmer.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(farmer.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                        if (!farmer.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent farmer killed by enemy", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            farmer.Dead = true;
                        }
                    }
                }
            }

            // enemies and medics
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Medic medic in this.medics.Where(medic => medic.Dead == false))
                {
                    if (!enemy.Dead && !medic.Dead && enemy.Hitbox.Intersects(medic.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(medic.Caliber))
                        {
                            this.EnemyDie(enemy);
                        }
                        if (!medic.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent medic killed by enemy", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            medic.Dead = true;
                        }
                    }
                }
            }

            this.soldiers.RemoveAll(p => p.ToDelete);
            this.enemies.RemoveAll(p => p.ToDelete);
            this.peasants.RemoveAll(p => p.ToDelete);
            this.farmers.RemoveAll(p => p.ToDelete);
            this.medics.RemoveAll(p => p.ToDelete);
        }

        private void UpdatePigsCollisions()
        {
            // pigs and player bullets
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Bullet bullet in this.player.Bullets.Where(bullet => bullet.ToDelete == false))
                {
                    if (pig.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!pig.TakeHit(bullet.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                    }
                }
            }

            // pigs and tower bullets
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Tower tower in this.towers)
                {
                    foreach (Bullet bullet in tower.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (pig.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!pig.TakeHit(bullet.Caliber / 2))
                            {
                                this.PigDie(pig);
                            }
                        }
                    }
                }
            }

            // pigs and soldiers bullets
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Soldier soldier in this.soldiers)
                {
                    foreach (Bullet bullet in soldier.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (pig.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!pig.TakeHit(bullet.Caliber / 2))
                            {
                                this.PigDie(pig);
                            }
                        }
                    }
                }
            }

            // pigs and player
            if (Game1.NextLevelAnimation == false)
            {
                foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
                {
                    if (this.player.Hitbox.Intersects(pig.Hitbox))
                    {
                        MyShake.Shake();
                        MyVibration.Vibrate();

                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!pig.TakeHit(this.player.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                        // player is mega strong
                        if (!this.player.TakeHit(pig.Caliber / 2))
                        {
                            this.GameOver();
                        }
                    }
                }
            }

            // pigs and soldiers
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Soldier soldier in this.soldiers.Where(soldier => soldier.Dead == false))
                {
                    if (!pig.Dead && !soldier.Dead && pig.Hitbox.Intersects(soldier.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!pig.TakeHit(soldier.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                        if (!soldier.TakeHit(pig.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Heroic soldier killed by pig rider", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            soldier.Dead = true;
                        }
                    }
                }
            }

            // pigs and peasants
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Peasant peasant in this.peasants.Where(peasant => peasant.Dead == false))
                {
                    if (!pig.Dead && !peasant.Dead && pig.Hitbox.Intersects(peasant.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!pig.TakeHit(peasant.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                        if (!peasant.TakeHit(pig.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent peasant killed by pig rider", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            peasant.Dead = true;
                        }
                    }
                }
            }

            // pigs and farmers
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Farmer farmer in this.farmers.Where(farmer => farmer.Dead == false))
                {
                    if (!pig.Dead && !farmer.Dead && pig.Hitbox.Intersects(farmer.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!pig.TakeHit(farmer.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                        if (!farmer.TakeHit(pig.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent farmer killed by pig rider", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            farmer.Dead = true;
                        }
                    }
                }
            }

            // pigs and medics
            foreach (Pig pig in this.pigs.Where(pig => pig.Dead == false))
            {
                foreach (Medic medic in this.medics.Where(medic => medic.Dead == false))
                {
                    if (!pig.Dead && !medic.Dead && pig.Hitbox.Intersects(medic.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!pig.TakeHit(medic.Caliber / 2))
                        {
                            this.PigDie(pig);
                        }
                        if (!medic.TakeHit(pig.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Innocent medic killed by pig rider", MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            medic.Dead = true;
                        }
                    }
                }
            }

            this.soldiers.RemoveAll(p => p.ToDelete);
            this.pigs.RemoveAll(p => p.ToDelete);
            this.peasants.RemoveAll(p => p.ToDelete);
            this.farmers.RemoveAll(p => p.ToDelete);
            this.medics.RemoveAll(p => p.ToDelete);
        }

        private void UpdateThingsCollisions()
        {
            // coins
            foreach (var coin in this.coins)
            {
                if (this.player.Hitbox.Intersects(coin.Hitbox) && this.player.CanAddMoney())
                {
                    Audio.PlaySound("Coin");
                    this.player.Money++;
                    coin.ToDelete = true;
                    break;
                }
            }
            this.coins.RemoveAll(p => p.ToDelete);

            // weapons
            foreach (var armory in this.armories.Where(a => a.WeaponsCount > 0))
            {
                foreach (var peasant in this.peasants)
                {
                    if (peasant.Hitbox.Intersects(armory.Hitbox) && armory.WeaponsCount > 0)
                    {
                        // ok, peasant get weapon and turns into soldier
                        Game1.MessageBuffer.AddMessage("Peasant => soldier", MessageType.Success);
                        Audio.PlaySound("SoldierSpawn");
                        peasant.ToDelete = true;
                        armory.DropWeapon();
                        this.soldiers.Add(new Soldier(peasant.Hitbox.X, Offset.Floor, peasant.Direction, caliber: Soldier.DefaultCaliber + this.GetUpgradeAttackAddition()));
                    }
                }
            }

            // tools
            foreach (var farm in this.farms.Where(a => a.ToolsCount > 0))
            {
                foreach (var peasant in this.peasants)
                {
                    if (peasant.Hitbox.Intersects(farm.Hitbox) && farm.ToolsCount > 0)
                    {
                        // ok, peasant get tool and turns into farmer
                        Game1.MessageBuffer.AddMessage("Peasant => farmer", MessageType.Success);
                        Audio.PlaySound("SoldierSpawn");
                        peasant.ToDelete = true;
                        farm.DropTool();
                        this.farmers.Add(new Farmer(peasant.Hitbox.X, Offset.Floor, peasant.Direction, caliber: Farmer.DefaultCaliber + this.GetUpgradeAttackAddition()));
                    }
                }
            }

            // medical kits
            foreach (var hospital in this.hospitals.Where(a => a.MedicalKitsCount > 0))
            {
                foreach (var peasant in this.peasants)
                {
                    if (peasant.Hitbox.Intersects(hospital.Hitbox) && hospital.MedicalKitsCount > 0)
                    {
                        // ok, peasant get medical kit and turns into medic
                        Game1.MessageBuffer.AddMessage("Peasant => medic", MessageType.Success);
                        Audio.PlaySound("SoldierSpawn");
                        peasant.ToDelete = true;
                        hospital.DropMedicalKit();
                        this.medics.Add(new Medic(peasant.Hitbox.X, Offset.Floor, peasant.Direction, caliber: Medic.DefaultCaliber + this.GetUpgradeAttackAddition()));
                    }
                }
            }

            this.peasants.RemoveAll(p => p.ToDelete);
        }

        private void UpdateActionsCollisions()
        {
            this.player.Action = null;
            this.player.ActionCost = 0;
            this.player.ActionName = null;
            this.player.ActionEnabled = true;

            foreach (var buildingSpot in this.buildingSpots)
            {
                if (!this.player.Hitbox.Intersects(buildingSpot.Hitbox))
                {
                    continue;
                }

                // Center?
                if (buildingSpot.Type == Building.Type.Center)
                {
                    if (this.center == null)
                    {
                        // no center - we can build it
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Center.Cost;
                        this.player.ActionName = Center.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money >= this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.center = new Center(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                        }
                    }
                    else if (this.center.Status == Building.Status.Built && this.center.Level < this.Game.Village && this.center.HasBeenUpgradedToday == false)
                    {
                        // center is built - level up?
                        this.player.Action = Enums.PlayerAction.Upgrade;
                        this.player.ActionCost = Center.Cost * (this.center.Level) * 2;
                        if (this.player.ActionCost > Center.CostMax)
                        {
                            this.player.ActionCost = Center.CostMax;
                        }
                        this.player.ActionName = Center.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money >= this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage("Building upgraded", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.center.Level++;

                                // upgrade, to another village, etc
                                this.Upgrade();
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                        }
                    }
                }

                // center must be built first
                if (this.center == null || this.center.Status != Building.Status.Built)
                {
                    continue;
                }

                // Armory?
                if (buildingSpot.Type == Building.Type.Armory)
                {
                    var armories = this.armories.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (armories.Count() == 0)
                    {
                        // no armory here - we can build something
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Armory.Cost;
                        this.player.ActionName = Armory.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Armory.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Armory.Cost;
                                this.armories.Add(new Armory(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // armory exists - create weapons?
                        var armory = armories.First();
                        if (armory.Status == Building.Status.Built)
                        {
                            this.player.Action = Enums.PlayerAction.Create;
                            this.player.ActionCost = Armory.WeaponCost;
                            this.player.ActionName = Weapon.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= Armory.WeaponCost)
                                {
                                    if (armory.AddWeapon())
                                    {
                                        Game1.MessageBuffer.AddMessage("Weapon kit purchased", MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= Armory.WeaponCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage("Armory is full", MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                                }
                            }
                        }
                    }
                }
                // Hospital?
                else if (buildingSpot.Type == Building.Type.Hospital)
                {
                    var hospitals = this.hospitals.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (hospitals.Count() == 0)
                    {
                        // no hospital here - we can build something
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Hospital.Cost;
                        this.player.ActionName = Hospital.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Hospital.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Hospital.Cost;
                                this.hospitals.Add(new Hospital(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // hospital exists - create medical kits?
                        var hospital = hospitals.First();
                        if (hospital.Status == Building.Status.Built)
                        {
                            this.player.Action = Enums.PlayerAction.Create;
                            this.player.ActionCost = Hospital.MedicalKitCost;
                            this.player.ActionName = MedicalKit.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= Hospital.MedicalKitCost)
                                {
                                    if (hospital.AddMedicalKit())
                                    {
                                        Game1.MessageBuffer.AddMessage("Medical kit purchased", MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= Hospital.MedicalKitCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage("Can't buy any more medical kits", MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                                }
                            }
                        }
                    }
                }
                // Market?
                else if (buildingSpot.Type == Building.Type.Market)
                {
                    var markets = this.markets.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (markets.Count() == 0)
                    {
                        // no market here - we can build something
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Market.Cost;
                        this.player.ActionName = Market.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Market.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Market.Cost;
                                this.markets.Add(new Market(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                }
                // Arsenal?
                else if (buildingSpot.Type == Building.Type.Arsenal)
                {
                    var arsenals = this.arsenals.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (arsenals.Count() == 0)
                    {
                        // no arsenal here - we can build something
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Arsenal.Cost;
                        this.player.ActionName = Arsenal.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Arsenal.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Arsenal.Cost;
                                this.arsenals.Add(new Arsenal(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // arsenal exists - buy bullets?
                        var arsenal = arsenals.First();
                        if (arsenal.Status == Building.Status.Built && this.player.CanAddCartridge())
                        {
                            this.player.Action = Enums.PlayerAction.Buy;
                            this.player.ActionCost = Arsenal.CartridgesCost;
                            this.player.ActionName = Arsenal.CartridgeName;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= Arsenal.CartridgesCost)
                                {
                                    this.player.Cartridges += Arsenal.CartridgesCount;
                                    Game1.MessageBuffer.AddMessage("Cartridge purchased", MessageType.Info);
                                    Audio.PlaySound("SoldierSpawn");
                                    this.player.Money -= Arsenal.CartridgesCost;
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                                }
                            }
                        }
                    }
                }
                // Tower?
                else if (buildingSpot.Type == Building.Type.Tower)
                {
                    var towers = this.towers.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (towers.Count() == 0)
                    {
                        // no tower - we can build something
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Tower.Cost;
                        this.player.ActionName = Tower.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Tower.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Tower.Cost;
                                this.towers.Add(new Tower(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                }
                // Farm?
                else if (buildingSpot.Type == Building.Type.Farm)
                {
                    var farms = this.farms.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (farms.Count() == 0)
                    {
                        // no farm here on building spot, we can build it
                        this.player.Action = Enums.PlayerAction.Build;
                        this.player.ActionCost = Farm.Cost;
                        this.player.ActionName = Farm.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < Farm.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= Farm.Cost;
                                this.farms.Add(new Farm(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // farm exists - create tools?
                        var farm = farms.First();
                        if (farm.Status == Building.Status.Built)
                        {
                            this.player.Action = Enums.PlayerAction.Create;
                            this.player.ActionCost = Farm.ToolCost;
                            this.player.ActionName = Tool.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= Farm.ToolCost)
                                {
                                    if (farm.AddTool())
                                    {
                                        Game1.MessageBuffer.AddMessage("Tool purchased", MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= Farm.ToolCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage("Farm is full", MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                                }
                            }
                        }
                    }
                }
                // Locomotive? Final building? Are we ready?
                else if (buildingSpot.Type == Building.Type.Locomotive && this.locomotive == null)
                {

                    // we can build it
                    this.player.Action = Enums.PlayerAction.Build;
                    this.player.ActionCost = Locomotive.Cost;
                    this.player.ActionName = Locomotive.Name + " and continue the journey!";
                    this.player.ActionEnabled = this.center.Level == this.Game.Village ? true : false;

                    // only if center is maxed up
                    if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                    {
                        if (this.player.Money < Locomotive.Cost)
                        {
                            Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                        }
                        else if (!this.player.ActionEnabled)
                        {
                            Game1.MessageBuffer.AddMessage("You must upgrade center base first", MessageType.Fail);
                        }
                        else
                        {
                            // save
                            this.saveFile.Save(this.GetSaveData());
                            Game1.MessageBuffer.AddMessage("Game saved", MessageType.Info);

                            // start building
                            Game1.MessageBuffer.AddMessage("Building started", MessageType.Info);
                            Audio.PlaySound("Rock");
                            this.player.Money -= Locomotive.Cost;
                            this.locomotive = new Locomotive(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess);
                        }
                    }
                }
            }

            // hire homelesses?
            if (this.player.Action == null)
            {
                foreach (var homeless in this.homelesses)
                {
                    if (this.player.Hitbox.Intersects(homeless.Hitbox))
                    {
                        this.player.Action = Enums.PlayerAction.Hire;
                        this.player.ActionCost = Homeless.Cost;
                        this.player.ActionName = Homeless.Name;

                        // hire homeless man? create peasant
                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money >= Homeless.Cost)
                            {
                                Game1.MessageBuffer.AddMessage("Homeless hired => peasant", MessageType.Success);
                                Audio.PlaySound("SoldierSpawn");
                                homeless.ToDelete = true;
                                this.player.Money -= Homeless.Cost;
                                this.peasants.Add(new Peasant(homeless.Hitbox.X, Offset.Floor, homeless.Direction, caliber: Peasant.DefaultCaliber + this.GetUpgradeAttackAddition()));
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage("Not enough money", MessageType.Fail);
                            }
                        }
                        break;
                    }
                }
                this.homelesses.RemoveAll(p => p.ToDelete);
            }
        }

        private void UpdateDayPhase()
        {
            this.dayPhaseTimer -= this.Game.DeltaTime;
            if (this.dayPhaseTimer <= 0)
            {

                if (this.dayPhase == DayPhase.Day)
                {
                    Game1.MessageBuffer.AddMessage("Brace yourselfs, enemies are coming", MessageType.Danger);
                    this.dayPhase = DayPhase.Night;
                    this.dayPhaseTimer = (int)DayNightLength.Night;
                }
                else
                {
                    this.saveFile.Save(this.GetSaveData());
                    Game1.MessageBuffer.AddMessage("Game saved", MessageType.Info);

                    if (this.center != null)
                    {
                        this.center.HasBeenUpgradedToday = false;
                    }

                    Game1.MessageBuffer.AddMessage("New dawn", MessageType.Success);
                    this.player.Days++;
                    this.dayPhase = DayPhase.Day;
                    this.dayPhaseTimer = (int)DayNightLength.Day;
                }

                Audio.SongTransition(null, this.dayPhase == DayPhase.Day ? "Day" : "Night");
            }
        }

        private void UpdateHomelesses()
        {
            // create new?
            int randomBase = newHomelessDefaultProbability - this.Game.Village * 128;
            if (randomBase < this.newHomelessProbabilityLowLimit)
            {
                randomBase = this.newHomelessProbabilityLowLimit;
            }

            if (Tools.GetRandom(randomBase) == 1 && this.homelesses.Count < this.homelessLimit)
            {
                Game1.MessageBuffer.AddMessage("New homeless available to hire!", MessageType.Opportunity);
                this.CreateHomeless();
            }

            // update 
            foreach (Homeless homeless in this.homelesses)
            {
                // get nearest slum and szukaj around it
                homeless.DeploymentX = this.slums.OrderBy(s => Math.Abs(s.X - homeless.X)).FirstOrDefault().X;
                homeless.Update(this.Game.DeltaTime);
            }
        }

        private bool RandomPeoplesCollision()
        {
            return Game1.GlobalTimer % 5 != 0 || Tools.GetRandom(2) != 0;
        }

    }
}
