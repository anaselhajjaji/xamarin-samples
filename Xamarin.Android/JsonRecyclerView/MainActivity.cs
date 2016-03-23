using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Json;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;

using Newtonsoft.Json;

namespace JsonRecyclerView
{
    [Activity(Label = "JsonRecyclerView", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        RecyclerView recyclerView;
        Android.Support.V7.Widget.RecyclerView.LayoutManager layoutManager;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Initialize the recycler view
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.AddItemDecoration(new SimpleItemDecoration(this));

            // Show progress dialog
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage(GetString(Resource.String.loading_msg));
            progressDialog.Show();

            // Fetch songs
            try
            {
                string url = GetString(Resource.String.data_url_github);
                List<Song> songs = await FetchSongs(url);

                // Update UI
                recyclerView.SetAdapter(new JsonAdapter(songs));
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Failed to download data: " + e.Message, ToastLength.Long).Show();
            }
            finally
            {
                progressDialog.Hide();
            }
        }

        private async Task<List<Song>> FetchSongs(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    List<Song> songs = JsonConvert.DeserializeObject<List<Song>>(jsonDoc.ToString());
                    return songs;
                }
            }
        }
    }
}


