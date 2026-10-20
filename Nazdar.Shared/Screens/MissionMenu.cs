using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nazdar.Controls;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;
using Keyboard = Nazdar.Controls.Keyboard;

namespace Nazdar.Screens
{
    // Provides the shared mission actions and load confirmation for campaign and survival menus.
    internal sealed class MissionMenu
    {
        private readonly Game1 game;
        private readonly Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        private readonly Dictionary<string, Button> confirmationButtons = new Dictionary<string, Button>();

        public bool IsConfirmingLoad { get; private set; }
        public string[] SaveDataLines { get; private set; }

        public MissionMenu(Game1 game)
        {
            this.game = game;
            this.Refresh();
            this.confirmationButtons.Add("yes", new Button(Offset.MenuX, 60, null, ButtonSize.Large, Translation.Get("gameOver.yes")));
            this.confirmationButtons.Add("no", new Button(Offset.MenuX, 100, null, ButtonSize.Large, Translation.Get("gameOver.no"), true));
        }

        private void Refresh()
        {
            var data = this.game.GetMissionMenuData();
            this.SaveDataLines = Tools.ParseSaveData(data);
            int village = data?["village"]?.Type == Newtonsoft.Json.Linq.JTokenType.Integer ? (int)data["village"] : 1;
            this.game.Village = this.game.SaveSlot == Save.Survival ? 0 : (village >= 1 && village <= Game1.MaxVillage ? village : 1);

            bool canResume = this.game.CanResumeMission;
            bool hasSave = this.game.HasMissionSave;
            this.buttons.Clear();
            this.buttons.Add("continue", new Button(Offset.MenuX, 60, null, ButtonSize.Large,
                Translation.Get(canResume ? "map.continue" : hasSave ? "map.loadGame" : "map.start"), true));

            int nextY = 101;
            // Only a paused game needs separate save and reload actions; otherwise the primary action loads directly.
            if (canResume)
            {
                this.buttons.Add("save", new Button(Offset.MenuX, nextY, null, ButtonSize.Medium, Translation.Get("map.saveGame")));
                nextY += 29;
                if (hasSave)
                {
                    this.buttons.Add("load", new Button(Offset.MenuX, nextY, null, ButtonSize.Medium, Translation.Get("map.loadGame")));
                    nextY += 29;
                }
            }
            if (hasSave)
            {
                this.buttons.Add("delete", new Button(Offset.MenuX, nextY, null, ButtonSize.Medium, Translation.Get("map.deleteSave")));
            }
            this.buttons.Add("menu", new Button(Offset.MenuX, 310, null, ButtonSize.Medium, Translation.Get("menu.backToMenu")));
        }

        private bool WasClicked(string key)
        {
            return this.buttons.TryGetValue(key, out var button) && button.HasBeenClicked();
        }

        public bool Update()
        {
            if (this.game.IsChangingScreen)
            {
                return true;
            }

            if (this.IsConfirmingLoad)
            {
                Button.UpdateButtons(this.confirmationButtons);
                if (this.confirmationButtons["no"].HasBeenClicked() || Keyboard.HasBeenPressed(Keys.Escape) || Gamepad.HasBeenPressed(Buttons.B))
                {
                    this.IsConfirmingLoad = false;
                }
                else if (this.confirmationButtons["yes"].HasBeenClicked())
                {
                    this.IsConfirmingLoad = false;
                    if (!this.game.LoadLastMissionSave())
                    {
                        this.Refresh();
                    }
                }
                return true;
            }

            Button.UpdateButtons(this.buttons);
            if (this.buttons["continue"].HasBeenClicked() || Gamepad.HasBeenPressed(Buttons.Start))
            {
                this.game.ContinueMission();
                return true;
            }
            if (this.WasClicked("save"))
            {
                this.game.SavePausedMission();
                this.Refresh();
                return true;
            }
            if (this.WasClicked("load"))
            {
                this.IsConfirmingLoad = true;
                this.confirmationButtons["no"].Focus = true;
                this.confirmationButtons["yes"].Focus = false;
                return true;
            }
            if (this.WasClicked("delete"))
            {
                this.game.LoadScreen(typeof(MapScreenDeleteSave));
                return true;
            }
            if (this.buttons["menu"].HasBeenClicked() || Keyboard.HasBeenPressed(Keys.Escape) || Gamepad.HasBeenPressed(Buttons.B))
            {
                this.game.LoadScreen(typeof(MenuScreen));
                return true;
            }
            return false;
        }

        public void Draw()
        {
            if (this.IsConfirmingLoad)
            {
                this.game.SpriteBatch.DrawString(Assets.Fonts["Large"], Translation.Get("map.confirmLoadTitle"),
                    new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);
                this.game.SpriteBatch.DrawString(Assets.Fonts["Small"], Translation.Get("map.confirmLoad1"),
                    new Vector2(Offset.MenuX, 150), MyColor.White);
                this.game.SpriteBatch.DrawString(Assets.Fonts["Small"], Translation.Get("map.confirmLoad2"),
                    new Vector2(Offset.MenuX, 168), MyColor.White);
            }

            foreach (var button in this.IsConfirmingLoad ? this.confirmationButtons.Values : this.buttons.Values)
            {
                button.Draw(this.game.SpriteBatch);
            }
            Game1.MessageBuffer.Draw(this.game.SpriteBatch);
        }
    }
}
