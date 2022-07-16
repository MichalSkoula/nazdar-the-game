using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using SiberianAnabasis.Objects;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Screens
{
    public class VillageScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public VillageScreen(Game1 game) : base(game) { }

        private readonly Camera camera = new Camera();

        private readonly Random rand = new Random();

        private readonly FileIO saveFile = new FileIO();

        // will be calculated
        public static int MapWidth = Enums.Screen.Width * 4; // 640 * 4 = 2560px

        // game components
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Soldier> soldiers = new List<Soldier>();
        private List<Building> buildings = new List<Building>();

        // day and night, timer
        private DayPhase dayPhase = DayPhase.Day;
        private double dayPhaseTimer = (int)DayNightLength.Day;

        public override void Initialize()
        {
            // stop whatever menu song is playing 
            MediaPlayer.Stop();

            // create player in the center of the map
            this.player = new Player(MapWidth / 2, Offset.Floor);

            // load objects from tileset
            foreach (var building in Assets.TilesetGroups["village1"].GetObjects("objects", "building"))
            {
                this.buildings.Add(new Building((int)building.x, (int)building.y, (int)building.width, (int)building.height));
            }

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            this.Load();

            // play song
            MediaPlayer.Play(Assets.Map);

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
            this.UpdateCollisions();
            this.UpdateDayPhase();
        }

        private void UpdateEnemies()
        {
            // create enemy?
            if (this.rand.Next(360) < 8 && this.dayPhase == DayPhase.Night)
            {
                // choose direction
                if (this.rand.Next(2) == 0)
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
            // create soldier?
            if (this.rand.Next(360) < 2 && this.dayPhase == DayPhase.Day)
            {
                // choose direction
                if (this.rand.Next(2) == 0)
                {
                    this.soldiers.Add(new Soldier(player.Hitbox.X + player.Hitbox.Width, Offset.Floor, Direction.Right));
                }
                else
                {
                    this.soldiers.Add(new Soldier(player.Hitbox.X - player.Hitbox.Width, Offset.Floor, Direction.Left));
                }
            }

            // update soldiers
            foreach (var soldier in this.soldiers)
            {
                soldier.Update(this.Game.DeltaTime);
            }
        }

        private void UpdateCollisions()
        {
            // enemies
            foreach (Enemy enemy in this.enemies)
            {
                // enemies and bullets
                foreach (Bullet bullet in this.player.Bullets)
                {
                    if (enemy.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!enemy.TakeHit(bullet.Caliber))
                        {
                            enemy.ToDelete = true;
                            Game.MessageBuffer.AddMessage("Bullet kill");
                        }
                    }
                }

                // enemies and player
                if (!enemy.ToDelete && this.player.Hitbox.Intersects(enemy.Hitbox))
                {
                    enemy.ToDelete = true;
                    Game.MessageBuffer.AddMessage("Bare hands kill");

                    if (!this.player.TakeHit(enemy.Caliber))
                    {
                        this.Game.LoadScreen(typeof(Screens.GameOverScreen));
                    }
                }
            }

            this.enemies.RemoveAll(p => p.ToDelete);

            // soldiers
            foreach (var soldier in this.soldiers)
            {
                // enemies and soldiers
                foreach (var enemy in this.enemies)
                {
                    if (enemy.Hitbox.Intersects(soldier.Hitbox))
                    {
                        if (!enemy.TakeHit(soldier.Caliber))
                        {
                            enemy.ToDelete = true;
                        }
                        else if (!soldier.TakeHit(enemy.Caliber))
                        {
                            soldier.ToDelete = true;
                        }
                    }
                }
            }

            this.enemies.RemoveAll(p => p.ToDelete);
            this.soldiers.RemoveAll(p => p.ToDelete);

            // buildings
            this.player.ActiveButton = false;
            foreach (var building in this.buildings)
            {
                if (this.player.Hitbox.Intersects(building.Hitbox))
                {
                    this.player.ActiveButton = true;
                }
            }
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

            // buildings
            foreach (Building building in this.buildings)
            {
                building.Draw(this.Game.SpriteBatch);
            }

            // stats
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "Village " + this.Game.Village.ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 10),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
               Assets.FontSmall,
               "Days " + this.player.Days.ToString(),
               new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 20),
               Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "Timer: " + Math.Ceiling(this.dayPhaseTimer).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 30),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "Health: " + (this.player.Health).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 40),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "Money: " + (this.player.Money).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 50),
                Color.Black);

            // messages
            Game.MessageBuffer.Draw(this.Game.SpriteBatch, this.camera.Transform.Translation.X);

            // player - camera follows
            this.player.Draw(this.Game.SpriteBatch);

            // enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
            }

            // soldiers
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }

        private void Load()
        {
            dynamic saveData = this.saveFile.Load();
            if (saveData == null)
            {
                return;
            }

            // load player
            if (saveData.ContainsKey("player"))
            {
                this.player.Load(saveData.GetValue("player"));
            }

            // load enemies
            if (saveData.ContainsKey("enemies"))
            {
                foreach (var enemy in saveData.GetValue("enemies"))
                {
                    this.enemies.Add(new Enemy((int)enemy.Hitbox.X, (int)enemy.Hitbox.Y, (Direction)enemy.Direction, (int)enemy.Health, (int)enemy.Caliber));
                }
            }

            // load soldiers
            if (saveData.ContainsKey("soldiers"))
            {
                foreach (var soldier in saveData.GetValue("soldiers"))
                {
                    this.soldiers.Add(new Soldier((int)soldier.Hitbox.X, (int)soldier.Hitbox.Y, (Direction)soldier.Direction, (int)soldier.Health, (int)soldier.Caliber));
                }
            }

            // load day phase
            if (saveData.ContainsKey("dayPhase") && saveData.ContainsKey("dayPhaseTimer"))
            {
                this.dayPhase = (DayPhase)saveData.GetValue("dayPhase");
                this.dayPhaseTimer = (double)saveData.GetValue("dayPhaseTimer");
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
                dayPhase = this.dayPhase,
                dayPhaseTimer = this.dayPhaseTimer,
                village = this.Game.Village,
            });
        }
    }
}
