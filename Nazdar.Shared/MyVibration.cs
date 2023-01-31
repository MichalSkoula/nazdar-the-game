using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Nazdar.Shared
{
    public static class MyVibration
    {
        public static void Vibrate(int ms = 250)
        {
            if (Game1.Vibrations == false)
            {
                return;
            }

            if (Game1.CurrentPlatform == Enums.Platform.Android)
            {
                // mobile, easy
                try
                {
                    Vibration.Vibrate(ms);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                // gamepad
                GamepadVibrate(ms);
            }
        }

        private static async void GamepadVibrate(int ms)
        {
            GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
            await Task.Delay(ms);
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }
    }
}
