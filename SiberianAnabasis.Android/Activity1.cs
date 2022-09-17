using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace SiberianAnabasis.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);

            // load some assets, only this way it works on android
            AssetManager assets = this.Assets;
            Game1.AssetStreams.Add("1_village.tmx", assets.Open("Content/Envs/1_village.tmx"));
            Game1.AssetStreams.Add("1_village.tsx", assets.Open("Content/Envs/1_village.tsx"));

            _game.Run();
        }
    }
}
