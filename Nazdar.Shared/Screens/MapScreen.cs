using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Nazdar.Shared;
using Nazdar.Shared.Translation;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Screens
{
    public class MapScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public MapScreen(Game1 game) : base(game) { }

        private MissionMenu missionMenu;

        // every mission (village) should have a description, revealed line by line in a panel
        // aligned with the mission menu: top of the first button (60) to the bottom of "Back to menu" (332)
        private static readonly Rectangle descriptionPanel = new Rectangle(272, 60, 358, 272);
        private const int DescriptionPadding = 12;
        private const int DescriptionLineHeight = 18;
        private const float DescriptionLineDelay = 0.4f;
        private const float DescriptionLineFade = 0.3f;
        private float descriptionTimer;
        private Dictionary<int, string[]> villageDescriptions;
        private List<(string Text, bool Heading)> descriptionLines;

        public override void Initialize()
        {
            // Initialize mission descriptions with translations
            villageDescriptions = new Dictionary<int, string[]>()
            {
                {
                    1, new string[] {
                        Translation.Get("mission1.line1"),
                        Translation.Get("mission1.line2"),
                        Translation.Get("mission1.line3"),
                        "",
                        Translation.Get("mission1.goals"),
                        Translation.Get("mission1.goal1"),
                    }
                },
                {
                    2, new string[] {
                        Translation.Get("mission2.line1"),
                        Translation.Get("mission2.line2"),
                        "",
                        Translation.Get("mission2.goals"),
                        Translation.Get("mission2.goal1"),
                        "",
                        Translation.Get("mission2.tips"),
                        Translation.Get("mission2.tip1"),
                    }
                },
                {
                    3, new string[] {
                        Translation.Get("mission3.line1"),
                        Translation.Get("mission3.line2"),
                        "",
                        Translation.Get("mission3.goals"),
                        Translation.Get("mission3.goal1"),
                        Translation.Get("mission3.goal2"),
                        "",
                        Translation.Get("mission3.tips"),
                        Translation.Get("mission3.tip1"),
                        Translation.Get("mission3.tip2"),
                        Translation.Get("mission3.tip3"),
                    }
                },
                {
                    4, new string[] {
                        Translation.Get("mission4.line1"),
                        Translation.Get("mission4.line2"),
                        Translation.Get("mission4.line3"),
                        Translation.Get("mission4.line4"),
                        "",
                        Translation.Get("mission4.goals"),
                        Translation.Get("mission4.goal1"),
                        "",
                        Translation.Get("mission4.tips"),
                        Translation.Get("mission4.tip1"),
                        Translation.Get("mission4.tip2"),
                    }
                },
                {
                    5, new string[] {
                        Translation.Get("mission5.line1"),
                        Translation.Get("mission5.line2"),
                        "",
                        Translation.Get("mission5.goals"),
                        Translation.Get("mission5.goal1"),
                        Translation.Get("mission5.goal2"),
                        "",
                        Translation.Get("mission5.tips"),
                        Translation.Get("mission5.tip1"),
                        Translation.Get("mission5.tip2"),
                        Translation.Get("mission5.tip3"),
                    }
                },
                {
                    6, new string[] {
                        Translation.Get("mission6.line1"),
                        Translation.Get("mission6.line2"),
                        Translation.Get("mission6.line3"),
                        "",
                        Translation.Get("mission6.goals"),
                        Translation.Get("mission6.goal1"),
                        "",
                        Translation.Get("mission6.tips"),
                        Translation.Get("mission6.tip1"),
                        Translation.Get("mission6.tip2"),
                        Translation.Get("mission6.tip3"),
                        Translation.Get("mission6.tip4"),
                    }
                },
                {
                    7, new string[] {
                        Translation.Get("mission7.line1"),
                        Translation.Get("mission7.line2"),
                        Translation.Get("mission7.line3"),
                        Translation.Get("mission7.line4"),
                        "",
                        Translation.Get("mission7.goals"),
                        Translation.Get("mission7.goal1"),
                        Translation.Get("mission7.goal2"),
                        Translation.Get("mission7.goal3"),
                        "",
                        Translation.Get("mission7.tips"),
                        Translation.Get("mission7.tip1"),
                    }
                },
                {
                    8, new string[] {
                        Translation.Get("mission8.line1"),
                        Translation.Get("mission8.line2"),
                        Translation.Get("mission8.line3"),
                        Translation.Get("mission8.line4"),
                        "",
                        Translation.Get("mission8.goals"),
                        Translation.Get("mission8.goal1"),
                        "",
                        Translation.Get("mission8.tips"),
                        Translation.Get("mission8.tip1"),
                        Translation.Get("mission8.tip2"),
                        Translation.Get("mission8.tip3"),
                        Translation.Get("mission8.tip4"),
                        Translation.Get("mission8.tip5"),
                    }
                },
            };

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

            this.descriptionTimer += this.Game.DeltaTime;
        }

        // a line after an empty one is a section heading (goals, tips); lines too long for the panel are wrapped
        private List<(string Text, bool Heading)> GetDescriptionLines()
        {
            int maxWidth = descriptionPanel.Width - (2 * DescriptionPadding);
            string[] source = this.villageDescriptions[this.Game.Village];
            List<(string Text, bool Heading)> lines = new List<(string Text, bool Heading)>();
            for (int j = 0; j < source.Length; j++)
            {
                bool heading = j > 0 && source[j - 1] == "";
                string current = "";
                foreach (string word in source[j].Split(' '))
                {
                    string candidate = current == "" ? word : current + " " + word;
                    if (current != "" && Assets.Fonts["Small"].MeasureString(candidate).X > maxWidth)
                    {
                        lines.Add((current, heading));
                        current = word;
                    }
                    else
                    {
                        current = candidate;
                    }
                }
                lines.Add((current, heading));
            }

            return lines;
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

            // description panel
            this.Game.SpriteBatch.FillRectangle(descriptionPanel, MyColor.DarkViolet * 0.5f);
            this.Game.SpriteBatch.DrawRectangle(descriptionPanel, MyColor.Gray4);

            // lines appear one by one
            List<(string Text, bool Heading)> lines = this.descriptionLines ??= this.GetDescriptionLines();
            for (int j = 0; j < lines.Count; j++)
            {
                float alpha = MathHelper.Clamp((this.descriptionTimer - (j * DescriptionLineDelay)) / DescriptionLineFade, 0, 1);
                if (alpha <= 0)
                {
                    break;
                }

                this.Game.SpriteBatch.DrawString(
                    Assets.Fonts["Small"],
                    lines[j].Text,
                    new Vector2(descriptionPanel.X + DescriptionPadding, descriptionPanel.Y + DescriptionPadding + (DescriptionLineHeight * j)),
                    (lines[j].Heading ? MyColor.Yellow : MyColor.Gray1) * alpha);
            }

            this.Game.DrawEnd();
        }
    }
}
