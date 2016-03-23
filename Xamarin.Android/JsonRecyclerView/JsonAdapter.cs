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

            public TextView ArtistTv { get; set; }
            public TextView TimeTv { get; set; }
            public TextView TrackIdTv { get; set; }
            public TextView TitleTv { get; set; }

            public JsonViewHolder(View view) : base(view) {
                ArtistTv = view.FindViewById<TextView>(Resource.Id.artistTv);
                TimeTv = view.FindViewById<TextView>(Resource.Id.timesTv);
                TrackIdTv = view.FindViewById<TextView>(Resource.Id.trackIdTv);
                TitleTv = view.FindViewById<TextView>(Resource.Id.titleTv);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);
            return new JsonViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Song element = Songs[position];
            (holder as JsonViewHolder).ArtistTv.Text = element.Artist;
            (holder as JsonViewHolder).TimeTv.Text = element.SongDate.ToString();
            (holder as JsonViewHolder).TrackIdTv.Text = element.TrackId;
            (holder as JsonViewHolder).TitleTv.Text = element.Title;
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

