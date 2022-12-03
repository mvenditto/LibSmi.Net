using LibSmi.Net.Node;
using System.Runtime.InteropServices;

namespace LibSmi.Net.Module
{
    public class SmiModule: LazyStructProxy
    {
        private unsafe readonly SmiModuleStruct* _modulePtr;

        private readonly Lazy<string?> _smiIdentifier;

        private readonly Lazy<string?> _path;

        private readonly Lazy<string?> _organization;

        private readonly Lazy<string?> _contactInfo;

        private readonly Lazy<string?> _description;

        private readonly Lazy<string?> _reference;

        internal unsafe SmiModule(SmiModuleStruct* modulePtr): base(modulePtr)
        {
            _modulePtr = modulePtr;

            _smiIdentifier = LazyAnsiStringMarshal(_modulePtr->SmiIdentifier);

            _path = LazyAnsiStringMarshal(_modulePtr->Path);

            _organization = LazyAnsiStringMarshal(_modulePtr->Organization);

            _contactInfo = LazyAnsiStringMarshal(_modulePtr->Contactinfo);

            _description = LazyAnsiStringMarshal(_modulePtr->Description);

            _reference = LazyAnsiStringMarshal(_modulePtr->Reference);
        }

        internal unsafe static SmiModule? FromPtr(SmiModuleStruct* modulePtr)
        {
            if (modulePtr == null)
            {
                return null;
            }

            return new SmiModule(modulePtr);
        }

        internal unsafe SmiModuleStruct* UnderlyingPtr => _modulePtr;

        public string? SmiIdentifier => _smiIdentifier.Value;

        public string? Path => _path.Value;

        public string? Organization => _organization.Value;

        public string? ContactInfo => _contactInfo.Value;

        public string? Description => _description.Value;

        public string? Reference => _reference.Value;

        public unsafe SmiLanguage Language => (SmiLanguage)_modulePtr->Language;

        public unsafe int Conformance => _modulePtr->Conformance;
    }
}