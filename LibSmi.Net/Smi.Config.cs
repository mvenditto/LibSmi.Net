using LibSmi.Net.Config;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static LibSmi.Net.LibSmiNative;

namespace LibSmi.Net
{
    public static partial class Smi
    {
        private static bool _initialized;

        public static void Initialize(string? tag)
        {
            var retcode = LibSmiNative.SmiInit(tag);

            if (retcode != 0)
            {
                throw new SmiException(nameof(LibSmiNative.SmiInit), retcode);
            }

            _initialized = true;
        }

        public static void Initialize(SmiConfig config)
        {
            ArgumentNullException.ThrowIfNull(config);

            Initialize(config.Tag);

            if (config.UserFlags != null)
            {
                LibSmiNative.SmiSetFlags(config.UserFlags.Value);
            }

            if (config.ErrorLevel > 0)
            {
                LibSmiNative.SmiSetErrorLevel(config.ErrorLevel);
            }

            if (!string.IsNullOrEmpty(config.SmiPath))
            {
                var retcode = LibSmiNative.SmiSetPath(config.SmiPath);

                if (retcode != 0)
                {
                    throw new SmiException(nameof(LibSmiNative.SmiSetPath), retcode);
                }
            }
        }

        public static void Initialize(string smiConfigPath, string? tag)
        {
            ArgumentNullException.ThrowIfNull(smiConfigPath);

            Initialize(tag);

            LoadConfiguration(smiConfigPath, tag);
        }

        private static void ThrowIfNotInitialized()
        {
            if (_initialized == false)
            {
                throw new InvalidOperationException(
                    "SMI has not been initialized. Perhaps Smi.Initialize(...) was not called?");
            }
        }

        public static void LoadConfiguration(string smiConfigPath, string? tag)
        {
            ThrowIfNotInitialized();

            ArgumentNullException.ThrowIfNull(smiConfigPath);

            var retcode = LibSmiNative.SmiReadConfig(smiConfigPath, tag);

            if (retcode != 0)
            {
                throw new SmiException(nameof(LibSmiNative.SmiReadConfig), retcode);
            }
        }

        public static string GetPath()
        {
            ThrowIfNotInitialized();

            return LibSmiNative.SmiGetPath();
        }

        public static void SetPath(string smiPath)
        {
            ThrowIfNotInitialized();

            ArgumentNullException.ThrowIfNull(smiPath);

            var retcode = LibSmiNative.SmiSetPath(smiPath);

            if (retcode != 0)
            {
                throw new SmiException(nameof(LibSmiNative.SmiSetPath), retcode);
            }
        }

        public static void SetErrorLevel(int errorLevel)
        {
            ThrowIfNotInitialized();

            if (errorLevel < 0 || errorLevel > 9)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(errorLevel)} must be in range 0-9 but was {errorLevel}");
            }

            LibSmiNative.SmiSetErrorLevel(errorLevel);
        }

        public static SmiFlags GetFlags()
        {
            ThrowIfNotInitialized();

            return LibSmiNative.SmiGetFlags();
        }

        public static void SetFlags(SmiFlags flags)
        {
            ThrowIfNotInitialized();

            ArgumentNullException.ThrowIfNull(flags);

            LibSmiNative.SmiSetFlags(flags);
        }

        public static string? LoadModule(string module)
        {
            ThrowIfNotInitialized();

            ArgumentNullException.ThrowIfNull(module);

            var modNamePtr =  LibSmiNative.SmiLoadModule(module);

            if (modNamePtr == IntPtr.Zero)
            {
                return null;
            }

            return Marshal.PtrToStringAnsi(modNamePtr);
        }

        public static bool IsModuleLoaded(string module)
        {
            ThrowIfNotInitialized();

            ArgumentNullException.ThrowIfNull(module);

            return LibSmiNative.SmiIsLoaded(module) > 0;
        }

        public static void Cleanup()
        {
            ThrowIfNotInitialized();

            _ = LibSmiNative.SmiExit();

            _initialized = false;
        }

        public delegate void SmiErrorEventHandler(object sender, SmiErrorEventArgs e);

        // Declare the event.
        public static event SmiErrorEventHandler? OnError;

        private static void ErrorHandler(string path, int line, int severity, string message, string tag)
        {
            OnError?.Invoke(null, new()
            {
                Path = path,
                Line = line,
                Severity = severity,
                Message = message,
                Tag = tag
            });
        }

        private static SmiErrorHandler ErrorHandlerRef = ErrorHandler;

        public static void EnableErrorHandler()
        {
            LibSmiNative.SmiSetErrorHandler(ErrorHandler);
        }

        public static void DisableErrorHandler()
        {
            LibSmiNative.SmiUnsetErrorHandler(IntPtr.Zero);
        }
    }
}
