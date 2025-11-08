using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nazdar.Controls
{
    public static class TouchControls
    {
        private static Rectangle left = new Rectangle(20, 290, 44, 48);
        private static Rectangle right = new Rectangle(90, 290, 44, 48);
        private static Rectangle select = new Rectangle((Enums.Screen.Width / 2) - 22, 290, 44, 48);

        private static Rectangle jump = new Rectangle(575, 225, 44, 48);
        private static Rectangle shoot = new Rectangle(515, 290, 44, 48);
        private static Rectangle action = new Rectangle(575, 290, 44, 48);

        public static bool IsPressedLeft()
        {
            return Touch.IsPressed(left);
        }

        public static bool IsPressedRight()
        {
            return Touch.IsPressed(right);
        }

        public static bool HasBeenPressedJump()
        {
            return Touch.HasBeenPressed(jump);
        }

        public static bool HasBeenPressedAction()
        {
            return Touch.HasBeenPressed(action);
        }

        public static bool HasBeenPressedShoot()
        {
            return Touch.HasBeenPressed(shoot);
        }

        public static bool HasBeenPressedSelect()
        {
            return Touch.HasBeenPressed(select);
        }

        public static void Draw(SpriteBatch spriteBatch, float leftOffset)
        {
            // Only draw touch controls if touch input is available on the device
            if (!Touch.IsAvailable())
            {
                return;
            }

            spriteBatch.Draw(Assets.Images["GamepadLeft"], new Rectangle(left.X - (int)leftOffset, left.Y, left.Width, left.Height), Color.White * 0.75f);
            spriteBatch.Draw(Assets.Images["GamepadRight"], new Rectangle(right.X - (int)leftOffset, right.Y, right.Width, right.Height), Color.White * 0.75f);
            spriteBatch.Draw(Assets.Images["GamepadSelect"], new Rectangle(select.X - (int)leftOffset, select.Y, select.Width, select.Height), Color.White * 0.75f);

            spriteBatch.Draw(Assets.Images["GamepadY"], new Rectangle(jump.X - (int)leftOffset, jump.Y, jump.Width, jump.Height), Color.White * 0.75f);
            spriteBatch.Draw(Assets.Images["GamepadX"], new Rectangle(shoot.X - (int)leftOffset, shoot.Y, shoot.Width, shoot.Height), Color.White * 0.75f);
            spriteBatch.Draw(Assets.Images["GamepadA"], new Rectangle(action.X - (int)leftOffset, action.Y, action.Width, action.Height), Color.White * 0.75f);
        }
    }
}
