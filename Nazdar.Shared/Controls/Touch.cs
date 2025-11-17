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

        // true when touch is available
        public static bool Available { get; private set; } = false;

        public static void GetState(bool isActive)
        {
            // create shallow copy of current touches for previous
            previousTouchLocations = new List<TouchLocation>(currentTouchLocations);
            currentTouchLocations.Clear();

            // Check if touch is supported on this device
            var capabilities = TouchPanel.GetCapabilities();
            if (capabilities.IsConnected)
            {
                Available = true;
            }

            // Always collect touch state regardless of isActive
            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State != TouchLocationState.Invalid)
                {
                    currentTouchLocations.Add(touch);
                }
            }

            // If we detect any touches, mark as available
            if (currentTouchLocations.Count > 0)
            {
                Available = true;
                Tools.Dump("Touch count: " + currentTouchLocations.Count);
            }
        }

        private static Point CalculatePosition(TouchLocation touch)
        {
            // normalize position into internal resolution using current bars/scale
            int posX = (int)((touch.Position.X - Game1.BarWidth) / Game1.Scale);
            int posY = (int)((touch.Position.Y - Game1.BarHeight) / Game1.Scale);
            return new Point(posX, posY);
        }

        public static bool HasBeenPressed(Rectangle hitbox)
        {
            if (!Available)
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
                    return true;
                }
            }

            return false;
        }

        public static bool HasBeenPressedAnywhere()
        {
            if (!Available)
            {
                return false;
            }

            // if any previous touch was released, ignore (debounce)
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
                    return true;
                }
            }

            return false;
        }

        public static bool IsPressed(Rectangle hitbox)
        {
            if (!Available)
            {
                return false;
            }

            // at least one finger is touching it
            foreach (var touch in currentTouchLocations)
            {
                var pos = CalculatePosition(touch);
                if (hitbox.Contains(pos) && (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
