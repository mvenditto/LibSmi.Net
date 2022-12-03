namespace LibSmi.Net
{
    internal static class LibSmi
    {
#if OS_WINDOWS
        public const string Path = "libsmi-2.dll";
#elif OS_LINUX
        public const string Path = "libsmi.so";
#endif
    }
}