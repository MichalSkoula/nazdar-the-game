namespace SiberianAnabasis
{
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
            Night,
            Day,
        }

        public enum DayNightLength
        {
            Night = 4,
            Day = 8
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
            HoverState,
        }

        public static class Save
        {
            public const string Slot1 = "save_slot_1.json";
            public const string Slot2 = "save_slot_2.json";
            public const string Slot3 = "save_slot_3.json";
        }

        public static class Offset
        {
            public const int StatusBar = 290;
            public const int Floor = 250;

            public static int MenuX = 20;
            public static int MenuY = 20;
            public static int MenuFooter = 340;
        }
    }
}
