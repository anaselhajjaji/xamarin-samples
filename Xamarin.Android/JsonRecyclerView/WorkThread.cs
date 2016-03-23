using System;
using System.Collections.Generic;
using System.Collections;

using Android.OS;

namespace JsonRecyclerView
{
    public class WorkThread : HandlerThread
    {
        private Handler workerHandler;

        public WorkThread(string name) : base(name)
        {
        }

        public void PostTask(Action task) {
            workerHandler.Post(task);
        }

        public void PrepareHandler() {
            workerHandler = new Handler(Looper);
        }
    }
}

