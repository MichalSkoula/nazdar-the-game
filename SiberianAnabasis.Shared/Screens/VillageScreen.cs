using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using SiberianAnabasis.Objects;
using SiberianAnabasis.Shared;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Screens
{
    public class VillageScreen : GameScreen
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
        private List<Homeless> homelesses = new List<Homeless>();
        private List<Peasant> peasants = new List<Peasant>();

        private List<Coin> coins = new List<Coin>();

        private List<BuildingSpot> buildingSpots = new List<BuildingSpot>();
        private Basecamp basecamp;
        private List<Armory> armories = new List<Armory>();

        // day and night, timer
        private DayPhase dayPhase = DayPhase.Day;
        private double dayPhaseTimer = (int)DayNightLength.Day;

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
                        (string)buildingSpot.type
                    )
                );
            }

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            this.Load();

            // play songs
            Audio.StopSong();
            Audio.CurrentSongCollection = this.dayPhase == DayPhase.Day ? "Day" : "Night";

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.Start))
            {
                // save
                this.Save();

                // back to menu
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            this.player.Update(this.Game.DeltaTime);
            this.camera.Follow(this.player);

            this.UpdateEnemies();
            this.UpdateSoldiers();
            this.UpdatePeasants();
            this.UpdateHomelesses();
            this.UpdateCoins();
            this.UpdateCollisions();
            this.UpdateDayPhase();

            if (this.basecamp != null)
            {
                this.basecamp.Update(this.Game.DeltaTime);
            }

            this.UpdateArmories();
        }

        private void UpdateEnemies()
        {
            // create enemy?
            // at night AND in first half on night AND random
            if (this.dayPhase == DayPhase.Night && this.dayPhaseTimer > (int)Enums.DayNightLength.Night / 2 && Tools.GetRandom(2560000) < 8)
            {
                Audio.PlayRandomSound("EnemySpawns");

                // choose direction
                if (Tools.GetRandom(2) == 0)
                {
                    this.enemies.Add(new Enemy(0, Offset.Floor, Direction.Right));
                }
                else
                {
                    this.enemies.Add(new Enemy(MapWidth, Offset.Floor, Direction.Left));
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
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Update(this.Game.DeltaTime);
            }
        }

        private void UpdatePeasants()
        {
            foreach (Peasant peasant in this.peasants)
            {
                peasant.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateHomelesses()
        {
            // create new?
            if (Tools.GetRandom(2000) < 2)
            {
                // choose side 
                if (Tools.GetRandom(2) == 0)
                {
                    this.homelesses.Add(new Homeless(0, Offset.Floor, Direction.Right));
                }
                else
                {
                    this.homelesses.Add(new Homeless(MapWidth, Offset.Floor, Direction.Left));
                }
            }

            // update 
            foreach (Homeless homeless in this.homelesses)
            {
                homeless.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateCoins()
        {
            // create new?
            if (Tools.GetRandom(1000) < 2)
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

        private void UpdateCollisions()
        {
            // enemies and bullets
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (Bullet bullet in this.player.Bullets)
                {
                    if (enemy.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!enemy.TakeHit(bullet.Caliber))
                        {
                            enemy.Dead = true;
                            Audio.PlayRandomSound("EnemyDeaths");
                            Game.MessageBuffer.AddMessage("Bullet kill");
                        }
                    }
                }
            }
            this.enemies.RemoveAll(p => p.ToDelete);

            // enemies and player
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                if (!enemy.ToDelete && this.player.Hitbox.Intersects(enemy.Hitbox))
                {
                    enemy.Dead = true;
                    Audio.PlayRandomSound("EnemyDeaths");
                    Game.MessageBuffer.AddMessage("Bare hands kill");

                    if (!this.player.TakeHit(enemy.Caliber))
                    {
                        this.Game.LoadScreen(typeof(Screens.GameOverScreen));
                    }
                }
            }
            this.enemies.RemoveAll(p => p.ToDelete);

            // enemies and soldiers
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (var soldier in this.soldiers.Where(soldier => soldier.Dead == false))
                {
                    if (enemy.Hitbox.Intersects(soldier.Hitbox))
                    {
                        if (!enemy.TakeHit(soldier.Caliber))
                        {
                            Audio.PlayRandomSound("EnemyDeaths");
                            enemy.Dead = true;
                        }
                        else if (!soldier.TakeHit(enemy.Caliber))
                        {
                            Audio.PlayRandomSound("SoldierDeaths");
                            soldier.Dead = true;
                        }
                    }
                }
                this.soldiers.RemoveAll(p => p.ToDelete);
            }
            this.enemies.RemoveAll(p => p.ToDelete);

            // enemies and peasants
            foreach (Enemy enemy in this.enemies.Where(enemy => enemy.Dead == false))
            {
                foreach (var peasant in this.peasants.Where(peasant => peasant.Dead == false))
                {
                    if (enemy.Hitbox.Intersects(peasant.Hitbox))
                    {
                        peasant.ToDelete = true;
                        this.homelesses.Add(new Homeless(peasant.Hitbox.X, Offset.Floor, peasant.Direction, peasant.Health));
                    }
                }
                this.peasants.RemoveAll(p => p.ToDelete);
            }

            // coins
            foreach (var coin in this.coins)
            {
                if (this.player.Hitbox.Intersects(coin.Hitbox))
                {
                    Audio.PlaySound("Coin");
                    this.player.Money++;
                    coin.ToDelete = true;
                    break;
                }
            }
            this.coins.RemoveAll(p => p.ToDelete);

            // buildingSpots - can we build something?
            this.player.Action = null;
            foreach (var buildingSpot in this.buildingSpots)
            {
                if (!this.player.Hitbox.Intersects(buildingSpot.Hitbox))
                {
                    continue;
                }

                // Basecamp?
                if (buildingSpot.Type == Building.Type.Basecamp && this.basecamp == null)
                {
                    this.player.Action = Enums.PlayerAction.Build;

                    if (Controls.Keyboard.HasBeenPressed(Keys.LeftControl) && this.player.Money >= buildingSpot.Cost)
                    {
                        Audio.PlaySound("SoldierSpawn");
                        this.player.Money -= buildingSpot.Cost;
                        this.basecamp = new Basecamp(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess);
                    }
                }
                // Armory?
                else if (buildingSpot.Type == Building.Type.Armory && this.armories.Where(a => a.Hitbox.Intersects(buildingSpot.Hitbox)).Count() == 0)
                {                    
                    this.player.Action = Enums.PlayerAction.Build;

                    if (Controls.Keyboard.HasBeenPressed(Keys.LeftControl) && this.player.Money >= buildingSpot.Cost)
                    {
                        Audio.PlaySound("SoldierSpawn");
                        this.player.Money -= buildingSpot.Cost;
                        this.armories.Add(new Armory(buildingSpot.X, buildingSpot.Y, Building.Status.InProcess));
                    }
                }
            }

            // homelesses
            foreach (var homeless in this.homelesses)
            {
                if (this.player.Hitbox.Intersects(homeless.Hitbox))
                {
                    this.player.Action = Enums.PlayerAction.Hire;

                    // hire homeless man? create peasant
                    if (Controls.Keyboard.HasBeenPressed(Keys.LeftControl) && this.player.Money >= homeless.Cost)
                    {
                        Audio.PlaySound("SoldierSpawn");
                        homeless.ToDelete = true;
                        this.player.Money -= homeless.Cost;
                        this.peasants.Add(new Peasant(homeless.Hitbox.X, Offset.Floor, homeless.Direction, homeless.Health));
                    }
                    break;
                }
            }
            this.homelesses.RemoveAll(p => p.ToDelete);
        }

        private void UpdateDayPhase()
        {
            this.dayPhaseTimer -= this.Game.DeltaTime;
            if (this.dayPhaseTimer <= 0)
            {
                
                if (this.dayPhase == DayPhase.Day)
                {
                    this.dayPhase = DayPhase.Night;
                    this.dayPhaseTimer = (int)DayNightLength.Night;
                }
                else
                {
                    this.player.Days++;
                    this.dayPhase = DayPhase.Day;
                    this.dayPhaseTimer = (int)DayNightLength.Day;
                }

                Audio.StopSong();
                Audio.CurrentSongCollection = this.dayPhase == DayPhase.Day ? "Day" : "Night";
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = this.camera.Transform;
            this.Game.DrawStart();

            // day or night sky
            this.GraphicsDevice.Clear(this.dayPhase == DayPhase.Day ? Color.CornflowerBlue : Color.DarkBlue);

            // background - tileset
            Assets.TilesetGroups["village1"].Draw("ground", this.Game.SpriteBatch);

            // stats
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Village " + this.Game.Village.ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 10),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
               Assets.Fonts["Small"],
               "Days " + this.player.Days.ToString(),
               new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 20),
               Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Timer: " + Math.Ceiling(this.dayPhaseTimer).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 30),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Health: " + (this.player.Health).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 40),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.Fonts["Small"],
                "Money: " + (this.player.Money).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 50),
                Color.Black);

            // messages
            Game.MessageBuffer.Draw(this.Game.SpriteBatch, this.camera.Transform.Translation.X);

            // game objects
            foreach (BuildingSpot buildingSpot in this.buildingSpots)
            {
                buildingSpot.Draw(this.Game.SpriteBatch);
            }

            if (this.basecamp != null)
            {
                this.basecamp.Draw(this.Game.SpriteBatch);
            }

            foreach (Armory armory in this.armories)
            {
                armory.Draw(this.Game.SpriteBatch);
            }

            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
            }

            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Draw(this.Game.SpriteBatch);
            }

            foreach (Homeless homeless in this.homelesses)
            {
                homeless.Draw(this.Game.SpriteBatch);
            }

            foreach (Peasant peasant in this.peasants)
            {
                peasant.Draw(this.Game.SpriteBatch);
            }

            foreach (Coin coin in this.coins)
            {
                coin.Draw(this.Game.SpriteBatch);
            }

            // player - camera follows him
            this.player.Draw(this.Game.SpriteBatch);

            this.Game.DrawEnd();
        }

        private void Load()
        {
            dynamic saveData = this.saveFile.Load();
            if (saveData == null)
            {
                return;
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
                    this.soldiers.Add(new Soldier((int)soldier.Hitbox.X, (int)soldier.Hitbox.Y, (Direction)soldier.Direction, (int)soldier.Health, (int)soldier.Caliber));
                }
            }

            if (saveData.ContainsKey("homelesses"))
            {
                foreach (var homeless in saveData.GetValue("homelesses"))
                {
                    this.homelesses.Add(new Homeless((int)homeless.Hitbox.X, (int)homeless.Hitbox.Y, (Direction)homeless.Direction, (int)homeless.Health));
                }
            }

            if (saveData.ContainsKey("peasants"))
            {
                foreach (var peasant in saveData.GetValue("peasants"))
                {
                    this.peasants.Add(new Peasant((int)peasant.Hitbox.X, (int)peasant.Hitbox.Y, (Direction)peasant.Direction, (int)peasant.Health));
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

            if (saveData.ContainsKey("basecamp") && saveData.GetValue("basecamp") != null)
            {
                this.basecamp = new Basecamp((int)saveData.GetValue("basecamp").X, (int)saveData.GetValue("basecamp").Y, (Building.Status)saveData.GetValue("basecamp").Status);
            }

            if (saveData.ContainsKey("armories"))
            {
                foreach (var armory in saveData.GetValue("armories"))
                {
                    this.armories.Add(new Armory((int)armory.Hitbox.X, (int)armory.Hitbox.Y, (Building.Status)armory.Status));
                }
            }

            Game.MessageBuffer.AddMessage("Game loaded");
        }

        private void Save()
        {
            this.saveFile.Save(new
            {
                player = this.player,
                enemies = this.enemies,
                soldiers = this.soldiers,
                homelesses = this.homelesses,
                peasants = this.peasants,
                basecamp = this.basecamp,
                armories = this.armories,
                coins = this.coins,
                dayPhase = this.dayPhase,
                dayPhaseTimer = this.dayPhaseTimer,
                village = this.Game.Village,
            });
        }
    }
}
