namespace LibSmi.Net.Node
{
    public unsafe class SmiType: LazyStructProxy
    {
        private readonly SmiTypeStruct* _typePtr;

        private readonly Lazy<string?> _name;

        private readonly Lazy<string?> _format;

        private readonly Lazy<string?> _units;

        private readonly Lazy<string?> _description;

        private readonly Lazy<string?> _reference;

        internal SmiType(SmiTypeStruct* typePtr): base(typePtr)
        {
            _typePtr = typePtr;

            _name = LazyAnsiStringMarshal(_typePtr->Name);

            _format = LazyAnsiStringMarshal(_typePtr->Format);

            _units = LazyAnsiStringMarshal(_typePtr->Units);

            _description = LazyAnsiStringMarshal(_typePtr->Description);

            _reference = LazyAnsiStringMarshal(_typePtr->Reference);
        }

        internal static unsafe SmiType? FromPtr(SmiTypeStruct* nodePtr)
        {
            if (nodePtr == null)
            {
                return null;
            }

            return new(nodePtr);
        }

        internal SmiTypeStruct* UnderlyingPtr => _typePtr;

        public string? Name => _name.Value;

        public string? Format => _format.Value;

        public string? Units => _units.Value;

        public string? Description => _description.Value;

        public string? Reference => _reference.Value;

        public SmiDecl Decl => _typePtr->Decl;

        public SmiBaseType BaseType => _typePtr->BaseType;

        public SmiStatus Status => _typePtr->Status;
    }
}