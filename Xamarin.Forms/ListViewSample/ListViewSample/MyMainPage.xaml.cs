using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace ListViewSample
{
    public partial class MyMainPage : ContentPage
    {
        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public MyMainPage()
        {
            InitializeComponent();

            // Define the data source
            EmployeeView.ItemsSource = employees;

            // Populate the data source
            PopulateDataSource();
        }

        private void PopulateDataSource() {
            for (int i = 1; i < 100; i++)
            {
                employees.Add(new Employee{ DisplayName="Employee Number " + i});
            }
        }
    }
}

