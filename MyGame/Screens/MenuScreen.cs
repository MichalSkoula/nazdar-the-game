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

        public MenuScreen(Game1 game) : base(game) { }

        private Button startButton;
        private Button exitButton;
        private Button fullscreenButton;

        public override void Initialize()
        {
            this.startButton = new Button((Game1.screenWidth / 2) - 100, 25, null, Enums.ButtonSize.Large, "Start");
            this.fullscreenButton = new Button((Game1.screenWidth / 2) - 100, 85, null, Enums.ButtonSize.Medium, "Toggle Fullscreen");
            this.exitButton = new Button((Game1.screenWidth / 2) - 100, 125, null, Enums.ButtonSize.Small, "Exit");

            // play song
            MediaPlayer.Play(Assets.Nature);

            // load and apply settings from json file
            // we load this also in splashscreen
            Settings.LoadSettings(Game);

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // update buttons
            this.startButton.Update();
            this.fullscreenButton.Update();
            this.exitButton.Update();

            // fullscreen
            if (Controls.Keyboard.HasBeenPressed(Keys.F) || this.fullscreenButton.HasBeenClicked())
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();

                Settings.SaveSettings(Game);
            }

            // exit game from menu
            if (Controls.Keyboard.HasBeenPressed(Keys.Escape) || this.exitButton.HasBeenClicked())
            {
                this.Game.Exit();
            }

            // start game
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter) || this.startButton.HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
        }
        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.startButton.Draw(this.Game.SpriteBatch);
            this.fullscreenButton.Draw(this.Game.SpriteBatch);
            this.exitButton.Draw(this.Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
