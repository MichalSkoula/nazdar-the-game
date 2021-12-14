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

        // default window size
        public const int screenWidthDefault = 1280;
        public const int screenHeightDefault = 720;

        // internal screen resolution
        public const int screenWidth = 1920;
        public const int screenHeight = 1080;

        // scaling + top / left bar
        public static float Scale { get; private set; }
        public static int BarHeight { get; private set; }
        public static int BarWidth { get; private set; }

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
            graphics.PreferredBackBufferWidth = screenWidthDefault;
            graphics.PreferredBackBufferHeight = screenHeightDefault;
            graphics.ApplyChanges();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // internal resolution
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
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
        }
        public void DrawStart(Matrix transform)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(transformMatrix: transform);
        }
        public void DrawEnd()
        {
            SpriteBatch.End();

            // calculate Scale and bars
            float outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float preferredAspect = screenWidth / (float)screenHeight;
            BarHeight = 0;
            BarWidth = 0;
            Rectangle dst;
            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect));
                BarHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, BarHeight, Window.ClientBounds.Width, presentHeight);

                Scale = 1f / ((float)screenWidth / Window.ClientBounds.Width);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect));
                BarWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(BarWidth, 0, presentWidth, Window.ClientBounds.Height);

                Scale = 1f / ((float)screenHeight / Window.ClientBounds.Height);
            }

            GraphicsDevice.SetRenderTarget(null);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, dst, Color.White);
            SpriteBatch.End();
        }
    }
}
