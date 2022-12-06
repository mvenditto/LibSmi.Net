using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSmi.Net.Helpers
{
    public class SmiScope : IDisposable
    {
        private readonly string _prevTag;

        public SmiScope(string prevTag, string viewTag)
        {
            _prevTag = prevTag;
            Smi.Initialize(viewTag);
        }

        public void Dispose()
        {
            Smi.Initialize(_prevTag);
            GC.SuppressFinalize(this);
        }
    }
}
