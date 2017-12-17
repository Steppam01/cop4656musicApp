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
        MediaRecorder mediaRecorder;
        bool isPaused = false;
        bool isStopped = false;
        bool isRecording = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Song);

            Songs songs = new Songs();
            int songIndex = Intent.GetIntExtra("songIndex", 0);
            string song = songs.GetSong(songIndex);
            StartSong(song);

            ImageButton backButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.back);
            ImageButton prevButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.prevSong);
            ImageButton playButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.play);
            ImageButton nextButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.nextSong);
            ImageButton stopButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.stop);
            ImageButton pauseButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.pause);
            ImageButton recordButton = (ImageButton)FindViewById<ImageButton>(Resource.Id.record);

            prevButton.LongClick += delegate
            {
                StartSong(songs.GetSong(0));
            };

            prevButton.Click += delegate
            {
                if (songIndex != 0)
                {
                    song = songs.GetSong(songIndex - 1);
                    songIndex -= 1;
                    StartSong(song);
                }
            };

            playButton.Click += delegate
            {
                if(isPaused) mediaPlayer.Start();
                if(isStopped) StartSong(songs.GetSong(songIndex));
                isPaused = false;
                isStopped = false;
            };

            nextButton.Click += delegate
            {
                if (songIndex != 2)
                {
                    song = songs.GetSong(songIndex + 1);
                    songIndex += 1;
                    StartSong(song);
                }
            };

            nextButton.LongClick += delegate
            {
                StartSong(songs.GetSong(2));
            };

            stopButton.Click += delegate
            {
                if (mediaPlayer.IsPlaying) mediaPlayer.Stop();
                isStopped = true;
            };

            pauseButton.Click += delegate
            {
                if (mediaPlayer.IsPlaying) mediaPlayer.Pause();
                isPaused = true;
            };

            recordButton.Click += delegate
            {
                if (mediaPlayer.IsPlaying)
                {
                    if (!isRecording)
                    {
                        recordButton.BackgroundTintList = GetColorStateList(Resource.Color.red);
                        mediaRecorder = new MediaRecorder();
                        isRecording = true;
                        mediaRecorder.SetAudioSource(AudioSource.Mic);
                        mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
                        mediaRecorder.SetOutputFile(Android.OS.Environment.ExternalStorageDirectory + "/" + song + ".3gp");
                        mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
                        mediaRecorder.Prepare();
                        mediaRecorder.Start();
                    } else
                    {
                        isRecording = false;

                        try
                        {
                            mediaRecorder.Stop();
                        } catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        mediaRecorder.Release();
                        mediaRecorder = null;
                    }
                }
            };

            backButton.Click += delegate
            {
                mediaPlayer.Release();
                StartActivity(new Intent(this, typeof(MainActivity)));
            };

        }

        private void StartSong(string name)
        {
            SetArtworkAndTitle(name);
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

        private void SetArtworkAndTitle(string song)
        {
            ImageView artwork = (ImageView)FindViewById<ImageView>(Resource.Id.artwork);
            TextView songTitle = (TextView)FindViewById<TextView>(Resource.Id.songTitle);

            switch (song)
            {
                case "bensound_acousticbreeze":
                    artwork.SetImageResource(Resource.Drawable.acousticbreeze);
                    songTitle.Text = "Acoustic Breeze";
                    break;
                case "bensound_happiness":
                    artwork.SetImageResource(Resource.Drawable.happiness);
                    songTitle.Text = "Happiness";
                    break;
                case "bensound_sweet":
                    artwork.SetImageResource(Resource.Drawable.sweet);
                    songTitle.Text = "Sweet";
                    break;
            }
        }
    }
}