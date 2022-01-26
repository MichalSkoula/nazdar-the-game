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

        public override void Initialize()
        {
            this.startButton = new Button((Game1.screenWidth / 2) - 60, 25, null, ButtonSize.Large, "Start");
            this.fullscreenButton = new Button((Game1.screenWidth / 2) - 60, 85, null, ButtonSize.Small, "Toggle Fullscreen");

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
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // start
            this.startButton.Update();
            if (this.startButton.HasBeenClicked())
            {

                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // fullscreen
            this.fullscreenButton.Update();
            if (this.fullscreenButton.HasBeenClicked())
            {
                this.Game.Graphics.IsFullScreen = !this.Game.Graphics.IsFullScreen;
                this.Game.Graphics.ApplyChanges();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.startButton.Draw(this.Game.SpriteBatch);
            this.fullscreenButton.Draw(this.Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
