using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace COP4656MusicApp.Droid
{
    class Songs
    {
        private string[] songs = new string[]
        {
            "bensound_acousticbreeze",
            "bensound_happiness",
            "bensound_sweet"
        };

        public string[] GetSongs()
        {
            return songs;
        }

        public string GetSong(int position)
        {
            return songs[position];
        }
    }
}