namespace MyGame.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using MonoGame.Extended.Screens;
    using MyGame.Controls;

    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MenuScreen(Game1 game)
            : base(game) { }

        private Button startButton;
        private Button fullscreenButton;
        private List<Button> resolutionButtons = new List<Button>();

        public override void Initialize()
        {
            this.startButton = new Button((Game1.screenWidth / 2) - 60, 50, null, ButtonSize.Large, "Start");
            this.fullscreenButton = new Button(10, 190, null, ButtonSize.Small, "Toggle Fullscreen");

            // get allowed resolutions
            int buttonIndex = 0;
            string currentResolution = string.Empty;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (this.GraphicsDevice.DisplayMode.Height < mode.Height || this.GraphicsDevice.DisplayMode.Width < mode.Width)
                {
                    continue;
                }

                if (mode.Height > mode.Width)
                {
                    continue;
                }

                if (mode.Height == Game1.screenHeightDefault && mode.Width == Game1.screenWidthDefault)
                {
                    currentResolution = mode.Width + "x" + mode.Height;
                }

                buttonIndex++;
                this.resolutionButtons.Add(new Button(10, 0, null, ButtonSize.Small, mode.Width + "x" + mode.Height, true, mode.Width + "x" + mode.Height));
            }

            // if more than 10 resolutions, lets remove every second one
            while (this.resolutionButtons.Count > 10)
            {
                for (int y = this.resolutionButtons.Count - 1; y >= 0; y--)
                {
                    // every second one, but not current
                    if (y % 2 == 1 && currentResolution != this.resolutionButtons[y].Data)
                    {
                        this.resolutionButtons.RemoveAt(y);
                    }
                }
            }

            // play song
            MediaPlayer.Play(Assets.Nature);

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // exit game from menu
                this.Game.Exit();
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
            {
                // to game
                this.Game.LoadScreen1();
            }

            // start
            this.startButton.Update();
            if (this.startButton.HasBeenClicked())
            {

                this.Game.LoadScreen1();
            }

            // fullscreen
            this.fullscreenButton.Update();
            if (this.fullscreenButton.HasBeenClicked())
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();
            }

            // resolution
            int buttonIndex = 0;
            foreach (Button btn in this.resolutionButtons)
            {
                buttonIndex++;
                btn.Hitbox = new Rectangle(btn.Hitbox.X, 210 + (buttonIndex * ((int)ButtonSize.Small + 10)), btn.Hitbox.Width, btn.Hitbox.Height);

                btn.Update();
                if (btn.HasBeenClicked())
                {
                    string[] resolution = btn.Data.Split('x');
                    this.Game.Graphics.PreferredBackBufferWidth = int.Parse(resolution[0]);
                    this.Game.Graphics.PreferredBackBufferHeight = int.Parse(resolution[1]);
                    this.Game.Graphics.ApplyChanges();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.DrawStart();

            this.startButton.Draw(this.Game.SpriteBatch);
            this.fullscreenButton.Draw(this.Game.SpriteBatch);

            if (this.resolutionButtons.Count > 0)
            {
                this.Game.SpriteBatch.DrawString(Assets.FontMedium, "Choose resolution", new Vector2(10, 140), Color.White);
                foreach (Button btn in this.resolutionButtons)
                {
                    btn.Draw(this.Game.SpriteBatch);
                }
            }

            this.Game.DrawEnd();
        }
    }
}
