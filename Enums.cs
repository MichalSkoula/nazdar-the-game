namespace MyGame
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Enums
    {
        public enum Direction
        {
            Up, // 0
            Right, // 1
            Down, // 2
            Left, // 3
        }

        public enum DayPhase
        {
            Night, // 0
            Day, // 1
        }

        public enum ButtonSize
        {
            Small = 15,
            Medium = 22,
            Large = 30,
        }

        public enum ButtonState
        {
            StaticState,
            ClickedState,
            HoverState,
        }
    }
}
