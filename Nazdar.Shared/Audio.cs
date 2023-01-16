using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;

namespace Nazdar.Shared
{
    public static class Audio
    {
        public static string CurrentSongCollection { get; set; } = null;
        public static float SongVolume { get; set; } = 0.5f;

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
            MediaPlayer.Volume = SongVolume;
            if (CurrentSongCollection != null && MediaPlayer.State != MediaState.Playing/* && MediaPlayer.PlayPosition.TotalSeconds == 0.0f && Assets.SongsCollection.ContainsKey(CurrentSongCollection)*/)
            {
                MediaPlayer.Play(Assets.SongsCollection[CurrentSongCollection][Tools.GetRandom(Assets.SongsCollection[CurrentSongCollection].Count)]);
            }
        }

        public static async void SongTransition(float? finalVolume, string songCollection)
        {
            // this collection is already playing
            if (CurrentSongCollection == songCollection)
            {
                // maybe just adjust volume, nothing else
                if (finalVolume != null)
                {
                    SongVolume = (float)finalVolume;
                }
                return;
            }

            // do not change volume
            if (finalVolume == null)
            {
                finalVolume = SongVolume;
            }

            float step = 0.01f;

            // change queue
            CurrentSongCollection = songCollection;

            // down
            while (SongVolume > step)
            {
                SongVolume -= step;
                await Task.Delay(10);
            }

            // change = stop old track
            MediaPlayer.Stop();

            // up
            while (SongVolume < finalVolume)
            {
                SongVolume += step;
                await Task.Delay(10);
            }
        }
    }
}
