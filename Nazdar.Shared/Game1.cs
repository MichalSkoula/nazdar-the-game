using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Nazdar.Messages;
using Nazdar.Shared;
using Newtonsoft.Json.Linq;
using System;

namespace Nazdar
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

        public static Enums.Platform CurrentPlatform;

        // scaling + top / left bar
        public static float Scale { get; private set; }

        public static int BarHeight { get; private set; }

        public static int BarWidth { get; private set; }

        public string SaveSlot { get; set; }

        public static JObject SaveTempData { get; set; }

        // world variables
        public int Village { get; set; } = 1;
        public static MessageBuffer MessageBuffer = new MessageBuffer();
        public static int GlobalTimer { get; private set; }
        public static bool NextLevelAnimation { get; set; } = false;
        public bool FirstRun { get; set; } = false;

        public static int[] Salt = new int[100];

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

            if (CurrentPlatform == Enums.Platform.Android)
            {
                // on Android, lets go fullscreen
                this.Graphics.IsFullScreen = true;
                this.Graphics.ApplyChanges();
            }
            else if (CurrentPlatform == Enums.Platform.GL)
            {
                // on pc, adjust default window size
                this.Graphics.PreferredBackBufferWidth = Enums.Screen.WidthDefault;
                this.Graphics.PreferredBackBufferHeight = Enums.Screen.HeightDefault;
                this.Graphics.ApplyChanges();
            }

            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            // internal resolution
            this.RenderTarget = new RenderTarget2D(this.GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);

            // fill salt
            for (int i = 0; i < Salt.Length; i++)
            {
                Salt[i] = Tools.GetRandom(2); // 0,1
            }

            // load settings
            Settings.LoadSettings(this);

            // start it with this scene
#if DEBUG
            this.LoadScreen(typeof(Screens.MenuScreen));

            System.Diagnostics.Debug.WriteLine(this.Window.ClientBounds.Width + " " + this.Window.ClientBounds.Height);
            System.Diagnostics.Debug.WriteLine(GraphicsDevice.Viewport.Width + " " + this.GraphicsDevice.Viewport.Height);
            System.Diagnostics.Debug.WriteLine(GraphicsDevice.Adapter.CurrentDisplayMode.Width + " " + GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            System.Diagnostics.Debug.WriteLine(GraphicsDevice.PresentationParameters.BackBufferWidth + " " + GraphicsDevice.PresentationParameters.BackBufferHeight);
#else
            IsMouseVisible = false;
            this.LoadScreen(typeof(Screens.SplashScreen));
#endif
        }

        protected override void LoadContent()
        {
            // load all assets
            this.assetsLoader.Load(this.Content, this.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            this.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Controls.Keyboard.GetState(this.IsActive);
            Controls.Gamepad.GetState(this.IsActive);
            Controls.Mouse.GetState(this.IsActive);
            Controls.Touch.GetState(this.IsActive);

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

        public void LoadScreen(Type type, bool transition = true)
        {
            var screen = Activator.CreateInstance(type, this);
            if (transition)
            {
                this.screenManager.LoadScreen((Screen)screen, new FadeTransition(this.GraphicsDevice, MyColor.Black, 0.5f));
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
            this.GraphicsDevice.Clear(MyColor.Black);

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
            float outputAspect = GraphicsDevice.Adapter.CurrentDisplayMode.Width / (float)GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            float preferredAspect = Enums.Screen.Width / (float)Enums.Screen.Height;
            BarHeight = 0;
            BarWidth = 0;
            Rectangle dst;

            if (outputAspect <= preferredAspect)
            {
                // bars on top/bottom
                int presentHeight = (int)(this.Window.ClientBounds.Width / preferredAspect);
                BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);
                Scale = 1f / ((float)Enums.Screen.Width / this.Window.ClientBounds.Width);
            }
            else
            {
                // bars left/right
                int presentWidth = (int)(this.Window.ClientBounds.Height * preferredAspect);
                BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);
                Scale = 1f / ((float)Enums.Screen.Height / this.Window.ClientBounds.Height);
            }

            this.GraphicsDevice.SetRenderTarget(null);
            this.Graphics.GraphicsDevice.Clear(ClearOptions.Target, MyColor.Black, 1.0f, 0);

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
