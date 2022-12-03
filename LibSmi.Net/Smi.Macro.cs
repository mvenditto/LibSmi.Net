using LibSmi.Net.Macro;
using LibSmi.Net.Module;

namespace LibSmi.Net
{
    public static partial class Smi
    {
        public static unsafe SmiMacro? GetMacro(this SmiModule module, string macro)
        {
            ArgumentNullException.ThrowIfNull(module);
            ArgumentNullException.ThrowIfNull(macro);

            var macroPtr = LibSmiNative.SmiGetMacro(module.UnderlyingPtr, macro);

            return SmiMacro.FromPtr(macroPtr);
        }
        public static unsafe SmiMacro? GetFirstMacro(this SmiModule module)
        {
            ArgumentNullException.ThrowIfNull(module);

            var macroPtr = LibSmiNative.SmiGetFirstMacro(module.UnderlyingPtr);

            return SmiMacro.FromPtr(macroPtr);
        }
        public static unsafe SmiMacro? GetNextMacro(this SmiMacro macro)
        {
            ArgumentNullException.ThrowIfNull(macro);

            var macroPtr = LibSmiNative.SmiGetNextMacro(macro.UnderlyingPtr);

            return SmiMacro.FromPtr(macroPtr);
        }

        public static unsafe SmiModule? GetMacroModule(this SmiMacro macro)
        {
            ArgumentNullException.ThrowIfNull(macro);

            var modulePtr = LibSmiNative.SmiGetMacroModule(macro.UnderlyingPtr);

            return SmiModule.FromPtr(modulePtr);
        }
    }
}
