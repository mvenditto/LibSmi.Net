namespace LibSmi.Net
{
    public class SmiError
    {
        public string? Path { get; init; }

        public string? Message { get; init; }

        public int Line { get; init; }

        public int Severity { get; init; }

        public string? Tag { get; init; }
    }
}