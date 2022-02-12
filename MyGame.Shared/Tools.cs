using MyGame.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MyGame.Enums;

namespace MyGame.Shared
{
    public static class Tools
    {
        public static void ButtonsIterateWithKeys(Direction direction, Dictionary<string, Button> buttons)
        {
            bool focusNext = false;
            int i = 0;
            foreach (KeyValuePair<string, Button> button in (direction == Direction.Up) ? buttons.Reverse() : buttons)
            {
                if (focusNext)
                {
                    button.Value.Focus = true;
                    focusNext = false;
                    break;
                }

                if (button.Value.Focus && i < buttons.Count - 1)
                {
                    button.Value.Focus = false;
                    focusNext = true;
                }

                i++;
            }
        }
    }
}
