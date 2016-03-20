using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

using Xamarin.Forms;
using Newtonsoft.Json;

namespace ListViewSample
{
    public partial class MyMainPage : ContentPage
    {
        public MyMainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try {
                // Fetch the data
                EmployeeView.ItemsSource = await FetchSongs(Consts.JsonUrl);
            }
            catch(Exception e) {
                await DisplayAlert ("Error", e.Message, "OK");
            }
            finally {
                Progress.IsVisible = false;
            }
        }

        private async Task<List<Song>> FetchSongs(string url)
        {
            // Progress step
            await Progress.ProgressTo(.2, 250, Easing.Linear);

            // Create an HTTP web request using the URL:
            var httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request);

            // Progress step
            await Progress.ProgressTo(.4, 250, Easing.Linear);

            // read the stream
            string responseContent = await response.Content.ReadAsStringAsync();

            // Progress step
            await Progress.ProgressTo(.6, 250, Easing.Linear);

            // Deserialize JSON
            List<Song> songs = JsonConvert.DeserializeObject<List<Song>>(responseContent);

            // Progress step
            await Progress.ProgressTo(1, 250, Easing.Linear);

            return songs;
        }
    }
}

