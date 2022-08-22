using SiberianAnabasis.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Shared
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
    }
}
