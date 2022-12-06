using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSmi.Net.Error
{
    public enum SmiErrorKind
    {
        Fatal = 0,
        Severe = 1,
        Error = 2,
        MinorError = 3,
        ChangeReccomended = 4,
        Warning = 5,
        Notice = 6,
        IgnoredErrorMessage = 128
    }
}
