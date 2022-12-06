using LibSmi.Net.Module;

namespace LibSmi.Net.Helpers
{
    public class SmiModuleLoadResult
    {
        public SmiModule? Module { get; set; }

        public bool Success => Module != null && Errors?.Any() == false;

        public IEnumerable<SmiError>? Errors { get; set; }
    }
}
