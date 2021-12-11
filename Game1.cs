using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;

namespace MyGame
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;
        public RenderTarget2D renderTarget;
        public float deltaTime;
        
        private readonly ScreenManager _screenManager;
        private AssetsLoader _assetsLoader = new AssetsLoader();
        public static float Scale { get; set; }
        public const int screenWidth = 1920;
        public const int screenHeight = 1080;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            // 60 fps
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0f);

            base.Initialize();

            // no border
            Window.IsBorderless = true;

            // window size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // internal resolution will always be 1080p
            renderTarget = new RenderTarget2D(GraphicsDevice, screenWidth, screenHeight);

            // start it with menu
            LoadMenuScreen(false);
        }

        protected override void LoadContent()
        {
            // load all assets
            _assetsLoader.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Controls.Keyboard.GetState();
            Controls.Mouse.GetState();

            base.Update(gameTime);
        }

        public void LoadMenuScreen(bool transition = true)
        {
            if (transition)
            {
                _screenManager.LoadScreen(new Screens.MenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
            }
            else
            {
                _screenManager.LoadScreen(new Screens.MenuScreen(this));
            }
        }

        public void LoadScreen1()
        {
            _screenManager.LoadScreen(new Screens.Screen1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        // draw renderTarget to screen 
        public void DrawStart()
        {
            Scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
        }
        public void DrawStart(Matrix transform)
        {
            Scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(transformMatrix: transform);
        }
        public void DrawEnd()
        {
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }
    }
}
