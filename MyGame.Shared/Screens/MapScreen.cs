using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MyGame.Enums;

namespace MyGame.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Camera camera = new Camera();

        private Random rand = new Random();

        private FileIO saveFile = new FileIO();

        // how large the map will be
        private int numberOfScreens = 4;

        // will be calculated
        public static int MapWidth;

        // game components
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();

        // day and night, timer
        private DayPhase dayPhase = DayPhase.Day;
        private const int NightLength = 4;
        private const int DayLength = 8;
        private double timer = DayLength;

        public override void Initialize()
        {
            MapWidth = Game1.screenWidth * this.numberOfScreens;

            // stop whatever menu song is playing 
            MediaPlayer.Stop();

            // create player in the center of the map
            this.player = new Player(MapWidth / 2, Offset.Floor);

            // set save slot and maybe load?
            this.saveFile.File = Game.SaveSlot;
            // System.Diagnostics.Debug.WriteLine("save_slot_" + Game.Slot + ".json");
            this.Load();

            // play song
            MediaPlayer.Play(Assets.Map);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // save
                this.Save();

                // back to menu
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            this.player.Update(this.Game.DeltaTime);

            this.camera.Follow(this.player);

            this.EnemiesUpdate();
            this.UpdateDayPhase();
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = this.camera.Transform;
            this.Game.DrawStart();

            // day or night sky
            this.GraphicsDevice.Clear(this.dayPhase == DayPhase.Day ? Color.CornflowerBlue : Color.Black);

            // background
            // this.Game.EffectStart(Assets.Pixelate);
            for (int i = 0; i < this.numberOfScreens; i++)
            {
                this.Game.SpriteBatch.Draw(
                    Assets.Background,
                    new Vector2(i * Game1.screenWidth, 0),
                    Color.White);
            }
            // this.Game.EffectEnd();

            // left tunnel
            this.Game.SpriteBatch.Draw(Assets.Tunnel, Vector2.Zero, Color.White);

            // right tunnel - all this for a flip
            this.Game.SpriteBatch.Draw(
                Assets.Tunnel,
                new Rectangle((Game1.screenWidth * this.numberOfScreens) - Assets.Tunnel.Width, 0, Assets.Tunnel.Width, Assets.Tunnel.Height),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipHorizontally,
                0);

            // status bar (minus translation to stay in place)
            this.Game.SpriteBatch.DrawString(
                Assets.FontMedium,
                "this is game you can escape to menu",
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar),
                Color.Black);

            // timer
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "timer: " + Math.Ceiling(this.timer).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 20),
                Color.Black);

            // player stats
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "health: " + (this.player.Health).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 30),
                Color.Black);
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "money: " + (this.player.Money).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, Offset.StatusBar + 40),
                Color.Black);

            // player - camera follows
            this.player.Draw(this.Game.SpriteBatch);

            // enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
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
        }

        private void Save()
        {
            this.saveFile.Save(new
                {
                    player = this.player,
                    enemies = this.enemies,
                }
            );
        }

        private void EnemiesUpdate()
        {
            // create enemy?
            if (this.rand.Next(120) < 3 && this.dayPhase == DayPhase.Night)
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

                // enemies and bullets
                foreach (Bullet bullet in this.player.Bullets)
                {
                    if (enemy.Hitbox.Intersects(bullet.Hitbox))
                    {
                        bullet.ToDelete = true;

                        if (!enemy.TakeHit(bullet.Caliber))
                        {
                            enemy.ToDelete = true;
                        }
                    }
                }

                // enemies and player
                if (this.player.Hitbox.Intersects(enemy.Hitbox))
                {
                    enemy.ToDelete = true;

                    if (!this.player.TakeHit(enemy.Caliber))
                    {
                        this.Game.LoadScreen(typeof(Screens.GameOverScreen));
                    }
                }
            }

            this.enemies.RemoveAll(p => p.ToDelete);
        }

        private void UpdateDayPhase()
        {
            this.timer -= this.Game.DeltaTime;
            if (this.timer <= 0)
            {
                if (this.dayPhase == DayPhase.Day)
                {
                    this.dayPhase = DayPhase.Night;
                    this.timer = NightLength;
                }
                else
                {
                    this.dayPhase = DayPhase.Day;
                    this.timer = DayLength;
                }
            }
        }
    }
}
