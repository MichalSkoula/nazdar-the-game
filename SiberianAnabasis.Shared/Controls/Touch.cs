using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace SiberianAnabasis.Controls
{
    class Touch
    {
        private static TouchLocation previousTouchLocation;
        private static TouchLocation currentTouchLocation = new TouchLocation();

        public static Point Position { get; set; }

        public static TouchLocation GetState()
        {
            previousTouchLocation = currentTouchLocation;
            currentTouchLocation = new TouchLocation();

            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                if (touchCollection[0].State == TouchLocationState.Released)
                {
                    currentTouchLocation = touchCollection[0];
                }
            }

            // deal with renderTarget scale and bars
            // Touch is only for android .. so we put there barwidth or barheight for android
            int posX = (int)((currentTouchLocation.Position.X + Game1.BarWidthAndroid) / Game1.Scale);
            int posY = (int)((currentTouchLocation.Position.Y + Game1.BarHeightAndroid) / Game1.Scale);
            Point newPosition = new Point(posX, posY);
            if (HasBeenPressed())
            {
                System.Diagnostics.Debug.WriteLine(posX + " " + posY);
            }
            Position = newPosition;

            return currentTouchLocation;
        }

        public static bool HasBeenPressed()
        {
            return currentTouchLocation.State == TouchLocationState.Released && previousTouchLocation.State != TouchLocationState.Released;
        }
    }
}