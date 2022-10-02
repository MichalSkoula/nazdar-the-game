using Microsoft.Xna.Framework.Media;

namespace Nazdar.Shared
{
    public static class Audio
    {
        public static string CurrentSongCollection { get; set; } = null;

        public static void PlaySound(string soundName)
        {
            if (!Assets.Sounds.ContainsKey(soundName))
            {
                return;
            }

            Assets.Sounds[soundName].Play();
        }

        public static void PlayRandomSound(string collectionName)
        {
            if (!Assets.SoundsCollection.ContainsKey(collectionName))
            {
                return;
            }

            Assets.SoundsCollection[collectionName][Tools.GetRandom(Assets.SoundsCollection[collectionName].Count)].Play();
        }

        // check if there is a song collection to play; if so, play it
        public static void PlaySongCollection()
        {
            if (CurrentSongCollection != null && MediaPlayer.State != MediaState.Playing/* && MediaPlayer.PlayPosition.TotalSeconds == 0.0f && Assets.SongsCollection.ContainsKey(CurrentSongCollection)*/)
            {
                MediaPlayer.Play(Assets.SongsCollection[CurrentSongCollection][Tools.GetRandom(Assets.SongsCollection[CurrentSongCollection].Count)]);
            }
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
