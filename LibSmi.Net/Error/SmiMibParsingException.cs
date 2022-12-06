namespace LibSmi.Net.Error
{
    public class SmiMibParsingException : Exception
    {
        public SmiError Error { get; init; }

        public SmiMibParsingException(SmiError error) : base(error.Message)
        {
            Error = error;
        }
    }
}
