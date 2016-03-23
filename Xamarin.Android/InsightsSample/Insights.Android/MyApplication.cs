using System;
using Android.App;
using Android.Runtime;

namespace InsightsSample
{
    [Application]
    public class MyApplication : Application
    {
        /// <summary>
        /// Base constructor which must be implemented if it is to successfully inherit from the Application
        /// class.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transfer"></param>
        public MyApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle,transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            // Initialize Insights
            Xamarin.Insights.Initialize(Consts.InsightsApiKey, this);
        }
    }
}

