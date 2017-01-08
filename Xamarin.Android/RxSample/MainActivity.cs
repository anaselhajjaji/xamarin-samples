using System;
using System.Threading;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

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
        IDisposable subscription;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Calling from Thread: " + Thread.CurrentThread.ManagedThreadId);

            // Show a message dialog to inform in case of error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(error.Message)
                .SetPositiveButton("OK", (sender, e) => { })
                .Create().Show();
        }

        public void OnNext(ProcessedData value)
        {
            Console.WriteLine("Calling from Thread: " + Thread.CurrentThread.ManagedThreadId);

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
                subscription = processor.Start().SubscribeOn(TaskPoolScheduler.Default).ObserveOn(Scheduler.CurrentThread).Subscribe(this);
                Toast.MakeText(this, "Processing started.", ToastLength.Short).Show();
            };
            Button stopButton = FindViewById<Button>(Resource.Id.stopButton);
            stopButton.Click += (sender, e) => 
            {
                ReleaseCurrentSubscription();
                Toast.MakeText(this, "Processing stoped.", ToastLength.Short).Show();
            };
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Unsubscribe...
            ReleaseCurrentSubscription();
        }

        void ReleaseCurrentSubscription()
        {
            if (subscription != null)
            {
                subscription.Dispose();
                subscription = null;
            }
        }
    }
}

