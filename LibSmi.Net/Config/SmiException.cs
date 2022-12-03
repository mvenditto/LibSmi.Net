using System.Text.Json;

namespace LibSmi.Net.Config
{
    public class SmiException : Exception
    {
        public int ReturnCode { get; private set; }

        private static string ToCamelCase(string s)
        {
            return JsonNamingPolicy.CamelCase.ConvertName(s);
        }

        public SmiException(string funcName, int retcode)
            :base($"{ToCamelCase(funcName)} failed with retcode={retcode}")
        {
            ReturnCode = retcode;
        }
    }
}
