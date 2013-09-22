using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace AITW
{
    class Helpers
    {

        public bool _musicPlayed;

        public int GetRandom(int min, int max)
        {
            var realRandom = new Random();
            var intRandom = realRandom.Next(min, max);
            return intRandom;
        }

        public void PlayMusic(Song song, bool isRepeating = true, float volume = 0.3F)
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3F;
            if (isRepeating)
            {
                _musicPlayed = true;
            }

        }
    }
}
