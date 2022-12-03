using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Objects;
using Nazdar.Shared;
using Newtonsoft.Json.Linq;
using System;
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

                // back to menu
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // player
            this.player.Update(this.Game.DeltaTime);
            this.camera.Follow(this.player);

            // game objects
            this.UpdateEnemies();
            this.UpdateSoldiers();
            this.UpdatePeasants();
            this.UpdateFarmers();
            this.UpdateHomelesses();
            this.UpdateCoins();

            // buildings
            if (this.center != null)
            {
                this.center.Update(this.Game.DeltaTime);
            }
            this.UpdateArmories();
            this.UpdateArsenals();
            this.UpdateTowers();
            this.UpdateFarms();

            // collisions
            this.UpdatePeoplesCollisions();
            this.UpdateActionsCollisions();
            this.UpdateThingsCollisions();

            // other
            this.UpdateDayPhase();
        }

        private void UpdateEnemies()
        {
            // create enemy?
            // - at night
            // - in first half on night
            // - random - every day it gets more difficult
            int randomBase = this.newEnemyProbability - this.player.Days * 2;
            if (randomBase < this.newEnemyProbabilityLowLimit)
            {
                randomBase = this.newEnemyProbabilityLowLimit;
            }

            if (this.dayPhase == DayPhase.Night && this.dayPhaseTimer >= (int)Enums.DayNightLength.Night / 2 && Tools.GetRandom(randomBase) == 0)
            {
                Audio.PlayRandomSound("EnemySpawns");
                Game1.MessageBuffer.AddMessage("New enemy!", MessageType.Danger);

                // every day it gets more difficult
                int newEnemyCaliber = Enemy.DefaultCaliber + this.player.Days;
                if (newEnemyCaliber > this.newEnemyMaxCaliber)
                {
                    newEnemyCaliber = this.newEnemyMaxCaliber;
                }

                // choose direction
                if (Tools.GetRandom(2) == 0)
                {
                    this.enemies.Add(new Enemy(0, Offset.Floor, Direction.Right, caliber: newEnemyCaliber));
                }
                else
                {
                    this.enemies.Add(new Enemy(MapWidth, Offset.Floor, Direction.Left, caliber: newEnemyCaliber));
                }
            }

            // update enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateSoldiers()
        {
            bool left = true;
            foreach (Soldier soldier in this.soldiers)
            {
                left = !left;

                soldier.DeploymentX = null;
                if (left && this.leftmostTowerX != null)
                {
                    // leftmost tower exists?
                    soldier.DeploymentX = this.leftmostTowerX;
                }
                else if (!left && this.rightmostTowerX != null)
                {
                    // rightmost tower exists?
                    soldier.DeploymentX = this.rightmostTowerX;
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

            if (this.dayPhase == DayPhase.Night || this.enemies.Where(enemy => enemy.Dead == false).Count() > 0)
            {
                // at night, go to the base and do not harvest
                foreach (Farmer farmer in this.farmers)
                {
                    farmer.DeploymentX = this.center.X + this.center.Width / 2;
                    farmer.CanBeFarming = false;
                }
            }
            else if (this.farms.Count > 0)
            {
                // distribute farmers to farms
                int f = 0; // farm index
                int[] farmLimitArray = new int[this.farms.Count];
                for (int i = 0; i < farmers.Count; i++, f++)
                {
                    f = f % farms.Count;
                    if (this.farms[f].Status != Building.Status.InProcess && farmLimitArray[f] < this.farmLimit)
                    {
                        this.farmers.ElementAt(i).DeploymentX = this.farms[f].X + this.farms[f].Width / 2;
                        this.farmers.ElementAt(i).CanBeFarming = true;
                        farmLimitArray[f]++;
                    }
                }
            }

            // update them
            foreach (Farmer farmer in this.farmers)
            {
                if (farmer.IsFarming && Tools.GetRandom(farmingMoneyProbability) == 1)
                {
                    Game1.MessageBuffer.AddMessage("New coin from farming!", MessageType.Opportunity);
                    this.coins.Add(new Coin(farmer.X + farmer.Width / 2, Offset.Floor2));
                }
                farmer.Update(this.Game.DeltaTime);
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
            }
            foreach (var arsenal in this.arsenals.Where(a => a.Status == Building.Status.InProcess))
            {
                this.Build(arsenal);
            }
            foreach (var tower in this.towers.Where(a => a.Status == Building.Status.InProcess))
            {
                this.Build(tower);
            }
            foreach (var farm in this.farms.Where(a => a.Status == Building.Status.InProcess))
            {
                this.Build(farm);
            }
            if (this.center != null && this.center.Status == Building.Status.InProcess)
            {
                this.Build(this.center);
            }

            // something to get?
            foreach (var farm in this.farms.Where(a => a.ToolsCount > 0))
            {
                this.Pick(farm);
            }
            foreach (var armory in this.armories.Where(a => a.WeaponsCount > 0))
            {
                this.Pick(armory);
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
                Game1.MessageBuffer.AddMessage("New coin!", MessageType.Opportunity);
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
            this.leftmostTowerX = this.rightmostTowerX = null;

            foreach (Tower tower in this.towers)
            {
                // is it left or rightmost tower (to send soldiers to this tower)
                if (this.center != null)
                {
                    if (tower.X < this.center.X && (this.leftmostTowerX == null || tower.X < this.leftmostTowerX))
                    {
                        this.leftmostTowerX = tower.X;
                    }
                    else if (tower.X > (this.center.X + this.center.Width) && (this.rightmostTowerX == null || tower.X > this.rightmostTowerX))
                    {
                        this.rightmostTowerX = tower.X + tower.Width;
                    }
                }

                // can shoot at closest enemy?
                if (tower.Status == Building.Status.Built)
                {
                    int range = Enums.Screen.Width / 2; // half of the visible screen
                    foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false).OrderBy(e => Math.Abs(e.X - tower.X)))
                    {
                        if (Math.Abs(enemy.X - tower.X) < range)
                        {
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

        private void UpdatePeoplesCollisions()
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
                            Game1.MessageBuffer.AddMessage("Bullet kill by player", MessageType.Success);
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
                                Game1.MessageBuffer.AddMessage("Bullet kill by tower", MessageType.Success);
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
                                Game1.MessageBuffer.AddMessage("Bullet kill by soldier", MessageType.Success);
                            }
                        }
                    }
                }
            }

            // enemies and player
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                if (this.player.Hitbox.Intersects(enemy.Hitbox))
                {
                    this.EnemyDie(enemy);
                    Game1.MessageBuffer.AddMessage("Bare hands kill", MessageType.Success);

                    if (!this.player.TakeHit(enemy.Caliber))
                    {
                        this.GameOver();
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
                        if (!enemy.TakeHit(soldier.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Enemy killed by soldier", MessageType.Success);
                            this.EnemyDie(enemy);
                        }
                        if (!soldier.TakeHit(enemy.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Soldier killed by enemy", MessageType.Fail);
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
                        if (!enemy.TakeHit(peasant.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Enemy killed by peasant", MessageType.Success);
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
                        if (!enemy.TakeHit(farmer.Caliber))
                        {
                            Game1.MessageBuffer.AddMessage("Enemy killed by farmer", MessageType.Success);
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

            this.soldiers.RemoveAll(p => p.ToDelete);
            this.enemies.RemoveAll(p => p.ToDelete);
            this.peasants.RemoveAll(p => p.ToDelete);
            this.farmers.RemoveAll(p => p.ToDelete);
        }

        private void UpdateThingsCollisions()
        {
            // coins
            foreach (var coin in this.coins)
            {
                if (this.player.Hitbox.Intersects(coin.Hitbox) && this.player.CanAddMoney())
                {
                    Game1.MessageBuffer.AddMessage("Coin acquired", MessageType.Success);
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

            this.peasants.RemoveAll(p => p.ToDelete);
        }

        private void UpdateActionsCollisions()
        {
            this.player.Action = null;
            this.player.ActionCost = 0;
            this.player.ActionName = null;
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
                    else if (this.center.Status == Building.Status.Built && this.center.Level < MaxCenterLevel)
                    {
                        // center is built - level up?
                        this.player.Action = Enums.PlayerAction.Upgrade;
                        this.player.ActionCost = Center.Cost * (this.center.Level + 1) * 2;
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
                // Arsenal?
                if (buildingSpot.Type == Building.Type.Arsenal)
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
                    Game1.MessageBuffer.AddMessage("Brace yourself", MessageType.Danger);
                    this.dayPhase = DayPhase.Night;
                    this.dayPhaseTimer = (int)DayNightLength.Night;
                }
                else
                {
                    Game1.MessageBuffer.AddMessage("New dawn", MessageType.Success);
                    this.player.Days++;
                    this.dayPhase = DayPhase.Day;
                    this.dayPhaseTimer = (int)DayNightLength.Day;
                }

                Audio.StopSong();
                Audio.CurrentSongCollection = this.dayPhase == DayPhase.Day ? "Day" : "Night";
            }
        }

        private void UpdateHomelesses()
        {
            // create new?
            if (Tools.GetRandom(this.newHomelessProbability) == 1 && this.homelesses.Count < this.homelessLimit)
            {
                Game1.MessageBuffer.AddMessage("New homeless available to hire!", MessageType.Opportunity);
                this.CreateHomeless();
            }

            // update 
            foreach (Homeless homeless in this.homelesses)
            {
                // get nearest slum and szukaj around it
                homeless.DeploymentX = this.slumXs.OrderBy(s => Math.Abs(s - homeless.X)).FirstOrDefault();
                homeless.Update(this.Game.DeltaTime);
            }
        }

    }
}
