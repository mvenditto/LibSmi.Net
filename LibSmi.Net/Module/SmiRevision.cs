using LibSmi.Net.Node;
using System.Runtime.InteropServices;

namespace LibSmi.Net.Module
{
    public unsafe class SmiRevision : LazyStructProxy
    {
        private readonly SmiRevisionStruct* _revisionPtr;

        private readonly Lazy<string?> _description;

        internal SmiRevision(SmiRevisionStruct* revisionPtr): base(revisionPtr)
        {
            _revisionPtr = revisionPtr;

            _description = LazyAnsiStringMarshal(_revisionPtr->Description);
        }

        internal static SmiRevision? FromPtr(SmiRevisionStruct* modulePtr)
        {
            if (modulePtr == null)
            {
                return null;
            }

            return new SmiRevision(modulePtr);
        }

        internal SmiRevisionStruct* UnderlyingPtr => _revisionPtr;

        public string? Description => _description.Value;

        public long Date => _revisionPtr->Date;
    }
}