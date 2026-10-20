using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class SurvivalScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public SurvivalScreen(Game1 game) : base(game) { }

        private MissionMenu missionMenu;

        // every mission (village) should have a description
        private float descriptionY = 300;
        private readonly int descriptionYStop = 62;
        private readonly int descriptionSpeed = 40;
        private readonly string[] description = new string[] {
 Translation.Get("survival.line1"),
            Translation.Get("survival.line2"),
          Translation.Get("survival.line3"),
            Translation.Get("survival.line4"),
            "",
         Translation.Get("survival.goals"),
            Translation.Get("survival.goal1"),
         "",
            Translation.Get("survival.tips"),
        Translation.Get("survival.tip1"),
    Translation.Get("survival.tip2"),
            Translation.Get("survival.tip3")
        };

        public override void Initialize()
        {
            this.missionMenu = new MissionMenu(this.Game);

            Audio.SongVolume = 0.25f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.missionMenu.Update())
            {
                return;
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

            if (!this.missionMenu.IsConfirmingLoad)
            {
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Large"], ((Villages)this.Game.Village).ToString(), new Vector2(Offset.MenuX, Offset.MenuY), MyColor.White);
            }

            this.missionMenu.Draw();
            if (this.missionMenu.IsConfirmingLoad)
            {
                this.Game.DrawEnd();
                return;
            }

            // save data - show only some things
            int i = 0;
            foreach (string line in this.missionMenu.SaveDataLines)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Medium"], line, new Vector2(Offset.MenuX, Offset.MenuY + 130 + (24 * i)), MyColor.White);
            }

            // descriptions
            i = 0;
            foreach (string line in this.description)
            {
                i++;
                this.Game.SpriteBatch.DrawString(Assets.Fonts["Small"], line, new Vector2(Offset.MenuX + 225, Offset.MenuY + descriptionY + (18 * i)), MyColor.Gray1);
            }

            this.Game.DrawEnd();
        }
    }
}
