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

        private Button startButton1;
        private Button startButton2;
        private Button startButton3;

        private Button exitButton;
        private Button fullscreenButton;

        public override void Initialize()
        {
            this.startButton1 = new Button(20, 60, null, Enums.ButtonSize.Large, "Slot #1");
            this.startButton2 = new Button(20, 100, null, Enums.ButtonSize.Large, "Slot #2");
            this.startButton3 = new Button(20, 140, null, Enums.ButtonSize.Large, "Slot #3");

            this.fullscreenButton = new Button(20, 240, null, Enums.ButtonSize.Medium, "Toggle Fullscreen");
            this.exitButton = new Button(20, 280, null, Enums.ButtonSize.Medium, "Exit");

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
            this.startButton1.Update();
            this.startButton2.Update();
            this.startButton3.Update();

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
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter) || this.startButton1.HasBeenClicked())
            {
                this.Game.Slot = 1;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            } 
            else if (this.startButton2.HasBeenClicked())
            {
                this.Game.Slot = 2;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            } 
            else if (this.startButton3.HasBeenClicked())
            {
                this.Game.Slot = 3;
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }
        }
        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "Start the game", new Vector2(20, 20), Color.White);
            this.startButton1.Draw(this.Game.SpriteBatch);
            this.startButton2.Draw(this.Game.SpriteBatch);
            this.startButton3.Draw(this.Game.SpriteBatch);

            this.fullscreenButton.Draw(this.Game.SpriteBatch);
            this.exitButton.Draw(this.Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
