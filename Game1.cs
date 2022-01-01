using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;

namespace MyGame
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public RenderTarget2D RenderTarget;
        public float DeltaTime;

        private readonly ScreenManager screenManager;
        private AssetsLoader assetsLoader = new AssetsLoader();

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

        // save data


        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.screenManager = new ScreenManager();
            this.Components.Add(this.screenManager);
        }

        protected override void Initialize()
        {
            // 60 fps
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0f);

            base.Initialize();

            // no border
            //Window.IsBorderless = true;

            // window size
            this.Graphics.PreferredBackBufferWidth = screenWidthDefault;
            this.Graphics.PreferredBackBufferHeight = screenHeightDefault;
            this.Graphics.ApplyChanges();

            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            // internal resolution
            this.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screenWidth, screenHeight);

            // start it with this scene
            this.LoadScreen1(false);
        }

        protected override void LoadContent()
        {
            // load all assets
            this.assetsLoader.Load(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            this.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Controls.Keyboard.GetState();
            Controls.Mouse.GetState();

            base.Update(gameTime);
        }

        public void LoadMenuScreen(bool transition = true)
        {
            if (transition)
            {
                this.screenManager.LoadScreen(new Screens.MenuScreen(this), new FadeTransition(this.GraphicsDevice, Color.Black));
            }
            else
            {
                this.screenManager.LoadScreen(new Screens.MenuScreen(this));
            }
        }

        public void LoadScreen1(bool transition = true)
        {
            if (transition)
            {
                this.screenManager.LoadScreen(new Screens.MapScreen(this), new FadeTransition(this.GraphicsDevice, Color.Black));
            }
            else
            {
                this.screenManager.LoadScreen(new Screens.MapScreen(this));
            }
        }

        // draw renderTarget to screen 
        public void DrawStart()
        {
            this.GraphicsDevice.SetRenderTarget(this.RenderTarget);
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.SpriteBatch.Begin();
        }

        public void DrawStart(Matrix transform)
        {
            this.GraphicsDevice.SetRenderTarget(this.RenderTarget);
            this.GraphicsDevice.Clear(Color.Black);

            this.SpriteBatch.Begin(transformMatrix: transform);
        }

        public void DrawEnd()
        {
            this.SpriteBatch.End();

            // calculate Scale and bars
            float outputAspect = this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height;
            float preferredAspect = screenWidth / (float)screenHeight;
            BarHeight = 0;
            BarWidth = 0;
            Rectangle dst;
            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((this.Window.ClientBounds.Width / preferredAspect));
                BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);

                Scale = 1f / ((float)screenWidth / this.Window.ClientBounds.Width);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((this.Window.ClientBounds.Height * preferredAspect));
                BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);

                Scale = 1f / ((float)screenHeight / this.Window.ClientBounds.Height);
            }

            this.GraphicsDevice.SetRenderTarget(null);
            this.Graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            this.SpriteBatch.Begin();
            this.SpriteBatch.Draw(this.RenderTarget, dst, Color.White);
            this.SpriteBatch.End();
        }
    }
}
