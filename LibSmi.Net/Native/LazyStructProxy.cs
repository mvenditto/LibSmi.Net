using System.Runtime.InteropServices;

namespace LibSmi.Net
{
    public abstract unsafe class LazyStructProxy
    {
        protected readonly void* _nativePtr;

        protected LazyStructProxy(void* nativePtr)
        {
            _nativePtr = nativePtr;
        }

        protected static Lazy<string?> LazyAnsiStringMarshal(IntPtr strPtr)
        {
            return new(() =>
            {
                try
                {
                    return Marshal.PtrToStringAnsi(strPtr);
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            });
        }
    }
}