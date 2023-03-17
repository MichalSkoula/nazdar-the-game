using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Nazdar.Shared
{
    public static class Settings
    {
        private static FileIO settingsFile = new FileIO("settings.json");

        public static void SaveSettings(Game1 Game)
        {
            settingsFile.Save(new
            {
                fullscreen = Game.Graphics.IsFullScreen,
                musicMuted = MediaPlayer.IsMuted,
                soundsVolume = SoundEffect.MasterVolume,
                vibrations = Game1.Vibrations
            });
        }

        public static void LoadSettings(Game1 Game)
        {
            dynamic settings = settingsFile.Load();
            if (settings == null)
            {
                // EITHER set default values
                MediaPlayer.IsMuted = false;
                SoundEffect.MasterVolume = 1;
                Game1.Vibrations = true;
#if DEBUG
                Game.Graphics.IsFullScreen = false;
#else
                Game.Graphics.IsFullScreen = true;
#endif
                Game.Graphics.ApplyChanges();

                return;
            }

            // OR load values from settings file
            if (settings.ContainsKey("fullscreen") && Game.Graphics.IsFullScreen != (bool)settings.fullscreen)
            {
                Game.Graphics.IsFullScreen = !Game.Graphics.IsFullScreen;
                Game.Graphics.ApplyChanges();
            }

            if (settings.ContainsKey("musicMuted"))
            {
                MediaPlayer.IsMuted = (bool)settings.musicMuted;
            }

            if (settings.ContainsKey("soundsVolume"))
            {
                SoundEffect.MasterVolume = (int)settings.soundsVolume;
            }

            if (settings.ContainsKey("vibrations"))
            {
                Game1.Vibrations = (bool)settings.vibrations;
            }
        }

        internal static string GetPath()
        {
            return settingsFile.Folder;
        }
    }
}
