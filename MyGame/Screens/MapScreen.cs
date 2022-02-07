namespace MyGame.Screens
{
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

    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private Camera camera = new Camera();

        private Random rand = new Random();

        // how large the map will be
        private int numberOfScreens = 4;

        // will be calculated
        public static int MapWidth;

        // Y positions
        private const int StatusBarPosition = 300;
        public const int FloorPos = 250;

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
            this.player = new Player(MapWidth / 2, FloorPos);

            // maybe load?
            this.Load();

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
                new Vector2(10 - this.camera.Transform.Translation.X, StatusBarPosition),
                Color.White);

            // timer
            this.Game.SpriteBatch.DrawString(
                Assets.FontSmall,
                "timer: " + Math.Ceiling(this.timer).ToString(),
                new Vector2(10 - this.camera.Transform.Translation.X, StatusBarPosition + 20),
                Color.White);

            // player - camera follows
            this.player.Draw(this.Game.SpriteBatch);

            // enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }

        public void Load()
        {
            if (!File.Exists("save.json"))
            {
                return;
            }

            // parse
            string json = File.ReadAllText("save.json");
            dynamic saveData = JObject.Parse(json);

            // create objects
            if (saveData.ContainsKey("player"))
            {
                this.player.Load(saveData.GetValue("player"));
            }

            if (saveData.ContainsKey("enemies"))
            {
                foreach (var enemy in saveData.GetValue("enemies"))
                {
                    this.enemies.Add(new Enemy((int)enemy.Hitbox.X, (int)enemy.Hitbox.Y, (Enums.Direction)enemy.Direction, (int)enemy.Health));
                }
            }
        }

        private void Save()
        {
            var saveData = new
            {
                player = this.player,
                enemies = this.enemies,
            };
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText("save.json", json);
            // System.Diagnostics.Debug.WriteLine(json);
        }

        private void EnemiesUpdate()
        {
            // create enemy?
            if (this.rand.Next(120) < 3 && this.dayPhase == DayPhase.Night)
            {
                // choose direction
                if (this.rand.Next(2) == 0)
                {
                    this.enemies.Add(new Enemy(0, FloorPos, Enums.Direction.Right));
                }
                else
                {
                    this.enemies.Add(new Enemy(MapWidth, FloorPos, Enums.Direction.Left));
                }
            }

            // update enemies
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Update(this.Game.DeltaTime);

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
