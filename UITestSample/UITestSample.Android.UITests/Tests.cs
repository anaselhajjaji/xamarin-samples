using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace UITestSample.Android.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp.Android.StartApp();
        }

        [Test]
        public void AbortInsertion()
        {
            // Tap add
            Func<AppQuery, AppQuery> addButton = c => c.Id("addAction");
            app.Tap(addButton);

            // Wait for the dialog
            app.WaitForElement(c=>c.Id("itemCreationPrompt"), "Did not see the dialog appearing.", TimeSpan.FromSeconds(10));

            // Tap cancel
            Func<AppQuery, AppQuery> cancelButton = c => c.Text("Cancel");
            app.Tap(cancelButton);

            // Verify the toast
            app.WaitForElement(c=>c.Text("Cancelled!"), "Did not see the abort message.", TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Insert50ItemsInList()
        {
            app.Screenshot("Initial state before insertion.");

            for (int i = 1; i <= 50; i ++) {
                // Tap Add
                Func<AppQuery, AppQuery> addButton = c => c.Id("addAction");
                app.Tap(addButton);

                // Wait for the dialog
                app.WaitForElement(c=>c.Id("itemCreationPrompt"), "Did not see the dialog appearing.", TimeSpan.FromSeconds(10));

                Func<AppQuery, AppQuery> editText = c => c.Id("itemName");
                app.EnterText(editText, "Item " + i);

                // Tap create
                Func<AppQuery, AppQuery> createButton = c => c.Text("Create");
                app.Tap(createButton);

                // Verify the toast
                app.WaitForElement(c=>c.Text("Item created!"), "Did not see the creation message.", TimeSpan.FromSeconds(10));
            }
        }
    }
}

