using System;

namespace MyLibrary.Android
{
    public class MyLibraryClass : IDisposable
    {
        public MyLibraryClass()
        {
        }

        public bool ShouldReturnTrue()
        {
            return true;
        }

        public bool ShouldReturnFalse()
        {
            return false;
        }

        public string ReturnSameValue(string value)
        {
            return value;
        }

        #region IDisposable implementation

        public void Dispose()
        {
            // Free resources here
        }

        #endregion
    }
}

