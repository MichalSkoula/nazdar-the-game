using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Nazdar.Shared;
using System.Collections.Generic;

namespace Nazdar.Controls
{
    internal class Touch
    {
        private static List<TouchLocation> previousTouchLocations = new List<TouchLocation>();
        private static readonly List<TouchLocation> currentTouchLocations = new List<TouchLocation>();
        private static bool touchDetected = false;

        public static bool IsAvailable()
        {
            // Check if touch has been detected at any point during this session
            // This is more reliable than TouchPanel.GetCapabilities().IsConnected
            // which may not work correctly on all platforms
            return touchDetected || TouchPanel.GetCapabilities().IsConnected;
        }

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
                    // Mark that we've detected touch input at least once
                    touchDetected = true;
                }
            }
        }

        private static Point CalculatePosition(TouchLocation touch)
        {
            // Calculate touch position accounting for screen bars and scaling
            int posX = (int)((touch.Position.X - Game1.BarWidth) / Game1.Scale);
            int posY = (int)((touch.Position.Y - Game1.BarHeight) / Game1.Scale);
            Tools.Dump(posX + " " + posY);
            return new Point(posX, posY);
        }

        public static bool HasBeenPressed(Rectangle hitbox)
        {
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

        public static bool HasBeenPressedAnywhere()
        {
            // if hitbox was not "released" previous iteration by any finger, go on
            foreach (var touch in previousTouchLocations)
            {
                if (touch.State == TouchLocationState.Released)
                {
                    return false;
                }
            }

            // and now it is released... by any finger
            foreach (var touch in currentTouchLocations)
            {
                if (touch.State == TouchLocationState.Released)
                {
                    // we can say, well, yes, it has been pressed now!!
                    return true;
                }
            }

            return false;
        }

        public static bool IsPressed(Rectangle hitbox)
        {
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