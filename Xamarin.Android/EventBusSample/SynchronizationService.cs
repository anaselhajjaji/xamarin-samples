using System;
using System.Threading;

using Android.App;

using DSoft.Messaging;

namespace EventBusSample
{
    /// <summary>
    /// Synchronization service.
    /// </summary>
    [Service]
    public class SynchronizationService : IntentService
    {
        protected override void OnHandleIntent(Android.Content.Intent intent)
        {
            for (int i = 10; i > 0; i--)
            {
                MessageBus.Default.Post("1234", this, new object[]{ i });

                // Wait 1 sec
                Thread.Sleep(1000);
            }

            MessageBus.Default.Post("1234", this, new object[]{ "Hello!" });
        }
    }
}

