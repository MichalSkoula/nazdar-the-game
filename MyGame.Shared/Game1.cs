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
        public Matrix? Matrix = null;
        public float DeltaTime;

        private readonly ScreenManager screenManager;
        private AssetsLoader assetsLoader = new AssetsLoader();

        // default window size
        public const int screenWidthDefault = 1280;
        public const int screenHeightDefault = 720;

        // internal screen resolution
        public const int screenWidth = 640;
        public const int screenHeight = 360;

        // scaling + top / left bar
        public static float Scale { get; private set; }

        public static int BarHeight { get; private set; }

        public static int BarWidth { get; private set; }

        public string SaveSlot { get; set; }

        // world variables
        public int Village { get; set; }
        public int VillageAccess { get; set; }
        public bool Traveling { get; set; }

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

            // TODO decide
            // Window.IsBorderless = true;

            // window size
            this.Graphics.PreferredBackBufferWidth = screenWidthDefault;
            this.Graphics.PreferredBackBufferHeight = screenHeightDefault;
            this.Graphics.ApplyChanges();

            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            // internal resolution
            this.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screenWidth, screenHeight);

            // start it with this scene
            #if DEBUG
                this.LoadScreen(typeof(Screens.MenuScreen), false);
            #else
                this.LoadScreen(typeof(Screens.SplashScreen), false);
            #endif
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

        public void LoadScreen(Type type, bool transition = true)
        {
            var screen = Activator.CreateInstance(type, this);
            if (transition)
            {
                this.screenManager.LoadScreen((Screen)screen, new FadeTransition(this.GraphicsDevice, Color.Black));
            }
            else
            {
                this.screenManager.LoadScreen((Screen)screen);
            }
        }

        // draw renderTarget to screen
        public void DrawStart()
        {
            this.GraphicsDevice.SetRenderTarget(this.RenderTarget);
            this.GraphicsDevice.Clear(Color.Black);

            this.SpriteBatchStart();
        }

        // when drawing image with effect, use this before and after
        public void EffectStart(Effect effect = null)
        {
            this.SpriteBatch.End();

            this.SpriteBatchStart(effect: effect);
        }

        public void EffectEnd()
        {
            this.SpriteBatch.End();

            this.SpriteBatchStart();
        }

        // draw it to screen
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
                int presentHeight = (int)(this.Window.ClientBounds.Width / preferredAspect);
                BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);

                Scale = 1f / ((float)screenWidth / this.Window.ClientBounds.Width);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)(this.Window.ClientBounds.Height * preferredAspect);
                BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);

                Scale = 1f / ((float)screenHeight / this.Window.ClientBounds.Height);
            }

            this.GraphicsDevice.SetRenderTarget(null);
            this.Graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            this.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp);
            this.SpriteBatch.Draw(this.RenderTarget, dst, Color.White);
            this.SpriteBatch.End();
        }

        private void SpriteBatchStart(Effect? effect = null)
        {
            this.SpriteBatch.Begin(
                transformMatrix: this.Matrix.HasValue ? this.Matrix : null,
                effect: effect);
        }
    }
}
