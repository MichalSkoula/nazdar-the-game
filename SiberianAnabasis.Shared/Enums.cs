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
            Night = 8,
            Day = 16
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

        public enum PlayerAction
        {
            Hire,
            Build,
            Create
        }

        public enum BulletType
        {
            Bullet,
            Cannonball
        }

        public enum MessageType
        {
            Info,
            Success,
            Fail,
            Danger,
            Opportunity
        }

        public static class Building
        {
            public enum Type
            {
                Center,
                Armory,
                Tower,
            }

            public enum Status
            {
                InProcess,
                Built
            }
        }

        public static class Save
        {
            public const string Slot1 = "save_slot_1.json";
            public const string Slot2 = "save_slot_2.json";
            public const string Slot3 = "save_slot_3.json";
        }

        public static class Offset
        {
            public const int StatusBarY = 290;
            public const int StatusBarX = 15;
            public const int Floor = 252;
            public const int Floor2 = 260;
            public const int MessagesX = Screen.Width - 250;

            public static int MenuX = 20;
            public static int MenuY = 20;
            public static int MenuFooter = 340;
        }

        public static class Screen
        {
            // default window size
            public const int WidthDefault = 1280;
            public const int HeightDefault = 720;

            // internal screen resolution
            public const int Width = 640;
            public const int Height = 360;
        }
    }
}
