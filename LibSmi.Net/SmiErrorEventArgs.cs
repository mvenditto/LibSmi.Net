namespace LibSmi.Net
{
    public class SmiErrorEventArgs
    {
        public string? Path { get; init; }

        public string? Message { get; init; }

        public int Line { get; init; }

        public int Severity { get; init; }

        public string? Tag { get; init; }
    }
}