using System.Text.RegularExpressions;

namespace LibSmi.Net.Error
{
    public static class SmiErrorHelpers
    {
        public static SmiErrorKind GetErrorKind(int errorLevel)
        {
            if (errorLevel <= 6)
            {
                return (SmiErrorKind)errorLevel;
            }

            return SmiErrorKind.IgnoredErrorMessage;
        }
    }
}
