using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Graphics;
using Square.Picasso;

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
            private ImageView songImage;

            public JsonViewHolder(View view) : base(view) {
                // Retrieve components
                artistTv = view.FindViewById<TextView>(Resource.Id.artistTv);
                timeTv = view.FindViewById<TextView>(Resource.Id.timesTv);
                trackIdTv = view.FindViewById<TextView>(Resource.Id.trackIdTv);
                titleTv = view.FindViewById<TextView>(Resource.Id.titleTv);
                songImage = view.FindViewById<ImageView>(Resource.Id.songImage);
            }

            public void BindViewHolder(Song song) {
                artistTv.Text = song.Artist;
                timeTv.Text = song.SongDate.ToString();
                trackIdTv.Text = song.TrackId;
                titleTv.Text = song.Title;

                // Download image
                Picasso.With(titleTv.Context)
                    .Load(song.TrackImage)
                    .Placeholder(Android.Resource.Drawable.IcMenuRotate)
                    .Error(Android.Resource.Drawable.IcMenuCamera)
                    .Into(songImage);
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

