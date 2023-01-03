using MonoGame.Extended.Screens;
using Nazdar.Objects;
using Nazdar.Shared;
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
        public static int MapWidth = Enums.Screen.Width * 4; // 640 * 4 = 2560px

        // game components
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Soldier> soldiers = new List<Soldier>();
        private List<Farmer> farmers = new List<Farmer>();
        private List<Medic> medics = new List<Medic>();
        private List<Homeless> homelesses = new List<Homeless>();
        private List<Peasant> peasants = new List<Peasant>();

        private List<Coin> coins = new List<Coin>();

        private List<BuildingSpot> buildingSpots = new List<BuildingSpot>();
        private List<BuildingSpot> slums = new List<BuildingSpot>();
        private Center center;
        private Locomotive locomotive;
        private List<Armory> armories = new List<Armory>();
        private List<Arsenal> arsenals = new List<Arsenal>();
        private List<Tower> towers = new List<Tower>();
        private List<Farm> farms = new List<Farm>();
        private List<Hospital> hospitals = new List<Hospital>();

        // day and night, timer
        private DayPhase dayPhase = DayPhase.Day;
        private double dayPhaseTimer = (int)DayNightLength.Day;

        // consts
        public readonly int MaxVillage = Assets.TilesetGroups.Count;

        // some settings - random 0-X == 1
        private int newEnemyProbability = 192; // every day, it gets -2
        private int newEnemyProbabilityLowLimit = 16;
        private int newEnemyMaxCaliber = Enemy.DefaultCaliber * 5;
        private int newHomelessProbability = 512 * 3;
        private int newCoinProbability = 512 * 4;
        private int enemyDropProbability = 8;
        private int homelessLimit = 16;
        public static int farmingMoneyProbability = 512 * 3;
        private int farmLimit = 4;

        // X positions for deployments
        private Tower? leftmostTower;
        private Tower? rightmostTower;

        public override void Initialize()
        {
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
            foreach (var other in Assets.TilesetGroups["village1"].GetObjects("objects", "Other"))
            {
                this.slums.Add(
                    new BuildingSpot(
                        (int)other.x,
                        (int)other.y,
                        (int)other.width,
                        (int)other.height,
                        (string)other.name
                    )
                );
            }

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            if (!this.Load())
            {
                // nothing to load - spark some coins and homelesses
                for (int i = 0; i < 16 + Tools.GetRandom(4); i++)
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
                foreach (var data in saveData.GetValue("enemies"))
                {
                    this.enemies.Add(new Enemy((int)data.Hitbox.X, (int)data.Hitbox.Y, (Direction)data.Direction, (int)data.Health, (int)data.Caliber));
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

            if (saveData.ContainsKey("center") && saveData.GetValue("center") != null)
            {
                var data = saveData.GetValue("center");
                this.center = new Center((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (int)data.Level, (float)data.TimeToBuild);
            }

            if (saveData.ContainsKey("locomotive") && saveData.GetValue("locomotive") != null)
            {
                var data = saveData.GetValue("locomotive");
                this.locomotive = new Locomotive((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild);
            }

            if (saveData.ContainsKey("armories"))
            {
                foreach (var data in saveData.GetValue("armories"))
                {
                    this.armories.Add(new Armory((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (int)data.WeaponsCount, (float)data.TimeToBuild));
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

            if (saveData.ContainsKey("towers"))
            {
                foreach (var data in saveData.GetValue("towers"))
                {
                    var newTower = new Tower((int)data.Hitbox.X, (int)data.Hitbox.Y, (Building.Status)data.Status, (float)data.TimeToBuild);
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
                player = this.player.GetSaveData(),
                enemies = this.enemies.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                soldiers = this.soldiers.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                farmers = this.farmers.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                homelesses = this.homelesses.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                peasants = this.peasants.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                medics = this.medics.Where(item => item.Dead == false).Select(item => item.GetSaveData()).ToList(),
                center = this.center?.GetSaveData(),
                locomotive = this.locomotive?.GetSaveData(),
                armories = this.armories.Select(item => item.GetSaveData()).ToList(),
                arsenals = this.arsenals.Select(item => item.GetSaveData()).ToList(),
                hospitals = this.hospitals.Select(item => item.GetSaveData()).ToList(),
                towers = this.towers.Select(item => item.GetSaveData()).ToList(),
                farms = this.farms.Select(item => item.GetSaveData()).ToList(),
                coins = this.coins.Select(item => item.GetSaveData()).ToList(),
                this.dayPhase,
                this.dayPhaseTimer,
                village = this.Game.Village,
            };
        }
    }
}
