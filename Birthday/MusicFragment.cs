using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Birthday
{
    public class MusicFragment : Fragment
    {
        private ListView music;
        private List<Music> musicList;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_music, container, false);

            music = view.FindViewById<ListView>(Resource.Id.music_list);

            musicList = new List<Music>();
            foreach (Music musicType in Enum.GetValues(typeof(Music)))
                musicList.Add(musicType);

            music.Adapter = new ArrayAdapter(Context, Resource.Layout.support_simple_spinner_dropdown_item, musicList);

            music.ItemClick += Music_ItemClick;

            return view;
        }

        private void Music_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            MusicSelected?.Invoke(music.GetItemAtPosition(e.Position), new EventArgs());
        }

        public event EventHandler MusicSelected;
    }
}