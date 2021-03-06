﻿using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

namespace InsightsSample
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

            // Insights!
            Xamarin.Insights.Track("New session started...", new Dictionary <string, string> { 
                {"Local-time", DateTime.Now.ToString()}
            });

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // A LinearLayoutManager is used here, this will layout the elements in a similar fashion
            // to the way ListView would layout elements. The RecyclerView.LayoutManager defines how the
            // elements are laid out.
            layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.AddItemDecoration(new SimpleItemDecoration(this));
            adapter = new CustomAdapter(dataSet);
            // Set CustomAdapter as the adapter for RecycleView
            recyclerView.SetAdapter(adapter);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle item selection
            switch (item.ItemId)
            {
                case Resource.Id.addAction:
                    AddNewItem();
                    return true;
                default:
                    return OnOptionsItemSelected(item);
            }
        }

        private void AddNewItem()
        {
            View dialogView = LayoutInflater.Inflate(Resource.Layout.dialog_view, null);

            //set alert for executing the task
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Item creation");
            alert.SetView(dialogView);
            alert.SetPositiveButton("Create", (senderAlert, args) =>
                {
                    EditText editText = (EditText)dialogView.FindViewById(Resource.Id.itemName);
                    if (!string.IsNullOrEmpty(editText.Text)) {
                        dataSet.Add(editText.Text);
                        adapter.NotifyItemInserted(dataSet.Count - 1);

                        // Insights!
                        Xamarin.Insights.Track("New item added...", new Dictionary <string, string> { 
                            {"Position", (dataSet.Count - 1).ToString()},
                            {"Item name", editText.Text}
                        });

                        Toast.MakeText(this, Resource.String.dialog_positive_message, ToastLength.Short).Show();
                    }
                });
            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    // Insights!
                    Xamarin.Insights.Track("Addition cancelled...");

                    Toast.MakeText(this, Resource.String.dialog_negative_message, ToastLength.Short).Show();
                });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}


