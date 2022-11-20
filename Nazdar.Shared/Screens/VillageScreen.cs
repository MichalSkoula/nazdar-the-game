using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public partial class VillageScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public VillageScreen(Game1 game) : base(game) { }

        private readonly Camera camera = new Camera();

        private readonly FileIO saveFile = new FileIO();

        // will be calculated
        public static int MapWidth = Enums.Screen.Width * 4; // 640 * 4 = 2560px

        // game components
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Soldier> soldiers = new List<Soldier>();
        private List<Farmer> farmers = new List<Farmer>();
        private List<Homeless> homelesses = new List<Homeless>();
        private List<Peasant> peasants = new List<Peasant>();

        private List<Coin> coins = new List<Coin>();

        private List<BuildingSpot> buildingSpots = new List<BuildingSpot>();
        private Center center;
        private List<Armory> armories = new List<Armory>();
        private List<Arsenal> arsenals = new List<Arsenal>();
        private List<Tower> towers = new List<Tower>();
        private List<Farm> farms = new List<Farm>();

        // day and night, timer
        private DayPhase dayPhase = DayPhase.Day;
        private double dayPhaseTimer = (int)DayNightLength.Day;

        // some settings - random 0-X == 1
        private int newEnemyProbability = 128; // every day, it gets -2
        private int newEnemyProbabilityLowLimit = 16;
        private int newEnemyMaxCaliber = Enemy.DefaultCaliber * 5;
        private int newHomelessProbability = 512 * 3;
        private int newCoinProbability = 512 * 6;
        private int enemyDropProbability = 8;
        private int homelessLimit = 16;
        private int farmingMoneyProbability = 512 * 3;
        private int farmLimit = 4;
        private int centerMaxLevel = 5;

        // X positions for deployments
        private int? leftmostTowerX = null;
        private int? rightmostTowerX = null;
        private List<int> slumXs = new List<int>();

        public override void Initialize()
        {
            // create player in the center of the map
            this.player = new Player(MapWidth / 2, Offset.Floor);

            // load building spots from tileset
            foreach (var buildingSpot in Assets.TilesetGroups["village1"].GetObjects("objects", "BuildingSpot"))
            {
                this.buildingSpots.Add(
                    new BuildingSpot(
                        (int)buildingSpot.x,
                        (int)buildingSpot.y,
                        (int)buildingSpot.width,
                        (int)buildingSpot.height,
                        buildingSpot.name
                    )
                );
            }

            // load other object from tileset
            foreach (var other in Assets.TilesetGroups["village1"].GetObjects("objects", "Other"))
            {
                if (other.name == "Slum")
                {
                    slumXs.Add((int)other.x);
                }
            }

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            if (!this.Load())
            {
                // nothing to load - spark some coins and homelesses
                for (int i = 0; i < 12 + Tools.GetRandom(4); i++)
                {
                    this.coins.Add(new Coin(Tools.GetRandom(VillageScreen.MapWidth), Offset.Floor2));
                }
                for (int i = 0; i < 2 + Tools.GetRandom(2); i++)
                {
                    this.CreateHomeless();
                }
            }

            // play songs
            Audio.StopSong();
            Audio.CurrentSongCollection = this.dayPhase == DayPhase.Day ? "Day" : "Night";

            base.Initialize();
        }

        private bool Load()
        {
            dynamic saveData = this.saveFile.Load();
            if (saveData == null)
            {
                return false;
            }

            if (saveData.ContainsKey("player"))
            {
                this.player.Load(saveData.GetValue("player"));
            }

            if (saveData.ContainsKey("enemies"))
            {
                foreach (var enemy in saveData.GetValue("enemies"))
                {
                    if ((bool)enemy.Dead)
                    {
                        continue;
                    }
                    this.enemies.Add(new Enemy((int)enemy.Hitbox.X, (int)enemy.Hitbox.Y, (Direction)enemy.Direction, (int)enemy.Health, (int)enemy.Caliber));
                }
            }

            if (saveData.ContainsKey("soldiers"))
            {
                foreach (var soldier in saveData.GetValue("soldiers"))
                {
                    if ((bool)soldier.Dead)
                    {
                        continue;
                    }

                    var newSoldier = new Soldier((int)soldier.Hitbox.X, (int)soldier.Hitbox.Y, (Direction)soldier.Direction, (int)soldier.Health, (int)soldier.Caliber);
                    if (soldier.ContainsKey("Bullets"))
                    {
                        foreach (var bullet in soldier.GetValue("Bullets"))
                        {
                            newSoldier.Bullets.Add(new Bullet((int)bullet.Hitbox.X, (int)bullet.Hitbox.Y, (Direction)bullet.Direction, (int)bullet.Caliber));
                        }
                    }
                    this.soldiers.Add(newSoldier);
                }
            }

            if (saveData.ContainsKey("homelesses"))
            {
                foreach (var homeless in saveData.GetValue("homelesses"))
                {
                    this.homelesses.Add(new Homeless((int)homeless.Hitbox.X, (int)homeless.Hitbox.Y, (Direction)homeless.Direction));
                }
            }

            if (saveData.ContainsKey("peasants"))
            {
                foreach (var peasant in saveData.GetValue("peasants"))
                {
                    this.peasants.Add(new Peasant((int)peasant.Hitbox.X, (int)peasant.Hitbox.Y, (Direction)peasant.Direction, (int)peasant.Health, (int)peasant.Caliber));
                }
            }

            if (saveData.ContainsKey("farmers"))
            {
                foreach (var farmer in saveData.GetValue("farmers"))
                {
                    this.farmers.Add(new Farmer((int)farmer.Hitbox.X, (int)farmer.Hitbox.Y, (Direction)farmer.Direction, (int)farmer.Health, (int)farmer.Caliber));
                }
            }

            if (saveData.ContainsKey("coins"))
            {
                foreach (var coin in saveData.GetValue("coins"))
                {
                    this.coins.Add(new Coin((int)coin.Hitbox.X, (int)coin.Hitbox.Y));
                }
            }

            if (saveData.ContainsKey("dayPhase") && saveData.ContainsKey("dayPhaseTimer"))
            {
                this.dayPhase = (DayPhase)saveData.GetValue("dayPhase");
                this.dayPhaseTimer = (double)saveData.GetValue("dayPhaseTimer");
            }

            if (saveData.ContainsKey("center") && saveData.GetValue("center") != null)
            {
                var center = saveData.GetValue("center");
                this.center = new Center((int)center.X, (int)center.Y, (Building.Status)center.Status, (int)center.Level);
            }

            if (saveData.ContainsKey("armories"))
            {
                foreach (var armory in saveData.GetValue("armories"))
                {
                    this.armories.Add(new Armory((int)armory.Hitbox.X, (int)armory.Hitbox.Y, (Building.Status)armory.Status, (int)armory.WeaponsCount));
                }
            }

            if (saveData.ContainsKey("arsenals"))
            {
                foreach (var arsenal in saveData.GetValue("arsenals"))
                {
                    this.arsenals.Add(new Arsenal((int)arsenal.Hitbox.X, (int)arsenal.Hitbox.Y, (Building.Status)arsenal.Status));
                }
            }

            if (saveData.ContainsKey("farms"))
            {
                foreach (var farm in saveData.GetValue("farms"))
                {
                    this.farms.Add(new Farm((int)farm.Hitbox.X, (int)farm.Hitbox.Y, (Building.Status)farm.Status, (int)farm.ToolsCount));
                }
            }

            if (saveData.ContainsKey("towers"))
            {
                foreach (var tower in saveData.GetValue("towers"))
                {
                    var newTower = new Tower((int)tower.Hitbox.X, (int)tower.Hitbox.Y, (Building.Status)tower.Status);
                    if (tower.ContainsKey("Bullets"))
                    {
                        foreach (var bullet in tower.GetValue("Bullets"))
                        {
                            newTower.Bullets.Add(new Bullet((int)bullet.Hitbox.X, (int)bullet.Hitbox.Y, (Direction)bullet.Direction, (int)bullet.Caliber, BulletType.Cannonball));
                        }
                    }
                    this.towers.Add(newTower);
                }
            }

            Game1.MessageBuffer.AddMessage("Game loaded", MessageType.Info);

            return true;
        }

        private dynamic GetSaveData()
        {
            return new
            {
                player = this.player,
                enemies = this.enemies,
                soldiers = this.soldiers,
                farmers = this.farmers,
                homelesses = this.homelesses,
                peasants = this.peasants,
                center = this.center,
                armories = this.armories,
                arsenals = this.arsenals,
                towers = this.towers,
                farms = this.farms,
                coins = this.coins,
                dayPhase = this.dayPhase,
                dayPhaseTimer = this.dayPhaseTimer,
                village = this.Game.Village,
            };
        }
    }
}
