using System;
using System.Collections.Generic;
using System.Collections;

using Android.OS;

namespace JsonRecyclerView
{
    public class WorkThread : HandlerThread
    {
        private Handler workerHandler;
        private int queueSize;
        private IList<Action> callbacks;

        public WorkThread(string name, int queueSize) : base(name)
        {
            this.queueSize = queueSize;
            this.callbacks = new List<Action>();
        }

        public void PostTask(Action task) {
            if (callbacks.Count > queueSize)
            {
                workerHandler.RemoveCallbacks(callbacks[0]);
                callbacks.RemoveAt(0);
            }

            callbacks.Add(task);
            workerHandler.Post(task);
        }

        public void PrepareHandler() {
            workerHandler = new Handler(Looper);
        }
    }
}

