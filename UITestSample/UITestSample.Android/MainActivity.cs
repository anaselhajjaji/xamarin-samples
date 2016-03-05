using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

namespace UITestSample.Android
{
    [Activity(Label = "UITestSample.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private RecyclerView recyclerView;
        private RecyclerView.Adapter adapter;
        private RecyclerView.LayoutManager layoutManager;
        private List<string> dataSet = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // A LinearLayoutManager is used here, this will layout the elements in a similar fashion
            // to the way ListView would layout elements. The RecyclerView.LayoutManager defines how the
            // elements are laid out.
            layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);

            adapter = new CustomAdapter(dataSet);
            // Set CustomAdapter as the adapter for RecycleView
            recyclerView.SetAdapter(adapter);
        }

        public override bool OnCreateOptionsMenu(IMenu menu) {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }
            
        public override bool OnOptionsItemSelected(IMenuItem item) {
            // Handle item selection
            switch (item.ItemId) {
                case Resource.Id.addAction:
                    dataSet.Add("new item");
                    adapter.NotifyItemInserted(dataSet.Count - 1);
                    return true;
                default:
                    return OnOptionsItemSelected(item);
            }
        }
    }
}


