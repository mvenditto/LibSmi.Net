namespace LibSmi.Net.Macro
{
    public unsafe class SmiMacro : LazyStructProxy
    {
        private readonly SmiMacroStruct* _macroPtr;

        private readonly Lazy<string?> _name;

        private readonly Lazy<string?> _description;

        private readonly Lazy<string?> _reference;

        internal SmiMacro(SmiMacroStruct* macroPtr) : base(macroPtr)
        {
            _macroPtr = macroPtr;

            _name = LazyAnsiStringMarshal(_macroPtr->Name);

            _description = LazyAnsiStringMarshal(_macroPtr->Description);

            _reference = LazyAnsiStringMarshal(_macroPtr->Reference);
        }

        internal static unsafe SmiMacro? FromPtr(SmiMacroStruct* nodePtr)
        {
            if (nodePtr == null)
            {
                return null;
            }

            return new(nodePtr);
        }

        internal SmiMacroStruct* UnderlyingPtr => _macroPtr;

        public string? Name => _name.Value;

        public string? Description => _description.Value;

        public string? Reference => _reference.Value;

        public SmiDecl Decl => _macroPtr->Decl;

        public SmiStatus Status => _macroPtr->Status;
    }
}