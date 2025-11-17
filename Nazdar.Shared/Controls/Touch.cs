using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Nazdar.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nazdar.Controls
{
    internal class Touch
    {
        private static List<TouchLocation> previousTouchLocations = new List<TouchLocation>();
        private static readonly List<TouchLocation> currentTouchLocations = new List<TouchLocation>();

        // true when runtime reports touch capability / touches detected in game area
        public static bool Available { get; private set; } = false;

        // once we detect capabilities keep them
        private static bool capabilityProbed = false;

        public static void GetState(bool isActive)
        {
            // create shallow copy of current touches for previous
            previousTouchLocations = new List<TouchLocation>(currentTouchLocations);
            currentTouchLocations.Clear();

            if (!isActive)
            {
                // don't change Available here, keep previously detected capability
                return;
            }

            // probe platform capability once (reflection to avoid API differences)
            if (!capabilityProbed)
            {
                capabilityProbed = true;
                try
                {
                    MethodInfo getCaps = typeof(TouchPanel).GetMethod("GetCapabilities", BindingFlags.Public | BindingFlags.Static);
                    if (getCaps != null)
                    {
                        var caps = getCaps.Invoke(null, null);
                        if (caps != null)
                        {
                            // try common property names
                            PropertyInfo prop = caps.GetType().GetProperty("IsConnected")
                                ?? caps.GetType().GetProperty("IsAvailable")
                                ?? caps.GetType().GetProperty("TouchPresent")
                                ?? caps.GetType().GetProperty("IsPresent")
                                ?? caps.GetType().GetProperty("TouchCount");

                            if (prop != null)
                            {
                                var val = prop.GetValue(caps);
                                if (val is bool b && b)
                                {
                                    Available = true;
                                }
                                else if (val is int i && i > 0)
                                {
                                    Available = true;
                                }
                            }

                            // fallback: if any boolean property is true
                            if (!Available)
                            {
                                var anyTrue = caps.GetType().GetProperties()
                                    .Where(p => p.PropertyType == typeof(bool))
                                    .Any(p => (bool)p.GetValue(caps));
                                if (anyTrue)
                                {
                                    Available = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignore probing errors, we'll fallback to runtime touch detection
                }
            }

            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State != TouchLocationState.Invalid)
                {
                    currentTouchLocations.Add(touch);
                }
            }

            // diagnostic: show how many touches detected
            Tools.Dump("Touch count: " + currentTouchLocations.Count);

            // Determine if any of the touches are actually inside the game's internal resolution
            bool hasInGameTouch = currentTouchLocations.Any(t =>
            {
                // map to internal coordinates
                int posX = (int)((t.Position.X - Game1.BarWidth) / Game1.Scale);
                int posY = (int)((t.Position.Y - Game1.BarHeight) / Game1.Scale);

                bool inBounds = posX >= 0 && posX <= Enums.Screen.Width && posY >= 0 && posY <= Enums.Screen.Height;
                bool meaningfulState = t.State == TouchLocationState.Pressed || t.State == TouchLocationState.Moved || t.State == TouchLocationState.Released;
                return inBounds && meaningfulState;
            });

            // mark Available if we detected an in-game touch this frame OR the probe already found capabilities
            if (hasInGameTouch)
            {
                Available = true;
            }
        }

        private static Point CalculatePosition(TouchLocation touch)
        {
            // normalize position into internal resolution using current bars/scale
            int posX = (int)((touch.Position.X - Game1.BarWidth) / Game1.Scale);
            int posY = (int)((touch.Position.Y - Game1.BarHeight) / Game1.Scale);
            //Tools.Dump(posX + " " + posY);
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
                    // we can say, well, yes, it has been pressed now!!
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
                    // we can say, well, yes, it has been pressed now!!
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
                    //Tools.Dump("Touch hit hitbox at: " + pos.X + "," + pos.Y);
                    return true;
                }
            }

            return false;
        }
    }
}
