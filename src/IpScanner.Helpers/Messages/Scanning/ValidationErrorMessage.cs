namespace IpScanner.Helpers.Messages.Scanning
{
    public class ValidationErrorMessage
    {
        public ValidationErrorMessage()
        {
            HasErrors = true;
        }

        public ValidationErrorMessage(bool hasErrors)
        {
            HasErrors = hasErrors;
        }

        public bool HasErrors { get; }
    }
}
