using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyGame.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public MapScreen(Game1 game) : base(game) { }

        private Camera _camera = new Camera();

        private Random _rand = new Random();

        // how large the map will be
        private int _numberOfScreens = 4;
        // will be calculated
        public static int mapWidth;

        // Y positions
        private const int StatusBarPosition = 900;
        public const int FloorPos = 700;
        
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();

        public override void Initialize()
        {
            mapWidth = Game1.screenWidth * _numberOfScreens;

            // stop whatever menu song is playing 
            MediaPlayer.Stop();

            // create player in the center of the map
            _player = new Player(mapWidth / 2, FloorPos);

            // maybe load?
            Load();

            base.Initialize();
        }

        private void Save()
        {
            var saveData = new
            {
                player = _player
            };
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText("save.json", json);
            System.Diagnostics.Debug.WriteLine(json);
        }

        public void Load()
        {
            if (!File.Exists("save.json"))
            {
                return;
            }

            string json = File.ReadAllText("save.json");
            dynamic saveData = JObject.Parse(json);

            _player.Load(saveData.player);
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // save
                Save();

                // back to menu
                Game.LoadMenuScreen();
            }

            _player.Update(Game.deltaTime);

            _camera.Follow(_player);

            // create enemy?
            if (_rand.Next(100) < 3)
            {
                // choose direction
                if (_rand.Next(2) == 0)
                {
                    _enemies.Add(new Enemy(0, FloorPos, Enums.Direction.Right));
                } 
                else
                {
                    _enemies.Add(new Enemy(mapWidth, FloorPos, Enums.Direction.Left));
                }
            }
            foreach (Enemy enemy in _enemies)
            {
                enemy.Update(Game.deltaTime);
            }
            _enemies.RemoveAll(p => p.ToDelete);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart(_camera.Transform);

            // background 
            for (int i = 0; i < _numberOfScreens; i++)
            {
                Game.SpriteBatch.Draw(Assets.background, new Vector2(i * Game1.screenWidth, 0), Color.White);
            }

            // left tunnel
            Game.SpriteBatch.Draw(Assets.tunnel, Vector2.Zero, Color.White);

            // right tunnel - all this for a flip
            Game.SpriteBatch.Draw(
                Assets.tunnel,
                new Rectangle(Game1.screenWidth * _numberOfScreens - Assets.tunnel.Width, 0, Assets.tunnel.Width, Assets.tunnel.Height),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipHorizontally,
                0
            );

            // status bar (minus translation to stay in place)
            Game.SpriteBatch.DrawString(
                Assets.fontLarge,
                "this is game you can escape to menu",
                new Vector2(10 - _camera.Transform.Translation.X, StatusBarPosition),
                Color.White
                );

            // player - camera follows
            _player.Draw(Game.SpriteBatch);

            // enemies
            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(Game.SpriteBatch);
            }

            Game.DrawEnd();
        }
    }
}
