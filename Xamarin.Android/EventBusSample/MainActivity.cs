using System;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using DSoft.Messaging;

namespace EventBusSample
{
    [Activity(Label = "EventBusSample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        MessageBusEventHandler eventHandler;
        Handler uiHandler = new Handler();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our text view from the layout resource
            TextView textView = FindViewById<TextView>(Resource.Id.myTextView);

            // Create the event handler
            eventHandler = new MessageBusEventHandler(
                    "1234",
                    (sender, data) =>
                        {
                            //update the UI
                            uiHandler.Post(new Action(delegate() {
                                textView.Text = data.Data[0].ToString();
                            }));
                        });

            // Start the service
            StartService(new Intent(this, typeof(SynchronizationService)));
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Register on the bus
            MessageBus.Default.Register(eventHandler);
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Deregister
            MessageBus.Default.DeRegister(eventHandler);
        }
    }
}


