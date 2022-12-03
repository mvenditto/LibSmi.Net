using System.Runtime.InteropServices;

namespace LibSmi.Net
{
    public enum SmiLanguage
    {
        Unknown = 0,  /* should not occur */
        SmiV1 = 1,
        SmiV2 = 2,
        Sming = 3,
        Sppi = 4,
        Yang = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiModuleStruct
    {
        public readonly /*string*/ IntPtr SmiIdentifier;

        public readonly /*string*/ IntPtr Path;

        public readonly /*string*/ IntPtr Organization;

        public readonly /*string*/ IntPtr Contactinfo;

        public readonly /*string*/ IntPtr Description;

        public readonly /*string*/ IntPtr Reference;

        public readonly int Language;

        public readonly int Conformance;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly record struct SmiRevisionStruct
    {
        public readonly long Date;

        public readonly /*string*/ IntPtr Description;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly record struct SmiImportStruct
    {
        public readonly /*string*/ IntPtr Module;

        public readonly /*string*/ IntPtr Name;
    }

    internal unsafe static partial class LibSmiNative
    {
        [DllImport(LibSmi.Path, EntryPoint = "smiGetModule", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiModuleStruct* SmiGetModule([MarshalAs(UnmanagedType.LPStr), In] string module);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstModule", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiModuleStruct* SmiGetFirstModule();

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextModule", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiModuleStruct* SmiGetNextModule([In] SmiModuleStruct* module);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetModuleIdentityNode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern IntPtr SmiGetModuleIdentityNode([In] SmiModuleStruct* module);

        [DllImport(LibSmi.Path, EntryPoint = "smiIsImported", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiIsImported([In] SmiModuleStruct* smiModule, [In] SmiModuleStruct* importedModule, [MarshalAs(UnmanagedType.LPStr), In] string node);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstRevision", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiRevisionStruct* SmiGetFirstRevision([In] SmiModuleStruct* smiModule);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextRevision", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiRevisionStruct* SmiGetNextRevision([In] SmiRevisionStruct* smiRevision);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetRevisionLine", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiGetRevisionLine([In] SmiRevisionStruct* smiRevision);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstImport", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiImportStruct* SmiGetFirstImport([In] SmiModuleStruct* smiModule);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextImport", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiImportStruct* SmiGetNextImport([In] SmiImportStruct* smiImport);
    }
}