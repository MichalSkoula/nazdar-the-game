using Nazdar.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }

        /// <summary>
        /// Rounds int to tenths
        /// </summary>
        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
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
                        (saveData.ContainsKey("center") && saveData.GetValue("center") != null) ? (int)saveData.GetValue("center").Level : 0
                    );

                    days = saveData.GetValue("player").Days;
                    money = saveData.GetValue("player").Money;
                    village = saveData.GetValue("village");

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
                };
            }

            return new string[] { " ", " ", " ", " " };
        }

        public static void Dump(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

        public static void Dump(int number)
        {
            System.Diagnostics.Debug.WriteLine(number.ToString());
        }
    }
}
