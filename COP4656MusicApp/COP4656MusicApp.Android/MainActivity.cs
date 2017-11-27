using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

namespace COP4656MusicApp.Droid
{
	[Activity (Label = "COP4656MusicApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            Songs songs = new Songs();
            var listView = FindViewById<ListView>(Resource.Id.songsList);
            var adapter = new ArrayAdapter(this, Resource.Layout.CustomListView, Android.Resource.Id.Text1, songs.GetSongs());

            listView.Adapter = adapter;

            listView.ItemClick += (sender, e) =>
            {
                var songActivity = new Intent(this, typeof(SongActivity));
                songActivity.PutExtra("songTitle", songs.GetSong(e.Position));
                songActivity.PutExtra("songIndex", e.Position);
                StartActivity(songActivity);
            };
		}
	}
}


