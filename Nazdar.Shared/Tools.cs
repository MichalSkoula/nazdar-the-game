using Nazdar.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Nazdar.Enums;

namespace Nazdar.Shared
{
    public static class Tools
    {
        public static Random Rand = new Random();

        public static void ButtonsIterateWithKeys(Direction direction, Dictionary<string, Button> buttons)
        {
            // count the active buttons in collection
            int activeButtonsCount = 0;
            foreach (KeyValuePair<string, Button> button in buttons)
            {
                if (button.Value.Active == true)
                {
                    activeButtonsCount++;
                }
            }

            // what button should have focus?
            bool focusNext = false;
            int i = 0;
            foreach (KeyValuePair<string, Button> button in (direction == Direction.Up) ? buttons.Reverse() : buttons)
            {
                if (button.Value.Active == false)
                {
                    continue;
                }

                if (focusNext)
                {
                    button.Value.Focus = true;
                    focusNext = false;
                    break;
                }

                if (button.Value.Focus && i < activeButtonsCount - 1)
                {
                    button.Value.Focus = false;
                    focusNext = true;
                }

                i++;
            }
        }

        public static int GetRandom(int limit)
        {
            return Rand.Next(limit);
        }

        public static bool RandomChance(int limit)
        {
            return Rand.Next(limit) == 0;
        }

        /// <summary>
        /// Rounds int to tenths
        /// </summary>
        public static int FloorOff(this int i)
        {
            int result = ((int)Math.Floor(i / 20.0)) * 20;
            return result;
        }

        public static int GetScore(int days, int money, int peasants, int soldiers, int kills, int centerLevel)
        {
            return
                days
                + money
                + peasants
                + soldiers
                + kills
                + centerLevel
            ;
        }

        public static string[] ParseSaveData(dynamic saveData)
        {
            if (saveData != null)
            {
                int score = 0;
                int village = 1;
                int days = 0;
                int money = 0;
                int kills = 0;

                // try to get score and other data from save slots
                try
                {
                    score = saveData.ContainsKey("player") ? (int)saveData.GetValue("player").BaseScore : -1;

                    score += GetScore(
                        saveData.ContainsKey("player") ? (int)saveData.GetValue("player").Days : 0,
                        saveData.ContainsKey("player") ? (int)saveData.GetValue("player").Money : 0,
                        saveData.ContainsKey("player") ? (int)saveData.GetValue("player").Kills : 0,
                        saveData.ContainsKey("peasants") ? (int)saveData.GetValue("peasants").Count : 0,
                        saveData.ContainsKey("soldiers") ? (int)saveData.GetValue("soldiers").Count : 0,
                        saveData.ContainsKey("centerLevel") ? (int)saveData.GetValue("centerLevel") : 0
                    );

                    days = saveData.GetValue("player").Days;
                    money = saveData.GetValue("player").Money;
                    village = saveData.GetValue("village");
                    kills = saveData.GetValue("player").Kills;
                }
                catch (Exception e)
                {
                    Dump(e.ToString());
                }

                return new string[] {
                    village + ". " + (Villages)village,
                    "Day " + days,
                    "Score: " + score,
                    "Money: " + money,
                    "Kills: " + kills
                };
            }

            return new string[] { " ", " ", " ", " ", " " };
        }

        /// <summary>
        /// Converts remaining time in day/night phase and returns date and time of day, in XX:XX format
        /// </summary>
        public static string DayPhaseTimerToHours(DayPhase dayPhase, double dayPhaseTimer)
        {
            int hours;
            double progressMinutes;
            if (dayPhase == DayPhase.Day)
            {
                double progress = ((double)DayNightLength.Day - dayPhaseTimer) / (double)DayNightLength.Day;
                hours = 6 + (int)Math.Floor(16 * progress); // starting at 6:00

                progressMinutes = Math.Floor(16 * progress) - 16 * progress;
            }
            else
            {
                double progress = ((double)DayNightLength.Night - dayPhaseTimer) / (double)DayNightLength.Night;
                hours = (int)Math.Floor(8 * progress);
                if (hours == 0)
                {
                    hours = 22;
                }
                else if (hours == 1)
                {
                    hours = 23;
                }
                else
                {
                    hours -= 2;
                }

                progressMinutes = Math.Floor(8 * progress) - 8 * progress;
            }

            // make minutes and convert it into quarters
            int progressInMinutes = (int)Math.Floor(Math.Abs(progressMinutes) * 60);
            if (progressInMinutes < 15)
            {
                progressInMinutes = 0;
            }
            else if (progressInMinutes < 30)
            {
                progressInMinutes = 15;
            }
            else if (progressInMinutes < 45)
            {
                progressInMinutes = 30;
            }
            else
            {
                progressInMinutes = 45;
            }

            return hours.ToString("00") + ":" + progressInMinutes.ToString("00");
        }

        public static void Dump(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

        public static void Dump(int number)
        {
            System.Diagnostics.Debug.WriteLine(number.ToString());
        }

        public static void Dump(double number)
        {
            System.Diagnostics.Debug.WriteLine(number.ToString());
        }

        public static void Dump(bool boolean)
        {
            System.Diagnostics.Debug.WriteLine(boolean.ToString());
        }

        public static async Task OpenLinkAsync(string link)
        {
            if (Game1.CurrentPlatform == Enums.Platform.GL)
            {
                Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
            }
            else if (Game1.CurrentPlatform == Enums.Platform.Android)
            {
                await AndroidOpenBrowser(new Uri(link));
            }
        }

        private static async Task AndroidOpenBrowser(Uri uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
                Tools.Dump(ex.Message);
            }
        }
    }
}
