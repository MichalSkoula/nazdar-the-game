﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MyGame.Controls;
using MyGame.Shared;
using static MyGame.Enums;

namespace MyGame.Screens
{
    public class GameOverScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public GameOverScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            buttons.Add("load", new Button(Offset.MenuX, 60, null, ButtonSize.Large, "Load last save", true));
            buttons.Add("new", new Button(Offset.MenuX, 100, null, ButtonSize.Large, "New game"));
            buttons.Add("menu", new Button(Offset.MenuX, 140, null, ButtonSize.Large, "Main menu"));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // update buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Update();
            }

            // iterate through buttons up/down
            if (Controls.Keyboard.HasBeenPressed(Keys.Down))
            {
                Tools.ButtonsIterateWithKeys(Direction.Down, this.buttons);
            }
            else if (Controls.Keyboard.HasBeenPressed(Keys.Up))
            {
                Tools.ButtonsIterateWithKeys(Direction.Up, this.buttons);
            }

            // enter? some button has focus? click!
            if (Controls.Keyboard.HasBeenPressed(Keys.Enter))
            {
                foreach (KeyValuePair<string, Button> button in this.buttons)
                {
                    if (button.Value.Focus)
                    {
                        button.Value.Clicked = true;
                        break;
                    }
                }
            }

            // load game
            if (this.buttons.GetValueOrDefault("load").HasBeenClicked())
            {
                this.Game.LoadScreen(typeof(Screens.DetailScreen));
            }

            // main menu
            if (this.buttons.GetValueOrDefault("menu").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // new game
            if (this.buttons.GetValueOrDefault("new").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape))
            {
                // delete save
                FileIO saveFile = new FileIO(Game.SaveSlot);
                saveFile.Delete();

                this.Game.LoadScreen(typeof(Screens.DetailScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.FontLarge, "GAME OVER", new Vector2(Offset.MenuX, Offset.MenuY), Color.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            this.Game.DrawEnd();
        }
    }
}