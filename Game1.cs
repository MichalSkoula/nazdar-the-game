using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;

namespace ScreenTest
{
    enum ScreenNames
    {
        MenuScreen,
        Screen1
    }

    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;
        public RenderTarget2D renderTarget;
        public float deltaTime;

        private readonly ScreenManager _screenManager;
        private AssetsLoader _assetsLoader = new AssetsLoader();
        private ScreenNames _currentScreen = ScreenNames.MenuScreen;
        private float scale;

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

            // window size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // start it with menu
            LoadMenuScreen();
        }

        protected override void LoadContent()
        {
            // load all assets
            _assetsLoader.Load(Content);

            // internal resolution will always be 1080p
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Keyboard.GetState();
            Mouse.GetState();

            base.Update(gameTime);
        }

        public void LoadMenuScreen()
        {
            _currentScreen = ScreenNames.MenuScreen;
            _screenManager.LoadScreen(new Screens.MenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreen1()
        {
            _currentScreen = ScreenNames.Screen1;
            _screenManager.LoadScreen(new Screens.Screen1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        // draw renderTarget to screen 
        public void DrawStart()
        {
            scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
        }
        public void DrawEnd()
        {
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }
    }
}
