using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
using Nazdar.Shared.Parallax;
using Nazdar.Weather;
using System.Collections.Generic;
using System.Linq;
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
        public static int MapWidth;

        // game components
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Pig> pigs = new List<Pig>();
        private List<Lenin> lenins = new List<Lenin>();
        private List<Soldier> soldiers = new List<Soldier>();
        private List<Farmer> farmers = new List<Farmer>();
        private List<Medic> medics = new List<Medic>();
        private List<Homeless> homelesses = new List<Homeless>();
        private List<Peasant> peasants = new List<Peasant>();

        private List<Coin> coins = new List<Coin>();

        private List<BuildingSpot> buildingSpots = new List<BuildingSpot>();
        private List<BuildingSpot> slums = new List<BuildingSpot>();
        private Ship ship;
        private Treasure treasure;
        private Center center;
        private Locomotive locomotive;
        private List<Armory> armories = new List<Armory>();
        private List<Arsenal> arsenals = new List<Arsenal>();
        private List<Tower> towers = new List<Tower>();
        private List<Farm> farms = new List<Farm>();
        private List<Hospital> hospitals = new List<Hospital>();
        private List<Market> markets = new List<Market>();
        private List<Rails> rails = new List<Rails>();

        private DayPhase dayPhase = DayPhase.Day;
        private double dayPhaseTimer = (int)DayNightLength.Day;
        private bool won = false;
        private Sky sky = new Sky();

        private ParallaxManager parallaxManager = new ParallaxManager();

        private Disease disease;

        // some settings - random 0-X == 1 ----------------------------------------------
        // new enemy settings
        // every day, it gets -2
        // every village, it gets -20
        private static int newEnemyDefaultProbability = 260;
        private int newEnemyProbabilityLowLimit = 16;
        private static int newHomelessDefaultProbability = 512 * 4;
        private int newHomelessProbabilityLowLimit = 768;
        private int newEnemyMaxCaliber = Enemy.DefaultCaliber * 5;
        private int newPigMaxCaliber = Pig.DefaultCaliber * 4;
        private int newLeninMaxCaliber = Lenin.DefaultCaliber * 4;
        private int newCoinProbability = 512 * 3;
        private int enemyDropProbability = 4;
        private int pigDropProbability = 4;
        private int leninDropProbability = 2;
        private int homelessLimit = 16;
        public static int farmingMoneyProbability = 512 * 3;
        public static int marketMoneyProbability = 512 * 2;
        private int newSkyApocalypseProbability = 512 * 6;
        private int diseaseProbability = 512 * 2;
        private readonly int farmLimit = 4;

        // X positions for deployments
        private Tower? leftmostTower;
        private Tower? rightmostTower;

        // lists for easy access
        private List<BasePerson> allEnemies
        {
            get
            {
                var list = (from x in this.pigs select (BasePerson)x).ToList();
                list.AddRange((from x in this.enemies select (BasePerson)x).ToList());
                list.AddRange((from x in this.lenins select (BasePerson)x).ToList());
                return list;
            }
        }

        private List<BasePerson> allLegionnaires
        {
            get
            {
                var list = (from x in this.peasants select (BasePerson)x).ToList();
                list.AddRange((from x in this.farmers select (BasePerson)x).ToList());
                list.AddRange((from x in this.soldiers select (BasePerson)x).ToList());
                list.AddRange((from x in this.medics select (BasePerson)x).ToList());
                return list;
            }
        }

        public override void Initialize()
        {
            MapWidth = Assets.TilesetGroups["village" + this.Game.Village].GetTilesetMapWidth();

            // create player in the center of the map
            this.player = new Player(MapWidth / 2, Offset.Floor);

            // load building spots from tileset
            foreach (var buildingSpot in Assets.TilesetGroups["village" + this.Game.Village].GetObjects("objects", "BuildingSpot"))
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
            foreach (var other in Assets.TilesetGroups["village" + this.Game.Village].GetObjects("objects", "Other"))
            {
                this.slums.Add(
                    new BuildingSpot(
                        (int)other.x,
                        (int)other.y,
                        (int)other.width,
                        (int)other.height,
                        other.name
                    )
                );
            }

            // load special buildings
            var otherShip = Assets.TilesetEdges["right" + this.Game.Village].GetObject("objects", "Special", "Ship");
            if (otherShip != null)
            {
                this.ship = new Ship(
                    (int)otherShip.x + MapWidth,
                    (int)otherShip.y
                );
            }
            var otherTreasure = Assets.TilesetGroups["village" + this.Game.Village].GetObject("objects", "Special", "Treasure");
            if (otherTreasure != null)
            {
                this.treasure = new Treasure(
                    (int)otherTreasure.x,
                    (int)otherTreasure.y
                );
            }

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            if (!this.Load() || this.Game.FirstRun)
            {
                // increase probability
                int probability = this.Game.Village;
                // @survivalMode
                if (this.Game.Village == 0)
                {
                    probability = 16;
                }

                // spark some coins and homelesses
                for (int i = 0; i < 12 + probability + Tools.GetRandom(6); i++)
                {
                    this.coins.Add(new Coin(Tools.GetRandom(VillageScreen.MapWidth), Offset.Floor2));
                }
                for (int i = 0; i < 2 + probability + Tools.GetRandom(3); i++)
                {
                    this.CreateHomeless();
                }
                this.Game.FirstRun = false;
            }

            Audio.SongTransition(0.5f, this.dayPhase == DayPhase.Day ? "Day" : "Night");

            // add parallax layers
            parallaxManager.Init(MapWidth, this.Game.Village);

            // maybe add some disease hahaha
            this.disease = new Disease(this.Game.Village);

            base.Initialize();
        }

        private bool Load()
        {
            dynamic saveData = this.saveFile.Load();
            if (saveData == null)
            {
                return false;
            }

            if (saveData.ContainsKey("village"))
            {
                this.Game.Village = (int)saveData.village;
            }

            Game1.CenterLevel = 1;
            if (saveData.ContainsKey("centerLevel"))
            {
                Game1.CenterLevel = (int)saveData.centerLevel;
            }

            Game1.TowersLevel = 1;
            if (saveData.ContainsKey("towersLevel"))
            {
                Game1.TowersLevel = (int)saveData.towersLevel;
            }

            if (saveData.ContainsKey("firstRun"))
            {
                this.Game.FirstRun = (bool)saveData.firstRun;
            }

            if (saveData.ContainsKey("player"))
            {
                this.player.Load(saveData.GetValue("player"));
            }

            if (saveData.ContainsKey("enemies"))
            {
                foreach (var data in saveData.GetValue("enemies"))
                {
                    this.enemies.Add(new Enemy((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber, this.Game.Village));
                }
            }

            if (saveData.ContainsKey("pigs"))
            {
                foreach (var data in saveData.GetValue("pigs"))
                {
                    this.pigs.Add(new Pig((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
                }
            }

            if (saveData.ContainsKey("lenins"))
            {
                foreach (var data in saveData.GetValue("lenins"))
                {
                    this.lenins.Add(new Lenin((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
                }
            }

            if (saveData.ContainsKey("soldiers"))
            {
                foreach (var data in saveData.GetValue("soldiers"))
                {
                    var newSoldier = new Soldier((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber);
                    foreach (var bulletData in data.GetValue("Bullets"))
                    {
                        newSoldier.Bullets.Add(new Bullet((int)bulletData.Hitbox.X, (int)bulletData.Hitbox.Y, (Direction)bulletData.Direction, (int)bulletData.Caliber));
                    }
                    this.soldiers.Add(newSoldier);
                }
            }

            if (saveData.ContainsKey("homelesses"))
            {
                foreach (var data in saveData.GetValue("homelesses"))
                {
                    this.homelesses.Add(new Homeless((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction));
                }
            }

            if (saveData.ContainsKey("peasants"))
            {
                foreach (var data in saveData.GetValue("peasants"))
                {
                    this.peasants.Add(new Peasant((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
                }
            }

            if (saveData.ContainsKey("farmers"))
            {
                foreach (var data in saveData.GetValue("farmers"))
                {
                    this.farmers.Add(new Farmer((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
                }
            }

            if (saveData.ContainsKey("medics"))
            {
                foreach (var data in saveData.GetValue("medics"))
                {
                    this.medics.Add(new Medic((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
                }
            }

            if (saveData.ContainsKey("coins"))
            {
                foreach (var data in saveData.GetValue("coins"))
                {
                    this.coins.Add(new Coin((int)data.Hitbox.X, (int)data.Hitbox.Y));
                }
            }

            if (saveData.ContainsKey("dayPhase") && saveData.ContainsKey("dayPhaseTimer"))
            {
                this.dayPhase = (DayPhase)saveData.GetValue("dayPhase");
                this.dayPhaseTimer = (double)saveData.GetValue("dayPhaseTimer");
            }

            if (saveData.ContainsKey("skyDropType") && saveData.ContainsKey("skyTtl") && (int)saveData.GetValue("skyTtl") > 0)
            {
                sky.Start(
                    (int)saveData.GetValue("skyTtl"),
                    (DropType)saveData.GetValue("skyDropType"),
                    saveData.ContainsKey("skyDropCount") ? (int)saveData.GetValue("skyDropCount") : 0
                );
            }

            if (saveData.ContainsKey("center") && saveData.GetValue("center") != null)
            {
                var data = saveData.GetValue("center");
                this.center = new Center((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild, (bool)data.HasBeenUpgradedToday);
            }

            if (saveData.ContainsKey("locomotive") && saveData.GetValue("locomotive") != null)
            {
                var data = saveData.GetValue("locomotive");
                this.locomotive = new Locomotive((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild);
            }

            if (saveData.ContainsKey("treasure") && saveData.GetValue("treasure") != null)
            {
                var data = saveData.GetValue("treasure");
                this.treasure = new Treasure((int)data.Hitbox.X, (int)data.Hitbox.Y, (int)data.Health);
            }

            if (saveData.ContainsKey("armories"))
            {
                foreach (var data in saveData.GetValue("armories"))
                {
                    this.armories.Add(new Armory((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (int)data.WeaponsCount, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("rails"))
            {
                foreach (var data in saveData.GetValue("rails"))
                {
                    this.rails.Add(new Rails((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("arsenals"))
            {
                foreach (var data in saveData.GetValue("arsenals"))
                {
                    this.arsenals.Add(new Arsenal((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("farms"))
            {
                foreach (var data in saveData.GetValue("farms"))
                {
                    this.farms.Add(new Farm((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (int)data.ToolsCount, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("hospitals"))
            {
                foreach (var data in saveData.GetValue("hospitals"))
                {
                    this.hospitals.Add(new Hospital((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (int)data.MedicalKitsCount, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("markets"))
            {
                foreach (var data in saveData.GetValue("markets"))
                {
                    this.markets.Add(new Market((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild));
                }
            }

            if (saveData.ContainsKey("towers"))
            {
                foreach (var data in saveData.GetValue("towers"))
                {
                    var newTower = new Tower((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild, (int)data.Caliber);
                    foreach (var bulletData in data.GetValue("Bullets"))
                    {
                        newTower.Bullets.Add(new Bullet((int)bulletData.Hitbox.X, (int)bulletData.Hitbox.Y, (Direction)bulletData.Direction, (int)bulletData.Caliber, BulletType.Cannonball));
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
                this.dayPhase,
                this.dayPhaseTimer,
                village = this.Game.Village,
                centerLevel = Game1.CenterLevel,
                towersLevel = Game1.TowersLevel,
                firstRun = this.Game.FirstRun,
                skyTtl = this.sky.Ttl,
                skyDropType = this.sky.Type,
                skyDropCount = this.sky.DropCount,
                player = this.player.GetSaveData(),
                enemies = this.enemies.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                pigs = this.pigs.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                lenins = this.lenins.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                soldiers = this.soldiers.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                farmers = this.farmers.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                homelesses = this.homelesses.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                peasants = this.peasants.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                medics = this.medics.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                center = this.center?.GetSaveData(),
                locomotive = this.locomotive?.GetSaveData(),
                treasure = this.treasure?.GetSaveData(),
                armories = this.armories.Select(item => item.GetSaveData()).ToList(),
                arsenals = this.arsenals.Select(item => item.GetSaveData()).ToList(),
                hospitals = this.hospitals.Select(item => item.GetSaveData()).ToList(),
                markets = this.markets.Select(item => item.GetSaveData()).ToList(),
                towers = this.towers.Select(item => item.GetSaveData()).ToList(),
                farms = this.farms.Select(item => item.GetSaveData()).ToList(),
                rails = this.rails.Select(item => item.GetSaveData()).ToList(),
                coins = this.coins.Select(item => item.GetSaveData()).ToList(),
            };
        }
    }
}
