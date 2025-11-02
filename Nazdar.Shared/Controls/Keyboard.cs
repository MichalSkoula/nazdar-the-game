using Microsoft.Xna.Framework.Input;

namespace Nazdar.Controls
{
    public class Keyboard
    {
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        public static KeyboardState GetState(bool isActive)
        {
            previousKeyState = currentKeyState;
            currentKeyState = isActive ? Microsoft.Xna.Framework.Input.Keyboard.GetState() : new KeyboardState();
            return currentKeyState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
    }
}
