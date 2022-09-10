using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using SiberianAnabasis.Messages;
using SiberianAnabasis.Shared;
using System;

namespace SiberianAnabasis
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

        // scaling + top / left bar
        public static float Scale { get; private set; }

        public static int BarHeight { get; private set; }

        public static int BarWidth { get; private set; }

        public string SaveSlot { get; set; }

        // world variables
        public int Village { get; set; }
        public static MessageBuffer MessageBuffer = new MessageBuffer();
        public static int GlobalTimer { get; private set; }

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
            this.Graphics.PreferredBackBufferWidth = Enums.Screen.WidthDefault;
            this.Graphics.PreferredBackBufferHeight = Enums.Screen.HeightDefault;
            this.Graphics.ApplyChanges();

            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            // internal resolution
            this.RenderTarget = new RenderTarget2D(this.GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);

            // start it with this scene
#if DEBUG
            this.LoadScreen(typeof(Screens.MenuScreen));
#else
            this.LoadScreen(typeof(Screens.SplashScreen));
#endif

            System.Diagnostics.Debug.WriteLine(this.Window.ClientBounds.Width + " " + this.Window.ClientBounds.Height);
            System.Diagnostics.Debug.WriteLine(GraphicsDevice.Viewport.Width + " " + this.GraphicsDevice.Viewport.Height);
            System.Diagnostics.Debug.WriteLine(GraphicsDevice.Adapter.CurrentDisplayMode.Width + " " + GraphicsDevice.Adapter.CurrentDisplayMode.Height);
        }

        protected override void LoadContent()
        {
            // load all assets
            this.assetsLoader.Load(this.Content, this.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            this.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Controls.Keyboard.GetState();
            Controls.Gamepad.GetState();
            Controls.Mouse.GetState();

            MessageBuffer.Update(this.DeltaTime);

            GlobalTimer++;
            if (GlobalTimer > 100)
            {
                GlobalTimer = 0;
            }

            // bg music
            Audio.PlaySongCollection();

            base.Update(gameTime);
        }

        public void LoadScreen(Type type, bool transition = false)
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
            float preferredAspect = Enums.Screen.Width / (float)Enums.Screen.Height;
            BarHeight = 0;
            BarWidth = 0;
            Rectangle dst;
            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)(this.Window.ClientBounds.Width / preferredAspect);
                BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);

                Scale = 1f / ((float)Enums.Screen.Width / this.Window.ClientBounds.Width);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)(this.Window.ClientBounds.Height * preferredAspect);
                BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);

                Scale = 1f / ((float)Enums.Screen.Height / this.Window.ClientBounds.Height);
            }

            // sometimes this helps for xbox
            //dst = new Rectangle(0, 0, Enums.Screen.WidthDefault, Enums.Screen.HeightDefault);

            this.GraphicsDevice.SetRenderTarget(null);
            this.Graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            this.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp);
            this.SpriteBatch.Draw(texture: this.RenderTarget, destinationRectangle: dst, color: Color.White);
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
