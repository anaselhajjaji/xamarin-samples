using System;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;

namespace JsonRecyclerView
{
    public class JsonAdapter : RecyclerView.Adapter
    {
        public List<Song> Songs { get; set; }

        public JsonAdapter(List<Song> songs)
        {
            Songs = songs;
        }

        public class JsonViewHolder : RecyclerView.ViewHolder {

            private TextView artistTv;
            private TextView timeTv;
            private TextView trackIdTv;
            private TextView titleTv;

            public JsonViewHolder(View view) : base(view) {
                artistTv = view.FindViewById<TextView>(Resource.Id.artistTv);
                timeTv = view.FindViewById<TextView>(Resource.Id.timesTv);
                trackIdTv = view.FindViewById<TextView>(Resource.Id.trackIdTv);
                titleTv = view.FindViewById<TextView>(Resource.Id.titleTv);
            }

            public void BindViewHolder(Song song) {
                artistTv.Text = song.Artist;
                timeTv.Text = song.SongDate.ToString();
                trackIdTv.Text = song.TrackId;
                titleTv.Text = song.Title;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);
            return new JsonViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Song song = Songs[position];
            (holder as JsonViewHolder).BindViewHolder(song);
        }

        public override int ItemCount
        {
            get
            {
                return Songs.Count;
            }
        }
    }
}

