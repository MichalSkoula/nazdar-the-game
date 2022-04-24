using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SiberianAnabasis.Controls
{
    class Mouse
    {
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static Point Position { get; set; }

        public static MouseState GetState()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            // deal with renderTarget scale and bars
            Point newPosition = new Point((int)((currentMouseState.Position.X - Game1.BarWidth) / Game1.Scale), (int)((currentMouseState.Position.Y - Game1.BarHeight) / Game1.Scale));
            // System.Diagnostics.Debug.WriteLine(((currentMouseState.Position.X - Game1.BarWidth) / Game1.Scale) + " " + ((currentMouseState.Position.Y - Game1.BarHeight) / Game1.Scale));
            Position = newPosition;

            return currentMouseState;
        }

        public static bool IsPressed(bool left = true)
        {
            if (left)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                return currentMouseState.RightButton == ButtonState.Pressed;
            }
        }

        public static bool HasBeenPressed(bool left = true)
        {
            if (left)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;
            }
            else
            {
                return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed;
            }
        }
    }
}