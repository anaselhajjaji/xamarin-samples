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
        private RecyclerView recyclerView;
        private Android.Support.V7.Widget.RecyclerView.LayoutManager layoutManager;
        private Handler uiHandler = new Handler();
        private WorkThread workThread;

        protected override void OnCreate(Bundle savedInstanceState)
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
            workThread = new WorkThread("Download Songs", 10);
            Action downloadTask = new Action(delegate
                {
                    try
                    {
                        string url = GetString(Resource.String.data_url_github);
                        List<Song> songs = FetchSongs(url);

                        uiHandler.Post(new Action(delegate {
                            // Update UI
                            recyclerView.SetAdapter(new JsonAdapter(songs));
                        }));
                    }
                    catch (Exception e)
                    {
                        uiHandler.Post(new Action(delegate {
                            Toast.MakeText(this, "Failed to download data: " + e.Message, ToastLength.Long).Show();
                        }));
                    }
                    finally
                    {
                        uiHandler.Post(new Action(delegate {
                            progressDialog.Hide();
                        }));
                    }
                });
            workThread.Start();
            workThread.PrepareHandler();
            workThread.PostTask(downloadTask);
        }

        protected override void OnDestroy()
        {
            workThread.Quit();

            base.OnDestroy();
        }

        private List<Song> FetchSongs(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = request.GetResponse())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    List<Song> songs = (List<Song>)serializer.Deserialize(streamReader, typeof(List<Song>));
                    return songs;
                }
            }
        }
    }
}


