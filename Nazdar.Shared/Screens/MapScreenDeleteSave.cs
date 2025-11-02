using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Nazdar.Controls;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    internal class MapScreenDeleteSave : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreenDeleteSave(Game1 game) : base(game) { }

        private readonly Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public override void Initialize()
        {
            buttons.Add("yes", new Button(Offset.MenuX, 60, null, ButtonSize.Large, Translation.Get("gameOver.yes")));
            buttons.Add("no", new Button(Offset.MenuX, 100, null, ButtonSize.Large, Translation.Get("gameOver.no"), true));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Button.UpdateButtons(this.buttons);

            // main menu - NO
            if (this.buttons.GetValueOrDefault("no").HasBeenClicked() || Controls.Keyboard.HasBeenPressed(Keys.Escape) || Controls.Gamepad.HasBeenPressed(Buttons.B))
            {
                this.Game.LoadScreen(typeof(Screens.MapScreen));
            }

            // new game - YES
            if (this.buttons.GetValueOrDefault("yes").HasBeenClicked())
            {
                // delete save
                FileIO saveFile = new FileIO(Game.SaveSlot);
                saveFile.Delete();

                // reset some variables
                this.Game.Village = 1;

                // back to menu
                this.Game.LoadScreen(typeof(Screens.MenuScreen));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.Matrix = null;
            this.Game.DrawStart();

            // title
            this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], Translation.Get("mapDeleteSave.title"), new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);

            // buttons
            foreach (KeyValuePair<string, Button> button in this.buttons)
            {
                button.Value.Draw(this.Game.SpriteBatch);
            }

            // messages
            Game1.MessageBuffer.Draw(Game.SpriteBatch);

            this.Game.DrawEnd();
        }
    }
}
