using System.Runtime.InteropServices;

namespace LibSmi.Net
{

    /* SmiDecl -- type or statement that leads to a definition*/
    public enum SmiDecl : int
    {
        Unknown = 0,  /* should not occur*/
        /* SMIv1/v2 ASN.1 statements and macros */
        ImplicitType = 1,
        TypeAssignment = 2,
        SequenceOf = 4,  // this will go away
        ValueAssignment = 5,
        ObjectType = 6,    /* values >= 6 are assumed to be */
        ObjectIdentity = 7,    /* registering an OID, see check.c */
        ModuleIdentity = 8,
        NotificationType = 9,
        TrapType = 10,
        ObjectGroup = 11,
        NotificationGroup = 12,
        ModuleCompliance = 13,
        AgentCapabilities = 14,
        TextualConvention = 15,
        Macro = 16,
        ComplGroup = 17,
        ComplObject = 18,
        ImplObject = 19,  /* object label in sth like "iso(1)" */
        /* SMIng statements */
        Module = 33,
        Extension = 34,
        Typedef = 35,
        Node = 36,
        Scalar = 37,
        Table = 38,
        Row = 39,
        Column = 40,
        Notification = 41,
        Group = 42,
        Compliance = 43,
        Identity = 44,
        Class = 45,
        Attribute = 46,
        Event = 47
    }

    /* SmiAccess -- values of access levels */
    public enum SmiAccess : int
    {
        Unknown = 0, /* should not occur*/
        NotImplemented = 1, /* only for agent capability variations*/
        NotAccesible = 2, /* the values 2 to 5 are allowed to be*/
        Notifiy = 3, /* compared by relational operators.*/
        ReadOnly = 4,
        ReadWrite = 5,
        Install = 6, /* these three entries are only valid*/
        InstallNotify = 7, /* for SPPI*/
        ReportOnly = 8,
        EventOnly = 9  /* this entry is valid only for SMIng*/
    }

    /* SmiStatus -- values of status levels*/
    public enum SmiStatus : int
    {
        Unknown = 0, /* should not occur*/
        Current = 1, /* only SMIv2, SMIng and SPPI*/
        Deprecated = 2, /* SMIv1, SMIv2, SMIng and SPPI*/
        Mandatory = 3, /* only SMIv1 */
        Optional = 4, /* only SMIv1*/
        Obsolete = 5  /* SMIv1, SMIv2, SMIng and SPPI*/
    }

    /* SmiBasetype -- base types of all languages                                */
    public enum SmiBaseType: int
    {
        Unknown = 0,  /* should not occur            */
        Integer32 = 1,  /* also SMIv1/v2 INTEGER       */
        OctetString = 2,
        ObjectIdentifier = 3,
        Unsigned32 = 4,
        Integer64 = 5,  /* SMIng and SPPI              */
        Unsigned64 = 6,  /* SMIv2, SMIng and SPPI       */
        Float32 = 7,  /* only SMIng                  */
        Float64 = 8,  /* only SMIng                  */
        Float128 = 9,  /* only SMIng                  */
        Enum = 10,
        Bits = 11, /* SMIv2, SMIng and SPPI       */
        Pointer = 12  /* only SMIng                  */
    }

    /* SmiIndexkind -- actual kind of a table row's index method                 */
    public enum SmiIndexKind: int
    {
        Unknown = 0,
        Index = 1,
        Augment = 2,
        Reorder = 3,
        Sparse = 4,
        Expand = 5
    }

    /* SmiValue -- any single value; for use in default values and subtyping*/
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public unsafe struct SmiValue
    {
        [FieldOffset(0)]
        public /*SmiBasetype*/ int BaseType;

        [FieldOffset(4)]
        public uint Len; // OID, OctetString, Bits

        #region union value
        [FieldOffset(16)]
        public ulong SmiUnsigned64;

        [FieldOffset(16)]
        public long SmiInteger64;

        [FieldOffset(16)]
        public uint SmiUnsigned32;

        [FieldOffset(16)]
        public int SmiInteger32;

        [FieldOffset(16)]
        public float SmiFloat32; 
        
        [FieldOffset(16)]
        public double SmiFloat64;

        [FieldOffset(16)]
        public double SmiFloat128; // long double ?

        [FieldOffset(16)]
        public uint* Oid;

        [FieldOffset(16)]
        public byte* Ptr;  // OID, OctetString, Bits
        #endregion
    }

    [Flags]
    public enum SmiNodeKind: uint
    {
        Unknown = 0x0000, // should not occur
        Node = 0x0001,
        Scalar = 0x0002,
        Table = 0x0004,
        Row = 0x0008,
        Column = 0x0010,
        Notification = 0x0020,
        Group = 0x0040,
        Compliance = 0x0080,
        Capabilities = 0x0100,
        Any = 0xffff
    }

    /*
        field   offset  size
        name         0
        oidlen       8  (8)
        oid         16  (8)
        decl        24  (8)
        access      28  (4)
        status      32  (4)
        format      40  (8)
        value       48 (32)
        units       80  (8) 
        description 88  (8)
        reference   96  (8)
        indexkind  104  (4)
        implied    108  (4)
        create     112  (4)
        nodekind   116
    */
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiNodeStruct
    {
        public readonly /*string*/ IntPtr Name;

        public readonly uint OidLen;

        public readonly IntPtr OidPtr;

        public readonly SmiDecl Decl;

        public readonly SmiAccess Access;

        public readonly SmiStatus Status;

        public readonly /*string*/ IntPtr Format;

        public readonly SmiValue Value;

        public readonly /*string*/ IntPtr Units;

        public readonly /*string*/ IntPtr Description;

        public readonly /*string*/ IntPtr Reference;

        public readonly SmiIndexKind IndexKind; // only valid for rows

        public readonly int Implied; // only valid for rows

        public readonly int Create; // only valid for rows

        public readonly SmiNodeKind NodeKind;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiOptionStruct
    {
        public readonly IntPtr Description;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiRefinementStruct
    {
        public readonly SmiAccess Access;

        public readonly IntPtr Description;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiTypeStruct
    {
        public readonly /*string*/ IntPtr Name;

        public readonly SmiBaseType BaseType;

        public readonly SmiDecl Decl;

        public readonly /*string*/ IntPtr Format;

        public readonly SmiValue Value;

        public readonly /*string*/ IntPtr Units;

        public readonly SmiStatus Status;

        public readonly /*string*/ IntPtr Description;

        public readonly /*string*/ IntPtr Reference;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiElementStruct
    {
        public readonly char _; // dummy
    }

    internal unsafe static partial class LibSmiNative
    {
        [DllImport(LibSmi.Path, EntryPoint = "smiGetNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetNode([In] SmiModuleStruct* module, [MarshalAs(UnmanagedType.LPStr), In] string node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNodeByOID", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetNodeByOid([In] uint oidLen, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In] uint[] oid);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetFirstNode([In] SmiModuleStruct* module, SmiNodeKind kinds);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetNextNode([In] SmiNodeStruct* node, SmiNodeKind kinds);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetParentNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetParentNode([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetRelatedNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetRelatedNode([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstChildNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetFirstChildNode([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextChildNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiNodeStruct* SmiGetNextChildNode([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNodeModule", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SmiModuleStruct* SmiGetNodeModule([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNodeType", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiTypeStruct* SmiGetNodeType([In] SmiNodeStruct* node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNodeLine", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiGetNodeLine([In] SmiNodeStruct* node);
        
        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstElement", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiElementStruct* SmiGetFirstElement([In] SmiNodeStruct* smiNodePtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextElement", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiElementStruct* SmiGetNextElement([In] SmiElementStruct* smiElementPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetElementNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiNodeStruct* SmiGetElementNode([In] SmiElementStruct* smiElementPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstOption", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiOptionStruct* SmiGetFirstOption([In] SmiNodeStruct* smiComplianceNodePtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextOption", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiOptionStruct* SmiGetNextOption([In] SmiOptionStruct* smiOptionPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetOptionNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiNodeStruct* SmiGetOptionNode([In] SmiOptionStruct* smiOptionPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstRefinement", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiRefinementStruct* SmiGetFirstRefinement([In] SmiNodeStruct* smiComplianceNodePtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextRefinement", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiRefinementStruct* SmiGetNextRefinement([In] SmiRefinementStruct* smiRefinementPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetRefinementNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiNodeStruct* SmiGetRefinementNode([In] SmiRefinementStruct* smiRefinementPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetRefinementType", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiTypeStruct* SmiGetRefinementType([In] SmiRefinementStruct* smiRefinementPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetRefinementWriteType", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiTypeStruct* SmiGetRefinementWriteType([In] SmiRefinementStruct* smiRefinementPt);
    }
}