using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MyGame.Controls;

namespace MyGame.Screens
{
    public class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public MenuScreen(Game1 game) : base(game) { }

        private Button _startButton;
        private List<Button> _resolutionButtons = new List<Button>();

        public override void Initialize()
        {
            _startButton = new Button(Game1.screenWidth / 2 - 60, 50, null, ButtonSize.large, "Start");

            // get allowed resolutions (+for fullscreen mode)
            int i = 0;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                bool is169 = (Math.Round((double)mode.Width / mode.Height, 2) == 1.78);
                if (!is169)
                {
                    continue;
                }

                i++;
                _resolutionButtons.Add(new Button(
                    10, 150 + i * ((int)ButtonSize.small + 10), 
                    null, ButtonSize.small, 
                    mode.Width + "x" + mode.Height, 
                    true, 
                    mode.Width + "x" + mode.Height + "xFalse"
                ));

                i++;
                _resolutionButtons.Add(new Button(
                    10, 150 + i * ((int)ButtonSize.small + 10), 
                    null, ButtonSize.small, 
                    mode.Width + "x" + mode.Height + " Fullscreen", 
                    true, 
                    mode.Width + "x" + mode.Height + "xTrue"
                ));
            }

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
                Game.Exit();
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
            {
                // to game
                Game.LoadScreen1();
            }

            // start
            _startButton.Update();
            if (_startButton.HasBeenClicked())
            {
                
                Game.LoadScreen1();
            }

            // resolution
            foreach (Button btn in _resolutionButtons)
            {
                btn.Update();
                if (btn.HasBeenClicked())
                {
                    string[] resolution = btn.Data.Split('x');
                    Game.graphics.PreferredBackBufferWidth = Int32.Parse(resolution[0]);
                    Game.graphics.PreferredBackBufferHeight = Int32.Parse(resolution[1]);
                    Game.graphics.IsFullScreen = resolution[2] == "True";
                    Game.graphics.ApplyChanges();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.DrawStart();

            _startButton.Draw(Game.SpriteBatch);

            if (_resolutionButtons.Count > 0)
            {
                Game.SpriteBatch.DrawString(Assets.fontMedium, "Choose resolution", new Vector2(10, 140), Color.White);
                foreach (Button btn in _resolutionButtons)
                {
                    btn.Draw(Game.SpriteBatch);
                }
            }

            Game.DrawEnd();
        }
    }
}
