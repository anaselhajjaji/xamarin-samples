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
using Com.Lilarcor.Cheeseknife;

namespace JsonRecyclerView
{
    public class JsonAdapter : RecyclerView.Adapter
    {
        /// <summary>
        /// Gets or sets the songs.
        /// </summary>
        /// <value>The songs.</value>
        public List<Song> Songs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRecyclerView.JsonAdapter"/> class.
        /// </summary>
        /// <param name="songs">Songs.</param>
        public JsonAdapter(List<Song> songs)
        {
            Songs = songs;
        }

        /// <summary>
        /// The View Holder
        /// </summary>
        public class JsonViewHolder : RecyclerView.ViewHolder {

            [InjectView(Resource.Id.artistTv)]
            private TextView artistTv;

            [InjectView(Resource.Id.timesTv)]
            private TextView timeTv;

            [InjectView(Resource.Id.trackIdTv)]
            private TextView trackIdTv;

            [InjectView(Resource.Id.titleTv)]
            private TextView titleTv;

            [InjectView(Resource.Id.songImage)]
            private ImageView songImage;

            /// <summary>
            /// Initializes a new instance of the <see cref="JsonRecyclerView.JsonAdapter+JsonViewHolder"/> class.
            /// </summary>
            /// <param name="view">View.</param>
            public JsonViewHolder(View view) : base(view) {
                Cheeseknife.Inject(this, view);
            }

            /// <summary>
            /// Binds the view holder.
            /// </summary>
            /// <param name="song">Song.</param>
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

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount
        {
            get
            {
                return Songs.Count;
            }
        }
    }
}

