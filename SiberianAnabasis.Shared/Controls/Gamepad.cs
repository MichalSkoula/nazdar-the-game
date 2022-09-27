using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SiberianAnabasis.Enums;

namespace SiberianAnabasis.Controls
{
    public class Gamepad
    {
        static GamePadState currentState;
        static GamePadState previousState;

        public static GamePadState GetState()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                previousState = currentState;
                currentState = GamePad.GetState(PlayerIndex.One);
                return currentState;
            }

            return new GamePadState();
        }

        public static bool IsPressed(Buttons btn)
        {
            return currentState.IsButtonDown(btn);
        }

        public static bool HasBeenPressed(Buttons btn)
        {
            return currentState.IsButtonDown(btn) && !previousState.IsButtonDown(btn);
        }

        public static bool IsPressedThumbstick(Direction direction)
        {
            if (direction == Direction.Left)
            {
                return currentState.ThumbSticks.Left.X < -0.5f;
            }
            else if (direction == Direction.Right)
            {
                return currentState.ThumbSticks.Left.X > 0.5f;
            }
            else if (direction == Direction.Up)
            {
                return currentState.ThumbSticks.Left.Y < -0.5f;
            }
            else if (direction == Direction.Down)
            {
                return currentState.ThumbSticks.Left.Y > 0.5f;
            }

            return false;
        }

        public static bool HasBeenPressedThumbstick(Direction direction)
        {
            if (direction == Direction.Left)
            {
                return currentState.ThumbSticks.Left.X < -0.5f && previousState.ThumbSticks.Left.X > -0.5f;
            }
            else if (direction == Direction.Right)
            {
                return currentState.ThumbSticks.Left.X > 0.5f && previousState.ThumbSticks.Left.X < 0.5f;
            }
            else if (direction == Direction.Up)
            {
                return currentState.ThumbSticks.Left.Y > 0.5f && previousState.ThumbSticks.Left.Y < 0.5f;
            }
            else if (direction == Direction.Down)
            {
                return currentState.ThumbSticks.Left.Y < -0.5f && previousState.ThumbSticks.Left.Y > -0.5f;
            }

            return false;
        }
    }
}
