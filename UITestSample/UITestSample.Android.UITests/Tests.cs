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
        public void Insert50ItemsInList()
        {
            // Insert 50 elements in the list
            Func<AppQuery, AppQuery> addButton = c => c.Id("addAction");
            for (int i = 0; i < 50; i++) {
                app.Tap(addButton);
            }
            app.Screenshot("50 elements inserted.");

            //AppResult[] results = app.Query(MyButton);
            //app.Screenshot("Button clicked twice.");

            //Assert.AreEqual("2 clicks!", results[0].Text);
        }
    }
}

