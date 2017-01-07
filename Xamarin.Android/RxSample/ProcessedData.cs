namespace RxSample
{
    /// <summary>
    /// The object that holds the data to be sent to the observer in Rx context.
    /// </summary>
    public class ProcessedData
    {
        public byte[] Data { get; private set; }
        public ProcessingEventName EventName { get; private set; }

        public ProcessedData(byte[] data, ProcessingEventName eventName)
        {
            Data = data;
            EventName = eventName;
        }
    }
}