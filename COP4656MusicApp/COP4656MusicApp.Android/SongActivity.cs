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
using Android.Media;

namespace COP4656MusicApp.Droid
{
    [Activity(Label = "SongActivity")]
    public class SongActivity : Activity
    {
        MediaPlayer mediaPlayer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Song);

            //Helper class with the names of the mp3s
            Songs songs = new Songs();

            //These variables store the current song and songIndex
            string song = Intent.GetStringExtra("songTitle");
            int songIndex = Intent.GetIntExtra("songIndex", 0);
            StartSong(song);

            //Getting a hold of the image buttons. Get the rest of them
            ImageButton backButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.back);
            ImageButton prevButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.prevSong);

            //For the media controls, create the rest of the delegates for the image buttons
            //Here's the previous song button, for example
            prevButton.Click += delegate
            {
                if (songIndex != 0)
                {
                    //Make sure to save into the variables upon a button clicked to keep track of the song
                    song = songs.GetSong(songIndex - 1);
                    songIndex -= 1;
                    SetArtwork(song);
                    StartSong(song);
                }
            };

            //For the back button, just go back to the main activity. If you want to be fancy, you can pass the media player so that the
            //song is still playing.
            backButton.Click += delegate {

            };

        }

        private void StartSong(string name)
        {
            if (mediaPlayer != null)
            {
                if (mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Reset();
                }
            }

            var resourceId = Resources.GetIdentifier(name, "raw", PackageName);
            mediaPlayer = MediaPlayer.Create(this, resourceId);

            mediaPlayer.Completion += delegate
            {
                mediaPlayer = null;
            };

            mediaPlayer.Start();
        }

        //Finish this
        private void SetArtwork(string song)
        {
            ImageView artwork = (ImageView)FindViewById<ImageView>(Resource.Id.artwork);

            if (song == "bensound_acousticbreeze")
            {
                artwork.SetImageResource(Resource.Drawable.acousticbreeze);
            }
        }
    }
}