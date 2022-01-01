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

    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game)
            : base(game) { }

        private Camera camera = new Camera();

        private Random rand = new Random();

        // how large the map will be
        private int numberOfScreens = 4;

        // will be calculated
        public static int MapWidth;

        // Y positions
        private const int StatusBarPosition = 900;
        public const int FloorPos = 700;

        private Player player;
        private List<Enemy> enemies = new List<Enemy>();

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
                this.Game.LoadMenuScreen();
            }

            this.player.Update(this.Game.DeltaTime);

            this.camera.Follow(this.player);

            this.EnemiesUpdate();
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.DrawStart(this.camera.Transform);

            // background
            for (int i = 0; i < this.numberOfScreens; i++)
            {
                this.Game.SpriteBatch.Draw(Assets.Background, new Vector2(i * Game1.screenWidth, 0), Color.White);
            }

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
                0
            );

            // status bar (minus translation to stay in place)
            this.Game.SpriteBatch.DrawString(
                Assets.FontLarge,
                "this is game you can escape to menu",
                new Vector2(10 - this.camera.Transform.Translation.X, StatusBarPosition),
                Color.White
                );

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

            string json = File.ReadAllText("save.json");
            dynamic saveData = JObject.Parse(json);

            this.player.Load(saveData.player);
        }

        private void Save()
        {
            var saveData = new
            {
                player = this.player,
            };
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText("save.json", json);
            System.Diagnostics.Debug.WriteLine(json);
        }

        private void EnemiesUpdate()
        {
            // create enemy?
            if (this.rand.Next(120) < 3)
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
            }

            this.enemies.RemoveAll(p => p.ToDelete);
        }
    }
}
