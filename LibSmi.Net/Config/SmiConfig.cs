using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSmi.Net.Config
{
    public class SmiConfig
    {
        public string? Tag { get; set; }

        public string? SmiPath { get; init; }

        public int ErrorLevel { get; init; }

        public SmiFlags? UserFlags { get; init; }
    }
}
