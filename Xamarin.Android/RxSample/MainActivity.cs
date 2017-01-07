using System;

using Android.App;
using Android.Widget;
using Android.OS;

using Com.Bumptech.Glide;

namespace RxSample
{
    [Activity(Label = "RxSample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IObserver<ProcessedData>
    {

        ProcessorExample processor = new ProcessorExample();
        ImageView imageView;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            // Show a message dialog to inform in case of error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(error.Message)
                .SetPositiveButton("OK", (sender, e) => { })
                .Create().Show();
        }

        public void OnNext(ProcessedData value)
        {
            if (value.Data != null)
            {
                RunOnUiThread(() => Glide.With(this).Load(value.Data).Into(imageView));
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            Button startButton = FindViewById<Button>(Resource.Id.startButton);
            startButton.Click += (sender, e) => 
            { 
                processor.Start();
                Toast.MakeText(this, "Processing started.", ToastLength.Short).Show();
            };
            Button stopButton = FindViewById<Button>(Resource.Id.stopButton);
            stopButton.Click += (sender, e) => 
            { 
                processor.Stop(); 
                Toast.MakeText(this, "Processing stoped.", ToastLength.Short).Show();
            };

            // Subscribe
            // TODO Investigate SubscribeOn(NewThreadScheduler.Default).ObserveOn(NewThreadScheduler.Default)
            processor.Subscribe(this);
        }

        protected override void OnPause()
        {
            base.OnPause();

            processor.Stop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Unsubscribe...
            processor.Dispose();
        }
    }
}

