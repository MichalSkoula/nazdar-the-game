using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SiberianAnabasis.Controls
{
    public static class TouchControls
    {
        private static Rectangle left = new Rectangle(30, 110, 44, 48);
        private static Rectangle right = new Rectangle(100, 110, 44, 48);
        private static Rectangle select = new Rectangle(Enums.Screen.Width / 2 - 22, 300, 44, 48);

        private static Rectangle jump = new Rectangle(550, 170, 44, 48);
        private static Rectangle action = new Rectangle(550, 50, 44, 48);
        private static Rectangle shoot = new Rectangle(490, 110, 44, 48);

        public static bool IsPressedLeft()
        {
            return Touch.IsPressed() && left.Contains(Touch.Position);
        }

        public static bool IsPressedRight()
        {
            return Touch.IsPressed() && right.Contains(Touch.Position);
        }

        public static bool HasBeenPressedJump()
        {
            return Touch.HasBeenPressed() && jump.Contains(Touch.Position);
        }

        public static bool HasBeenPressedAction()
        {
            return Touch.HasBeenPressed() && action.Contains(Touch.Position);
        }

        public static bool HasBeenPressedShoot()
        {
            return Touch.HasBeenPressed() && shoot.Contains(Touch.Position);
        }

        public static bool HasBeenPressedSelect()
        {
            return Touch.HasBeenPressed() && select.Contains(Touch.Position);
        }

        public static void Draw(SpriteBatch spriteBatch, float leftOffset)
        {
            if (Game1.CurrentPlatform != Enums.Platform.Android)
            {
                return;
            }

            spriteBatch.Draw(Assets.Images["GamepadLeft"], new Rectangle(left.X - (int)leftOffset, left.Y, left.Width, left.Height), Color.White * 0.7f);
            spriteBatch.Draw(Assets.Images["GamepadRight"], new Rectangle(right.X - (int)leftOffset, right.Y, right.Width, right.Height), Color.White * 0.7f);
            spriteBatch.Draw(Assets.Images["GamepadSelect"], new Rectangle(select.X - (int)leftOffset, select.Y, select.Width, select.Height), Color.White * 0.7f);

            spriteBatch.Draw(Assets.Images["GamepadA"], new Rectangle(jump.X - (int)leftOffset, jump.Y, jump.Width, jump.Height), Color.White * 0.7f);
            spriteBatch.Draw(Assets.Images["GamepadY"], new Rectangle(action.X - (int)leftOffset, action.Y, action.Width, action.Height), Color.White * 0.7f);
            spriteBatch.Draw(Assets.Images["GamepadX"], new Rectangle(shoot.X - (int)leftOffset, shoot.Y, shoot.Width, shoot.Height), Color.White * 0.7f);
        }
    }
}
