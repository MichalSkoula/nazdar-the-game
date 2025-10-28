using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using Nazdar.Weather;
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
                Game1.MessageBuffer.AddMessage(Translation.Get("message.gameSaved"), MessageType.Info);

                // back to menu
                if (this.Game.Village == 0)
                {
                    this.Game.LoadScreen(typeof(Screens.SurvivalScreen));
                }
                else
                {
                    this.Game.LoadScreen(typeof(Screens.MapScreen));
                }
            }

#if DEBUG
            // cheats
            if (Keyboard.HasBeenPressed(Keys.C) || Gamepad.HasBeenPressed(Buttons.RightTrigger))
            {
                this.player.Health = 100;
                this.player.Cartridges = 100;
                this.player.Money = 100;
                this.CreateHomeless();
            }
            if (Keyboard.HasBeenPressed(Keys.K) || Gamepad.HasBeenPressed(Buttons.LeftTrigger))
            {
                this.soldiers.OrderBy(f => Math.Abs(f.X - player.X)).First().ToDelete = true;
            }
#endif

            // player, camera, parallax stuff
            this.player.Update(this.Game.DeltaTime);
            this.camera.Follow(this.player);
            this.parallaxManager.Update(this.player.X, Sky.GetParallaxColor(this.dayPhase, this.dayPhaseTimer));

            // game objects
            this.UpdateEnemies();
            this.UpdatePigs();
            this.UpdateLenins();
            this.UpdateSoldiers();
            this.UpdatePeasants();
            this.UpdateFarmers();
            this.UpdateHomelesses();
            this.UpdateMedics();
            this.UpdateCoins();

            // buildings
            this.center?.Update(this.Game.DeltaTime);
            this.locomotive?.Update(this.Game.DeltaTime);
            this.ship?.Update(this.Game.DeltaTime);
            this.treasure?.Update(this.Game.DeltaTime);
            this.UpdateArmories();
            this.UpdateArsenals();
            this.UpdateTowers();
            this.UpdateFarms();
            this.UpdateHospitals();
            this.UpdateMarkets();
            this.UpdateRails();

            // collisions
            this.UpdateAllEnemiesCollisions();
            this.UpdateActionsCollisions();
            this.UpdateThingsCollisions();

            // other
            this.UpdateDayPhaseAndSky();
            this.UpdateDisease();
            this.CheckIfWeCanGoToAnotherVillageOrWeWon();
            this.Tutorial();
        }

        private void UpdateEnemies()
        {
            // create enemy?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult, every village also

            int randomBase = this.GetNewEnemyProbability();
            // @survivalMode x4
            if (this.Game.Village == 0)
            {
                randomBase = (int)(randomBase / 4);
            }

            if (this.dayPhase == DayPhase.Night && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.RandomChance(randomBase))
            {
                Audio.PlayRandomSound("EnemySpawns");

                // every day it gets more difficult
                int newEnemyCaliber = Enemy.DefaultCaliber + this.player.Days;
                if (newEnemyCaliber > newEnemyMaxCaliber)
                {
                    newEnemyCaliber = newEnemyMaxCaliber;
                }

                // choose direction
                // at last level, create enemies only on the left
                if (this.Game.Village == Game1.MaxVillage || Tools.RandomChance(2))
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
            // create pigs?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult, every village also

            // pigs only in survival mode or village >= 3
            int randomBase = this.GetNewEnemyProbability() * 4; // for pigs = rare
            bool pigsEnabled = false;
            // @survivalMode x4
            if (this.Game.Village >= 3 || (this.Game.Village == 0 && player.Days >= 3))
            {
                pigsEnabled = true;
                randomBase /= 4;
            }

            if (this.dayPhase == DayPhase.Night && pigsEnabled && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.RandomChance(randomBase))
            {
                Audio.PlayRandomSound("PigSpawns");

                // every day it gets more difficult
                int newPigCaliber = Pig.DefaultCaliber + this.player.Days;
                if (newPigCaliber > newPigMaxCaliber)
                {
                    newPigCaliber = newPigMaxCaliber;
                }

                // choose direction
                // at last level, create enemies only on the left
                if (this.Game.Village == Game1.MaxVillage || Tools.RandomChance(2))
                {
                    this.pigs.Add(new Pig(0, Offset.FloorPig, Direction.Right, caliber: newPigCaliber));
                }
                else
                {
                    this.pigs.Add(new Pig(MapWidth, Offset.FloorPig, Direction.Left, caliber: newPigCaliber));
                }
            }

            // update pigs
            foreach (Pig pig in this.pigs)
            {
                pig.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateLenins()
        {
            // create lenin?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult, every village also

            // ONLY AT LAST LEVEL or survival days > 5
            bool leninEnabled = false;
            int randomBase = this.GetNewEnemyProbability() * 8; // for lenin = super rare
            // @survivalMode x4
            if (this.Game.Village == Game1.MaxVillage || (this.Game.Village == 0 && player.Days >= 6))
            {
                leninEnabled = true;
                randomBase /= 4;
            }

            if (this.dayPhase == DayPhase.Night && leninEnabled && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.RandomChance(randomBase))
            {
                Audio.PlaySound("LeninSpawn");

                // every day it gets more difficult
                int newLeninCaliber = Lenin.DefaultCaliber + this.player.Days;
                if (newLeninCaliber > newLeninMaxCaliber)
                {
                    newLeninCaliber = newLeninMaxCaliber;
                }

                // choose direction
                // at last level, create enemies only on the left
                // otherwise it is survival, so on both sides
                if (this.Game.Village == Game1.MaxVillage || Tools.RandomChance(2))
                {
                    this.lenins.Add(new Lenin(0, Offset.FloorLenin, Direction.Right, caliber: newLeninCaliber));
                }
                else
                {
                    this.lenins.Add(new Lenin(MapWidth, Offset.FloorLenin, Direction.Left, caliber: newLeninCaliber));
                }
            }

            // update lenins
            foreach (Lenin lenin in this.lenins)
            {
                lenin.Update(this.Game.DeltaTime);
            }
        }

        private int GetNewEnemyProbability()
        {
            int randomBase = newEnemyDefaultProbability - this.Game.Village * 10 - this.player.Days * 4;
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

                // last level? everyone on the left
                if (Game1.MaxVillage == this.Game.Village)
                {
                    soldier.DeploymentBuilding = null;
                    if (this.leftmostTower != null && this.leftmostTower.Status == Building.Status.Built)
                    {
                        soldier.DeploymentBuilding = this.leftmostTower;
                    }
                }

                // can shoot at closest enemy?
                int range = Enums.Screen.Width / 3; // third of the visible screen
                foreach (BasePerson uniEnemy in this.allEnemies.Where(ue => ue.Dead == false).OrderBy(e => Math.Abs(e.X - soldier.X)))
                {
                    if (Math.Abs(uniEnemy.X - soldier.X) < range)
                    {
                        soldier.PrepareToShoot((uniEnemy.X + uniEnemy.Width / 2) < (soldier.X + soldier.Width / 2) ? Direction.Left : Direction.Right);
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

            if (this.dayPhase == DayPhase.Day && this.allEnemies.Where(enemy => enemy.Dead == false).Count() == 0 && this.farms.Count > 0)
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

            // make list of all wounded, in prioritized order
            List<BasePerson> wounded = new List<BasePerson>();
            if (this.player.Health < 100)
            {
                wounded.Add(this.player);
            }
            wounded.AddRange(this.soldiers.Where(i => i.Health < 100 && i.Dead == false));
            wounded.AddRange(this.farmers.Where(i => i.Health < 100 && i.Dead == false));
            wounded.AddRange(this.peasants.Where(i => i.Health < 100 && i.Dead == false));

            // maybe free medics?
            foreach (Medic medic in this.medics)
            {
                // does the patient exists??
                if (!wounded.Where(w => w == medic.DeploymentPerson).Any())
                {
                    medic.DeploymentPerson = null;
                }

                // remove deployment, it the job is done
                if (medic.DeploymentPerson?.Health == 100)
                {
                    medic.DeploymentPerson = null;
                }
            }

            // pair medics to wounded
            if (wounded.Count > 0)
            {
                foreach (var w in wounded)
                {
                    // already being healed?
                    bool alreadyBeingHealed = false;
                    foreach (var medic in this.medics)
                    {
                        if (medic.DeploymentPerson == w)
                        {
                            alreadyBeingHealed = true;
                            break;
                        }
                    }

                    if (alreadyBeingHealed)
                    {
                        continue;
                    }

                    // get nearest medic
                    var nearestMedic = this.medics.Where(m => m.DeploymentPerson == null).OrderBy(m => Math.Abs(m.X - w.X)).FirstOrDefault();
                    if (nearestMedic != null)
                    {
                        nearestMedic.DeploymentPerson = w;
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
            foreach (var rails in this.rails.Where(a => a.Status == Building.Status.InProcess))
            {
                this.Build(rails);
                break;
            }

            // something to get?
            foreach (var farm in this.farms.Where(a => a.ToolsCount > 0))
            {
                this.Pick(farm, false, farm.Tools.Count);
                break;
            }
            foreach (var armory in this.armories.Where(a => a.WeaponsCount > 0))
            {
                this.Pick(armory, true, armory.Weapons.Count);
                break;
            }
            foreach (var hospital in this.hospitals.Where(a => a.MedicalKitsCount > 0))
            {
                this.Pick(hospital, true, hospital.MedicalKits.Count);
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
            if (Tools.RandomChance(this.newCoinProbability))
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

        private void UpdateRails()
        {
            foreach (Rails rails in this.rails)
            {
                rails.Update(this.Game.DeltaTime);

                // Hides buildingsSpots under built rails
                if (rails.Status == Building.Status.Built)
                {
                    foreach (BuildingSpot bs in this.buildingSpots.Where(bs => bs.Type == Building.Type.Rails && bs.Hitbox.Intersects(rails.Hitbox)))
                    {
                        bs.Hide = true;
                    }
                }
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
                if (tower.Status == Building.Status.Built)
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
                    tower.CanFire = false;
                    int range = Enums.Screen.Width / 2; // half of the visible screen
                    foreach (var uniEnemy in this.allEnemies.Where(enemy => enemy.Dead == false).OrderBy(e => Math.Abs(e.X - tower.X)))
                    {
                        if (Math.Abs(uniEnemy.X - tower.X) < range)
                        {
                            tower.CanFire = true;
                            tower.PrepareToShoot(
                                (uniEnemy.X + uniEnemy.Width / 2) < (tower.X + tower.Width / 2) ? Direction.Left : Direction.Right,
                                uniEnemy.X,
                                range
                            );
                            break;
                        }
                    }
                }

                tower.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateAllEnemiesCollisions()
        {
            // enemies and player bullets
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Bullet bullet in this.player.Bullets.Where(bullet => bullet.ToDelete == false))
                {
                    if (enemy.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!enemy.TakeHit(bullet.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                    }
                }
            }

            // enemies and tower bullets
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Tower tower in this.towers)
                {
                    foreach (Bullet bullet in tower.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (enemy.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!enemy.TakeHit(bullet.Caliber / caliberDivider))
                            {
                                this.UniEnemyDie(enemy);
                            }
                        }
                    }
                }
            }

            // enemies and soldiers bullets
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Soldier soldier in this.soldiers)
                {
                    foreach (Bullet bullet in soldier.Bullets.Where(bullet => bullet.ToDelete == false))
                    {
                        if (enemy.Hitbox.Intersects(bullet.Hitbox))
                        {
                            bullet.ToDelete = true;

                            if (!enemy.TakeHit(bullet.Caliber / caliberDivider))
                            {
                                this.UniEnemyDie(enemy);
                            }
                        }
                    }
                }
            }

            // enemies and player
            if (Game1.NextLevelAnimation == false)
            {
                foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
                {
                    int caliberDivider = this.GetCaliberDivider(enemy);
                    if (this.player.Hitbox.Intersects(enemy.Hitbox))
                    {
                        MyShake.Shake();
                        MyVibration.Vibrate();

                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(this.player.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                        // player is mega strong
                        if (!this.player.TakeHit(enemy.Caliber / 2))
                        {
                            this.GameOver();
                        }
                    }
                }
            }

            // enemies and golden treasure
            if (Game1.NextLevelAnimation == false && this.treasure != null)
            {
                foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
                {
                    int caliberDivider = this.GetCaliberDivider(enemy);
                    if (this.treasure.Hitbox.Intersects(enemy.Hitbox) && this.treasure.Health > 0)
                    {
                        MyShake.Shake();
                        MyVibration.Vibrate();

                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!this.treasure.CoinTheft(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.treasureStolen"), MessageType.Fail);
                            this.GameOver();
                        }
                    }
                }
            }

            // enemies and soldiers
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Soldier soldier in this.soldiers.Where(soldier => soldier.Dead == false))
                {
                    if (!enemy.Dead && !soldier.Dead && enemy.Hitbox.Intersects(soldier.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(soldier.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                        if (!soldier.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.soldierKilled", enemy.Name), MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            soldier.Dead = true;
                        }
                    }
                }
            }

            // enemies and peasants
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Peasant peasant in this.peasants.Where(peasant => peasant.Dead == false))
                {
                    if (!enemy.Dead && !peasant.Dead && enemy.Hitbox.Intersects(peasant.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(peasant.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                        if (!peasant.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.peasantKilled", enemy.Name), MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            peasant.Dead = true;
                        }
                    }
                }
            }

            // enemies and farmers
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Farmer farmer in this.farmers.Where(farmer => farmer.Dead == false))
                {
                    if (!enemy.Dead && !farmer.Dead && enemy.Hitbox.Intersects(farmer.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(farmer.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                        if (!farmer.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.farmerKilled", enemy.Name), MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            farmer.Dead = true;
                        }
                    }
                }
            }

            // enemies and medics
            foreach (BasePerson enemy in this.allEnemies.Where(enemy => enemy.Dead == false))
            {
                int caliberDivider = this.GetCaliberDivider(enemy);
                foreach (Medic medic in this.medics.Where(medic => medic.Dead == false))
                {
                    if (!enemy.Dead && !medic.Dead && enemy.Hitbox.Intersects(medic.Hitbox))
                    {
                        // random collision?
                        if (this.RandomPeoplesCollision())
                        {
                            continue;
                        }

                        if (!enemy.TakeHit(medic.Caliber / caliberDivider))
                        {
                            this.UniEnemyDie(enemy);
                        }
                        if (!medic.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.medicKilled", enemy.Name), MessageType.Fail);
                            Audio.PlayRandomSound("SoldierDeaths");
                            medic.Dead = true;
                        }
                    }
                }
            }

            this.soldiers.RemoveAll(p => p.ToDelete);
            this.enemies.RemoveAll(p => p.ToDelete);
            this.pigs.RemoveAll(p => p.ToDelete);
            this.lenins.RemoveAll(p => p.ToDelete);
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

            // Weapons
            foreach (var armory in this.armories.Where(a => a.WeaponsCount > 0))
            {
                foreach (var peasant in this.peasants)
                {
                    if (peasant.Hitbox.Intersects(armory.Hitbox) && armory.WeaponsCount > 0)
                    {
                        // ok, peasant get weapon and turns into soldier
                        Game1.MessageBuffer.AddMessage(Translation.Get("message.peasantToSoldier"), MessageType.Success);
                        Audio.PlaySound("SoldierSpawn");
                        peasant.ToDelete = true;
                        armory.DropWeapon();
                        this.soldiers.Add(new Soldier(peasant.Hitbox.X, Offset.Floor, peasant.Direction, caliber: Soldier.DefaultCaliber + this.GetUpgradeAttackAddition()));
                    }
                }
            }

            // Tools
            foreach (var farm in this.farms.Where(a => a.ToolsCount > 0))
            {
                foreach (var peasant in this.peasants)
                {
                    if (peasant.Hitbox.Intersects(farm.Hitbox) && farm.ToolsCount > 0)
                    {
                        // ok, peasant get tool and turns into farmer
                        Game1.MessageBuffer.AddMessage(Translation.Get("message.peasantToFarmer"), MessageType.Success);
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
                        Game1.MessageBuffer.AddMessage(Translation.Get("message.peasantToMedic"), MessageType.Success);
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
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.center = new Center(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                        }
                    }
                    else if (this.center.Status == Building.Status.Built && (Game1.CenterLevel < this.Game.Village || this.Game.Village == 0) && this.center.HasBeenUpgradedToday == false)
                    {
                        // center is built - level up?
                        this.player.Action = Enums.PlayerAction.Upgrade;
                        this.player.ActionCost = Center.Cost * Game1.CenterLevel * 2;
                        if (this.player.ActionCost > Center.CostMax)
                        {
                            this.player.ActionCost = Center.CostMax;
                        }
                        this.player.ActionName = Center.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money >= this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingUpgraded"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                Game1.CenterLevel++;

                                // upgrade, to another village, etc
                                this.Upgrade();
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.armories.Add(new Armory(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // armory exists - create Weapons?
                        var armory = armories.First();
                        if (armory.Status == Building.Status.Built)
                        {
                            this.player.Action = Enums.PlayerAction.Create;
                            this.player.ActionCost = Armory.WeaponCost;
                            this.player.ActionName = Weapon.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= this.player.ActionCost)
                                {
                                    if (armory.AddWeapon())
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.weaponKitPurchased"), MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= this.player.ActionCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.armoryFull"), MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                                }
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
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
                                if (this.player.Money >= this.player.ActionCost)
                                {
                                    if (hospital.AddMedicalKit())
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.medicalKitPurchased"), MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= this.player.ActionCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.cantBuyMoreMedicalKits"), MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                                }
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.markets.Add(new Market(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    break;
                }
                // Rails?
                else if (buildingSpot.Type == Building.Type.Rails)
                {
                    var rails = this.rails.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox));
                    if (!rails.Any())
                    {
                        // no rails here - we can build them
                        this.player.Action = Enums.PlayerAction.Repair;
                        this.player.ActionCost = Rails.Cost;
                        this.player.ActionName = Rails.Name;

                        if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                        {
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.rails.Add(new Rails(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
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
                                if (this.player.Money >= this.player.ActionCost)
                                {
                                    this.player.Cartridges += Arsenal.CartridgesCount;
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.cartridgePurchased"), MessageType.Info);
                                    Audio.PlaySound("SoldierSpawn");
                                    this.player.Money -= this.player.ActionCost;
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                                }
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.towers.Add(new Tower(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess, caliber: Tower.DefaultCaliber + this.GetUpgradeAttackAdditionTowers()));
                            }
                        }
                    }
                    else
                    {
                        var tower = towers.First();
                        // upgrade is possible, if:
                        // - tower is built
                        // AND tower level is less then village level
                        // OR village is 0 (survival) and tower level is less then max village
                        if (tower.Status == Building.Status.Built && ((Game1.TowersLevel < this.Game.Village) || (this.Game.Village == 0 && Game1.TowersLevel < Game1.MaxVillage)))
                        {
                            this.player.Action = Enums.PlayerAction.Upgrade;
                            // upgrade costs tower level *2 (for survival, *4)
                            this.player.ActionCost = Game1.TowersLevel * (this.Game.Village == 0 ? 4 : 2);
                            this.player.ActionName = Tower.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= this.player.ActionCost)
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.towerUpgraded"), MessageType.Info);
                                    Audio.PlaySound("Rock");
                                    this.player.Money -= this.player.ActionCost;
                                    Game1.TowersLevel++;

                                    // upgrade caliber
                                    this.Upgrade();
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                                }
                            }
                        }
                    }
                    break;
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
                            if (this.player.Money < this.player.ActionCost)
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                                Audio.PlaySound("Rock");
                                this.player.Money -= this.player.ActionCost;
                                this.farms.Add(new Farm(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                            }
                        }
                    }
                    else
                    {
                        // farm exists - create Tools?
                        var farm = farms.First();
                        if (farm.Status == Building.Status.Built)
                        {
                            this.player.Action = Enums.PlayerAction.Create;
                            this.player.ActionCost = Farm.ToolCost;
                            this.player.ActionName = Tool.Name;

                            if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                            {
                                if (this.player.Money >= this.player.ActionCost)
                                {
                                    if (farm.AddTool())
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.toolPurchased"), MessageType.Info);
                                        Audio.PlaySound("SoldierSpawn");
                                        this.player.Money -= this.player.ActionCost;
                                    }
                                    else
                                    {
                                        Game1.MessageBuffer.AddMessage(Translation.Get("message.farmFull"), MessageType.Fail);
                                    }
                                }
                                else
                                {
                                    Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                                }
                            }
                        }
                    }
                    break;
                }
                // Locomotive? Final building? Are we ready?
                else if (buildingSpot.Type == Building.Type.Locomotive && this.locomotive == null)
                {
                    // we can build it
                    this.player.Action = Enums.PlayerAction.Repair;
                    this.player.ActionCost = Locomotive.Cost + this.Game.Village * 4;
                    this.player.ActionName = Locomotive.Name;
                    string actionEnabledText = "";

                    // only if center is maxed up
                    if (Game1.CenterLevel < this.Game.Village)
                    {
                        this.player.ActionEnabled = false;
                        actionEnabledText = "First you need to upgrade the base";
                    }
                    // and rails are repaired
                    else if (this.buildingSpots.Where(bs => bs.Hide == false && bs.Type == Building.Type.Rails).Any())
                    {
                        this.player.ActionEnabled = false;
                        actionEnabledText = "First you need to repair all rails";
                    }

                    if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                    {
                        if (!this.player.ActionEnabled)
                        {
                            Game1.MessageBuffer.AddMessage(actionEnabledText, MessageType.Fail);
                        }
                        else if (this.player.Money < this.player.ActionCost)
                        {
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                        }
                        else
                        {
                            // start building
                            Game1.MessageBuffer.AddMessage(Translation.Get("message.buildingStarted"), MessageType.Info);
                            Audio.PlaySound("Rock");
                            this.player.Money -= this.player.ActionCost;
                            this.locomotive = new Locomotive(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess);
                        }
                    }
                    break;
                }
            }

            // Ship? Is it really happening?
            if (this.ship != null && this.ship.X - this.player.X < 50 && !Game1.WonAnimation)
            {
                // we can buy it
                this.player.Action = Enums.PlayerAction.Buy;
                this.player.ActionCost = Ship.Cost;
                this.player.ActionName = Ship.Name;
                string actionEnabledText = "";
                if (Game1.CenterLevel < this.Game.Village)
                {
                    this.player.ActionEnabled = false;
                    actionEnabledText = "First you need to upgrade the base";
                }

                // only if center is maxed up
                if (Keyboard.HasBeenPressed(ControlKeys.Action) || Gamepad.HasBeenPressed(ControlButtons.Action) || TouchControls.HasBeenPressedAction())
                {
                    if (!this.player.ActionEnabled)
                    {
                        Game1.MessageBuffer.AddMessage(actionEnabledText, MessageType.Fail);
                    }
                    else if (this.player.Money < this.player.ActionCost)
                    {
                        Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                    }
                    else
                    {
                        // ok
                        this.ship.Status = Building.Status.Built;
                        Game1.MessageBuffer.AddMessage(Translation.Get("message.shipBought"), MessageType.Info);
                        Game1.MessageBuffer.SetSuperMessage("Off we go!", 30);
                        Audio.PlaySound("Rock");
                        this.player.Money -= this.player.ActionCost;
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
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.homelessHired"), MessageType.Success);
                                Audio.PlaySound("SoldierSpawn");
                                homeless.ToDelete = true;
                                this.player.Money -= Homeless.Cost;
                                this.peasants.Add(new Peasant(homeless.Hitbox.X, Offset.Floor, homeless.Direction, caliber: Peasant.DefaultCaliber + this.GetUpgradeAttackAddition()));
                            }
                            else
                            {
                                Game1.MessageBuffer.AddMessage(Translation.Get("message.notEnoughMoney"), MessageType.Fail);
                            }
                        }
                        break;
                    }
                }
                this.homelesses.RemoveAll(p => p.ToDelete);
            }
        }

        private void UpdateDayPhaseAndSky()
        {
            this.dayPhaseTimer -= this.Game.DeltaTime;
            if (this.dayPhaseTimer <= 0)
            {
                if (this.dayPhase == DayPhase.Day)
                {
                    Audio.PlaySound("PrepareYourself");
                    Game1.MessageBuffer.AddMessage(Translation.Get("message.enemiesComing"), MessageType.Danger);
                    this.dayPhase = DayPhase.Night;
                    this.dayPhaseTimer = (int)DayNightLength.Night;
                    Tools.Dump("enemy probability: " + this.GetNewEnemyProbability());
                }
                else
                {
                    if (this.center != null)
                    {
                        this.center.HasBeenUpgradedToday = false;
                    }

                    Game1.MessageBuffer.AddMessage(Translation.Get("message.newDawn"), MessageType.Success);
                    this.player.Days++;
                    this.dayPhase = DayPhase.Day;
                    this.dayPhaseTimer = (int)DayNightLength.Day;
                }

                Audio.SongTransition(null, this.dayPhase == DayPhase.Day ? "Day" : "Night");
            }

            this.sky.Update(this.Game.DeltaTime);
            // maybe start new apocalypse
            if (!this.sky.Active && Tools.RandomChance(newSkyApocalypseProbability))
            {
                this.sky.Start(Tools.GetRandom(20) + 10, (DropType)Assets.TilesetType[Game.Village]);
            }
        }

        private void UpdateDisease()
        {
            if (!this.disease.Status)
            {
                return;
            }

            // everyone except medics
            var list = (from x in this.peasants select (BasePerson)x).ToList();
            list.AddRange((from x in this.soldiers select (BasePerson)x).ToList());
            list.AddRange((from x in this.farmers select (BasePerson)x).ToList());
            list.Add(this.player);

            foreach (var person in list)
            {
                if (person.Dead || !Tools.RandomChance(this.diseaseProbability))
                {
                    continue;
                }

                // die?
                if (!person.TakeHit(this.disease.Power))
                {
                    if (person is Player)
                    {
                        this.GameOver();
                    }
                    else
                    {
                        person.Dead = true;
                    }
                }
            }
        }

        private void UpdateHomelesses()
        {
            // create new homeless - get probability
            int villageCoeff = this.Game.Village;
            // @survivalMode
            if (this.Game.Village == 0)
            {
                villageCoeff = 6;
            }

            int randomBase = newHomelessDefaultProbability - this.slums.Count * 128 - villageCoeff * 128;
            if (randomBase < this.newHomelessProbabilityLowLimit)
            {
                randomBase = this.newHomelessProbabilityLowLimit;
            }

            if (Tools.RandomChance(randomBase) && this.homelesses.Count < this.homelessLimit)
            {
                Game1.MessageBuffer.AddMessage(Translation.Get("message.newHomelessAvailable"), MessageType.Opportunity);
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
            return Game1.GlobalTimer % 5 != 0 || Tools.RandomChance(2);
        }

        private int GetCaliberDivider(BasePerson enemy)
        {
            if (enemy is Pig)
            {
                return 2;
            }
            if (enemy is Lenin)
            {
                return 4;
            }

            // normal enemy
            return 1;
        }

        private void Tutorial()
        {
            int messageTime = 3;

            if (this.Game.Village != 1)
            {
                return;
            }

            // no base yet? ---------------------------------------------------------------------
            if (this.player.Days == 0 && this.dayPhaseTimer < 60 && this.center == null)
            {
                Game1.MessageBuffer.SetSuperMessage("I should build the Base", messageTime, true);
            }

            if (this.center == null)
            {
                return;
            }

            // base building started but no peasants?
            if (this.center.Status == Building.Status.InProcess && this.peasants.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to build the Base", messageTime, true);
            }

            // no farms? -------------------------------------------------------------------------
            if (this.center.Status == Building.Status.Built && this.farms.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should build some farms to generate money", messageTime, true);
            }

            // first farm to be built but no peasants?
            if (this.peasants.Count == 0 && this.farms.Count > 0 && this.farms[0].Status == Building.Status.InProcess)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to build the farm", messageTime, true);
            }

            // first farm built but no farmers or tools
            if (this.farmers.Count == 0 && this.farms.Count > 0 && this.farms[0].Status == Building.Status.Built && this.farms[0].Tools.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should create some farm tools to be able to recruit farmers.", messageTime, true);
            }

            // first farm built but no farmers or tools
            if (this.farmers.Count == 0 && this.peasants.Count == 0 && this.farms.Count > 0 && this.farms[0].Status == Building.Status.Built && this.farms[0].Tools.Count > 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to become farmers.", messageTime, true);
            }

            if (this.farmers.Count == 0)
            {
                return;
            }

            // build Armory -----------------------------------------------------------------------
            if (this.farms.Count > 0 && this.armories.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should build the Armory to be able to recruit soldiers.", messageTime, true);
            }

            // first armory to be built but no peasants?
            if (this.peasants.Count == 0 && this.armories.Count > 0 && this.armories[0].Status == Building.Status.InProcess)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to build the Armory", messageTime, true);
            }

            // first armory built but no soldiers or weapons
            if (this.soldiers.Count == 0 && this.armories.Count > 0 && this.armories[0].Status == Building.Status.Built && this.armories[0].Weapons.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should create some weapons to be able to recruit soldiers.", messageTime, true);
            }

            // first armory built but no soldiers or tools
            if (this.soldiers.Count == 0 && this.peasants.Count == 0 && this.armories.Count > 0 && this.armories[0].Status == Building.Status.Built && this.armories[0].Weapons.Count > 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to become soldiers.", messageTime, true);
            }

            if (this.soldiers.Count == 0)
            {
                return;
            }

            // build Arsenal -----------------------------------------------------------------------
            if (this.arsenals.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should build the Arsenal to be able to buy cartridges.", messageTime, true);
            }

            if (this.arsenals.Count > 0 && this.arsenals[0].Status == Building.Status.InProcess && this.peasants.Count == 0)
            {
                Game1.MessageBuffer.SetSuperMessage("I should hire some people to build the Arsenal", messageTime, true);
            }

            // enought of this
            if (this.arsenals.Count > 0 && this.arsenals[0].Status == Building.Status.Built)
            {
                Game1.MessageBuffer.SetSuperMessage("I should defend the base and make enought money to repair the Locomotive.", messageTime, true);
            }
        }
    }
}
