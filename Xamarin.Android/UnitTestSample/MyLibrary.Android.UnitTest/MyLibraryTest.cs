using System;
using NUnit.Framework;

using MyLibrary.Android;

namespace UnitTestSample.Android
{
    [TestFixture]
    public class TestsSample
    {
        MyLibraryClass myLib;

        [SetUp]
        public void Setup()
        {
            myLib = new MyLibraryClass();
        }
        
        [TearDown]
        public void Tear()
        {
            myLib.Dispose();
        }

        [Test]
        public void Pass()
        {
            Assert.True(myLib.ShouldReturnTrue());
        }

        [Test]
        public void Fail()
        {
            Assert.True(myLib.ShouldReturnFalse());
        }

        [Test]
        [Ignore("Ignored test for some reason")]
        public void Ignore()
        {
            myLib.ShouldReturnFalse();
        }
    }
}

