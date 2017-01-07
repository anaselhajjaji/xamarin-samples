using System;
using System.IO;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxSample
{
    public class ProcessorExample : IObservable<ProcessedData>, IDisposable
    {
        readonly Subject<ProcessedData> subject = new Subject<ProcessedData>();
        IDisposable inner;

        public ProcessorExample()
        {
            inner = Disposable.Create(() => ReleaseResources());
        }

        public void Stop()
        {
        }

        public IObservable<ProcessedData> Start()
        {
            return Observable.Create<ProcessedData>(
                (observer, cancellationToken) => Task.Factory.StartNew(
                () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            Thread.Sleep(3000);

                            byte[] data = MakeHttpRequest();
                            observer.OnNext(new ProcessedData(data, ProcessingEventName.DataAvailable));
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            break;
                        }
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default));
        }

        /// <summary>
        /// Makes the http request: retriving a random image data buffer.
        /// </summary>
        /// <returns>The image data buffer.</returns>
        byte[] MakeHttpRequest()
        {
            byte[] data = null;
            var request = HttpWebRequest.Create("http://lorempixel.com/200/200/");
            request.ContentType = "application/json";
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                using (var memstream = new MemoryStream())
                {
                    var buffer = new byte[512];
                    var bytesRead = default(int);
                    while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memstream.Write(buffer, 0, bytesRead);
                    }
                    data = memstream.ToArray();
                }
            }

            return data;
        }

        public IDisposable Subscribe(IObserver<ProcessedData> observer)
        {
            return subject.Subscribe(observer);
        }

        public void Dispose()
        {
            // Setting this to Disposable.Empty is an easy way to make sure that
            // nothing bad happens if someone double-disposes
            inner.Dispose();
            inner = Disposable.Empty;
        }

        void ReleaseResources()
        {
        }
    }
}
