using LibSmi.Net.Node;
using System.Runtime.InteropServices;

namespace LibSmi.Net.Module
{
    public unsafe class SmiImport: LazyStructProxy
    {
        private readonly SmiImportStruct* _importPtr;

        private readonly Lazy<string?> _module;

        private readonly Lazy<string?> _name;

        internal SmiImport(SmiImportStruct* importPtr): base(importPtr)
        {
            _importPtr = importPtr;

            _module = LazyAnsiStringMarshal(importPtr->Module);

            _name = LazyAnsiStringMarshal(_importPtr->Name);
        }

        internal static SmiImport? FromPtr(SmiImportStruct* modulePtr)
        {
            if (modulePtr == null)
            {
                return null;
            }

            return new SmiImport(modulePtr);
        }

        internal SmiImportStruct* UnderlyingPtr => _importPtr;

        public string? Name => _name.Value;

        public string? Module => _module.Value;
    }
}