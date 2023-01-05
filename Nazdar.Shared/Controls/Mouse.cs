using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nazdar.Controls
{
    class Mouse
    {
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static Point Position { get; set; }

        public static MouseState GetState(bool isActive)
        {
            if (Game1.DisableMouse)
            {
                return currentMouseState;
            }

            previousMouseState = currentMouseState;
            currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            // deal with renderTarget scale and bars
            int posX = isActive ? (int)((currentMouseState.Position.X - Game1.BarWidth) / Game1.Scale) : -999;
            int posY = isActive ? (int)((currentMouseState.Position.Y - Game1.BarHeight) / Game1.Scale) : -999;
            Point newPosition = new Point(posX, posY);
            if (HasBeenPressed())
            {
                System.Diagnostics.Debug.WriteLine(posX + " " + posY);
            }
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