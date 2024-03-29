﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class GameFinishedScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public GameFinishedScreen(Game1 game) : base(game) { }

        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private string[] saveDataLines;

        private readonly string[] outro =
        {
            "You took over the entire Trans-Siberian Railway,",
            "but you didn't stop those rotten Bolsheviks from",
            "taking control of all Russia.",
            "",
            "However you achieved the main goal - convinced the",
            "Allied Powers about the project of an independent",
            "Czechoslovakia."
        };
        private float descriptionY = 300;
        private readonly int descriptionYStop = 110;
        private readonly int descriptionSpeed = 40;

        public override void Initialize()
        {
            buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, "Back to Menu", true));

            this.Load();

            Audio.SongVolume = 0.25f;

            base.Initialize();
        }

        private void Load()
        {
            // try to get current stats to show
            FileIO saveFile = new FileIO(Game.SaveSlot);

            dynamic saveData = saveFile.Load();
            this.saveDataLines = Tools.ParseSaveData(saveData);
        }

        public override void Update(GameTime gameTime)
        {
            Button.UpdateButtons(this.buttons);

            // main menu
            if (this.buttons.GetValueOrDefault("menu").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }

            // move description
            if (this.descriptionY > descriptionYStop)
            {
                this.descriptionY -= this.Game.DeltaTime * descriptionSpeed;
            }
            else
            {
                // to be precise
                this.descriptionY = descriptionYStop;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], "YOU WON!", new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            // save data
            int i = 0;
            foreach (string line in this.saveDataLines)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 100 + 28 * i), MyColor.White);
            }

            // outro up
            i = 0;
            foreach (string line in this.outro)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 250, descriptionY + 18 * i), MyColor.Gray1);
            }

            this.Game.DrawEnd();
        }
    }
}
