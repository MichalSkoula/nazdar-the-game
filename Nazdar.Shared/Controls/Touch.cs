using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Nazdar.Shared;
using System.Collections.Generic;

namespace Nazdar.Controls
{
    class Touch
    {
        private static List<TouchLocation> previousTouchLocations = new List<TouchLocation>();
        private static List<TouchLocation> currentTouchLocations = new List<TouchLocation>();

        public static void GetState(bool isActive)
        {
            previousTouchLocations = currentTouchLocations.GetRange(0, previousTouchLocations.Count); // create shallow copy
            currentTouchLocations.Clear();

            if (!isActive)
            {
                return;
            }

            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State != TouchLocationState.Invalid)
                {
                    currentTouchLocations.Add(touch);
                }
            }
        }

        private static Point CalculatePosition(TouchLocation touch)
        {
            // Touch is only for android .. so we put there barwidth or barheight for android
            int posX = (int)((touch.Position.X - Game1.BarWidth) / Game1.Scale);
            int posY = (int)((touch.Position.Y - Game1.BarHeight) / Game1.Scale);
            Tools.Dump(posX + " " + posY);
            return new Point(posX, posY);
        }

        public static bool HasBeenPressed(Rectangle hitbox)
        {
            if (Game1.CurrentPlatform != Enums.Platform.Android)
            {
                return false;
            }

            // if hitbox was not "released" previous iteration by any finger, go on
            foreach (var touch in previousTouchLocations)
            {
                if (hitbox.Contains(CalculatePosition(touch)) && touch.State == TouchLocationState.Released)
                {
                    return false;
                }
            }

            // and now it is released... by any finger
            foreach (var touch in currentTouchLocations)
            {
                if (hitbox.Contains(CalculatePosition(touch)) && touch.State == TouchLocationState.Released)
                {
                    // we can say, well, yes, it has been pressed now!!
                    return true;
                }
            }

            return false;
        }

        public static bool IsPressed(Rectangle hitbox)
        {
            if (Game1.CurrentPlatform != Enums.Platform.Android)
            {
                return false;
            }

            // at least one finger is touching it
            foreach (var touch in currentTouchLocations)
            {
                if (hitbox.Contains(CalculatePosition(touch)) && (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved))
                {
                    return true;
                }
            }

            return false;
        }
    }
}