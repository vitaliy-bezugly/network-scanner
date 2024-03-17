namespace IpScanner.Helpers.Messages
{
    public class DevicesLoadedMessage<T>
    {
        public DevicesLoadedMessage(T storageFile)
        {
            NewFile = storageFile;
        }

        public T NewFile { get; set; }
    }
}
