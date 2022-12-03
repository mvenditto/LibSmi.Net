using System.Runtime.InteropServices;

namespace LibSmi.Net
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SmiMacroStruct
    {
        public readonly /*string*/ IntPtr Name;

        public readonly SmiDecl Decl;

        public readonly SmiStatus Status;

        public readonly /*string*/ IntPtr Description;

        public readonly /*string*/ IntPtr Reference;
    }

    internal unsafe static partial class LibSmiNative
    {
        [DllImport(LibSmi.Path, EntryPoint = "smiGetMacro", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiMacroStruct* SmiGetMacro([In] SmiModuleStruct* smiModulePtr, [MarshalAs(UnmanagedType.LPStr), In] string macro);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFirstMacro", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiMacroStruct* SmiGetFirstMacro([In] SmiModuleStruct* smiModulePtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetNextMacro", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiMacroStruct* SmiGetNextMacro([In] SmiMacroStruct* smiMacroPtr);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetMacroModule", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiModuleStruct* SmiGetMacroModule([In] SmiMacroStruct* smiMacroPtr);
    }
}
