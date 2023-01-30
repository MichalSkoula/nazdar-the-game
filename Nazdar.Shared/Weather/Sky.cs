using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nazdar.Screens;
using Nazdar.Shared;
using System.Collections.Generic;
using static Nazdar.Enums;

namespace Nazdar.Weather
{
    public class Sky
    {
        private List<IDrop> drops = new List<IDrop>();
        public double Ttl = 0;
        public DropType Type = DropType.Raining;
        public int DropCount
        {
            get
            {
                return drops.Count;
            }
        }
        public bool Active
        {
            get
            {
                return Ttl > 0;
            }
        }

        public void Start(int ttl, DropType? type = null, int startCount = 0)
        {
            this.Ttl = ttl;
            this.Type = (DropType)(type == null ? (DropType)Tools.GetRandom(2) : type);

            if (startCount > 0)
            {
                for (int i = 0; i < startCount; i++)
                {
                    this.AddDrop(true);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var drop in this.drops)
            {
                drop.Draw(spriteBatch);
            }
        }

        public void Update(float deltaTime)
        {
            if (this.Active)
            {
                // add random number of drops
                for (int i = 0; i < Tools.GetRandom(3) + 2; i++)
                {
                    this.AddDrop();
                }
            }

            foreach (var drop in this.drops)
            {
                drop.Fall();

                if (drop.Y > Enums.Screen.Height)
                {
                    drop.ToDelete = true;
                }
            }

            this.drops.RemoveAll(d => d.ToDelete);

            this.Ttl -= deltaTime;
        }

        private void AddDrop(bool randomY = false)
        {
            int x = Tools.GetRandom(VillageScreen.MapWidth + Enums.Screen.Width) - Enums.Screen.Width / 2;
            int y = -10;
            if (randomY)
            {
                y = Tools.GetRandom(Enums.Screen.Height);
            }

            if (this.Type == DropType.Raining)
            {
                this.drops.Add(new Raindrop(x, y));
            }
            else
            {
                this.drops.Add(new Snowflake(x, y));
            }
        }

        public static Color GetSkyColor(DayPhase dayPhase, double dayPhaseTimer)
        {
            float progress = Sky.GetDayProgress(dayPhase, dayPhaseTimer);
            if (dayPhase == DayPhase.Day)
            {
                if (progress < 0.15f)
                {
                    // dawn
                    return MyColor.Violet;
                }
                else if (progress > 0.85f)
                {
                    // dusk
                    return MyColor.DarkViolet;
                }
                else
                {
                    // day
                    return MyColor.Blue;
                }
            }
            else
            {
                // night
                return MyColor.DarkerViolet;
            }
        }

        public static Color GetParallaxColor(DayPhase dayPhase, double dayPhaseTimer)
        {
            float progress = Sky.GetDayProgress(dayPhase, dayPhaseTimer);
            if (dayPhase == DayPhase.Day)
            {
                if (progress < 0.15f || progress > 0.85f)
                {
                    // dawn, dusk
                    return MyColor.Gray1;
                }
                else
                {
                    // day
                    return MyColor.White;
                }
            }
            else
            {
                // night
                return MyColor.Gray3;
            }
        }

        private static float GetDayProgress(DayPhase dayPhase, double dayPhaseTimer)
        {
            float currentPhaseLength = dayPhase == DayPhase.Day ? (float)DayNightLength.Day : (float)DayNightLength.Night;
            return (float)((currentPhaseLength - dayPhaseTimer) / currentPhaseLength);
        }
    }
}
