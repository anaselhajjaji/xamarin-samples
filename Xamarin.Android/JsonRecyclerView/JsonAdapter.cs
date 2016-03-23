using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Graphics;

namespace JsonRecyclerView
{
    public class JsonAdapter : RecyclerView.Adapter
    {
        public List<Song> Songs { get; set; }
        private WorkThread workThread;

        public JsonAdapter(List<Song> songs)
        {
            Songs = songs;
            workThread = new WorkThread("Image download...", 10);
            workThread.Start();
            workThread.PrepareHandler();
        }

        public class JsonViewHolder : RecyclerView.ViewHolder {

            private string songId;
            private TextView artistTv;
            private TextView timeTv;
            private TextView trackIdTv;
            private TextView titleTv;
            private ImageView songImage;
            private Handler uiHandler = new Handler();
            private WorkThread workThread;

            public JsonViewHolder(View view) : base(view) {
                // Retrieve components
                artistTv = view.FindViewById<TextView>(Resource.Id.artistTv);
                timeTv = view.FindViewById<TextView>(Resource.Id.timesTv);
                trackIdTv = view.FindViewById<TextView>(Resource.Id.trackIdTv);
                titleTv = view.FindViewById<TextView>(Resource.Id.titleTv);
                songImage = view.FindViewById<ImageView>(Resource.Id.songImage);
            }

            public void BindViewHolder(Song song, WorkThread workThread) {
                artistTv.Text = song.Artist;
                timeTv.Text = song.SongDate.ToString();
                trackIdTv.Text = song.TrackId;
                titleTv.Text = song.Title;
                this.songId = song.TrackId;
                this.workThread = workThread;

                // Download image
                songImage.SetImageResource(Android.Resource.Drawable.IcMenuRotate);
                DownloadImage(song.TrackId, song.TrackImage);
            }

            private void DownloadImage(string id, string url) {
                Action downloadTask = new Action(delegate
                    {
                        try
                        {
                            Bitmap image = BitmapDownload(url);

                            if (id.Equals(songId)) {
                                uiHandler.Post(new Action(delegate {
                                    // Update UI
                                    songImage.SetImageBitmap(image);
                                }));
                            }
                        }
                        catch (Exception e)
                        {
                            // Manage error here.
                        }
                    });
                workThread.PostTask(downloadTask);
            }

            private Bitmap BitmapDownload(string url)
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.Method = "GET";

                // Send the request to the server and wait for the response:
                using (WebResponse response = request.GetResponse())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        return BitmapFactory.DecodeStream(stream);
                    }
                }
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
            (holder as JsonViewHolder).BindViewHolder(song, workThread);
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

