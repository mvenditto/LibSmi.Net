using System.Runtime.InteropServices;

namespace LibSmi.Net
{
    [Flags]
    public enum SmiFlags : uint
    {
        NoDescr = 0x0800,
        ViewAll = 0x1000,
        Errors = 0x2000,
        Recursive = 0x4000,
        Stats = 0x8000,
        Mask = NoDescr | ViewAll | Stats | Recursive | Errors
    }

    internal unsafe static partial class LibSmiNative
    {
        [DllImport(LibSmi.Path, EntryPoint = "smiInit", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiInit([MarshalAs(UnmanagedType.LPStr), In] string? tag);

        [DllImport(LibSmi.Path, EntryPoint = "smiSetErrorLevel", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void SmiSetErrorLevel([In] int errorLevel);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetFlags", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern SmiFlags SmiGetFlags();

        [DllImport(LibSmi.Path, EntryPoint = "smiSetFlags", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void SmiSetFlags([In] SmiFlags userFlags);

        [DllImport(LibSmi.Path, EntryPoint = "smiLoadModule", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = true)]
        internal static extern IntPtr SmiLoadModule([MarshalAs(UnmanagedType.LPStr), In] string module);

        [DllImport(LibSmi.Path, EntryPoint = "smiIsLoaded", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiIsLoaded([MarshalAs(UnmanagedType.LPStr), In] string module);

        [DllImport(LibSmi.Path, EntryPoint = "smiGetPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = true)]
        internal static extern string SmiGetPath();

        [DllImport(LibSmi.Path, EntryPoint = "smiSetPath", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiSetPath([MarshalAs(UnmanagedType.LPStr), In] string path);

        [DllImport(LibSmi.Path, EntryPoint = "smiSetSeverity", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiSetSeverity([MarshalAs(UnmanagedType.LPStr), In] string pattern, [In] int severity);
        
        [DllImport(LibSmi.Path, EntryPoint = "smiReadConfig", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiReadConfig([MarshalAs(UnmanagedType.LPStr), In] string filename, [MarshalAs(UnmanagedType.LPStr), In] string? tag);

        // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SmiErrorHandler(
            [MarshalAs(UnmanagedType.LPStr), In] string path,
            int line, 
            int severity,
            [MarshalAs(UnmanagedType.LPStr), In] string message,
            [MarshalAs(UnmanagedType.LPStr), In] string tag);

        [DllImport(LibSmi.Path, EntryPoint = "smiSetErrorHandler", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void SmiSetErrorHandler(SmiErrorHandler errorHandler);

        [DllImport(LibSmi.Path, EntryPoint = "smiSetErrorHandler", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void SmiUnsetErrorHandler(IntPtr errorHandler);

        [DllImport(LibSmi.Path, EntryPoint = "smiExit", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int SmiExit();
    }
}