﻿using Microsoft.Xna.Framework.Input;

namespace Nazdar
{
    public static class Enums
    {
        public enum Direction
        {
            Up,     // 0
            Right,  // 1
            Down,   // 2
            Left,   // 3
        }

        public enum DayPhase
        {
            Night,
            Day,
        }

        public enum DayNightLength
        {
            Night = 32,
            Day = 64
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
            Create,
            Upgrade,
            Buy,
            Repair
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
            Opportunity,
            Super
        }

        // ControlKeys for touchscreen are defined in TouchControl.cs
        public static class ControlKeys
        {
            public const Keys Left = Keys.Left;
            public const Keys Right = Keys.Right;
            public const Keys Jump = Keys.Up;
            public const Keys Action = Keys.Down;
            public const Keys Shoot = Keys.Space;
        }

        public static class ControlButtons
        {
            public const Buttons Jump = Buttons.Y;
            public const Buttons Action = Buttons.A;
            public const Buttons Shoot = Buttons.X;
        }

        public static class Building
        {
            public enum Type
            {
                Center,
                Armory,
                Tower,
                Farm,
                Arsenal,
                Slum,       // cant be build
                Hospital,
                Locomotive,
                Market,
                Rails,      // when "built", it "disappears"
                Treasure    // is always built
            }

            public enum Status
            {
                InProcess,
                Built,
                Finished // only locomotive...to next level
            }
        }

        public static class Save
        {
            public const string Slot1 = "save_slot_1.json";
            public const string Slot2 = "save_slot_2.json";
            public const string Slot3 = "save_slot_3.json";
            public const string Survival = "survival.json";
        }

        public static class Offset
        {
            public const int StatusBarY = 225;
            public const int StatusBarX = 15;
            public const int Floor = 172; // normal people
            public const int Floor2 = 180;
            public const int FloorPig = 160;
            public const int FloorLenin = 160 - 16;
            public const int MessagesX = Screen.Width - 310;
            public const int SuperMessageY = 100;

            public static int MenuX = 20;
            public static int MenuY = 20;
            public static int MenuFooter = 338;

            public static int RowLimit = 16;

            public static int MapTilesOffset = 20 * 16;
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

        public enum Platform
        {
            GL,
            UWP,
            Android
        }

        public enum Villages
        {
            Survival = 0,
            Chelyabinsk = 1,
            Omsk = 2,
            Pemza = 3,
            Lipjag = 4,
            Ufa = 5,
            Yekaterinburg = 6,
            Kazan = 7,
            Vladivostok = 8
        }

        public enum DropType
        {
            Raining,
            Snowing
        }

        public enum WeatherType
        {
            Grass,
            Snow
        }
    }
}
