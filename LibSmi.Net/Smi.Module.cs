using LibSmi.Net.Module;
using LibSmi.Net.Node;

namespace LibSmi.Net
{
    public static partial class Smi
    {
        public static unsafe SmiModule? GetModule(string module)
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            var modPtr = LibSmiNative.SmiGetModule(module);

            return SmiModule.FromPtr(modPtr);
        }

        public static unsafe SmiModule? GetFirstModule()
        {
            var modPtr = LibSmiNative.SmiGetFirstModule();

            return SmiModule.FromPtr(modPtr);
        }

        public static unsafe SmiModule? GetNextModule(this SmiModule module)
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            var modPtr = LibSmiNative.SmiGetNextModule(module.UnderlyingPtr);

            return SmiModule.FromPtr(modPtr);
        }

        public static unsafe bool IsImported(this SmiModule module, SmiModule importedModule, string node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));
            ArgumentNullException.ThrowIfNull(module, nameof(module));
            ArgumentNullException.ThrowIfNull(importedModule, nameof(importedModule));

            var imported = LibSmiNative.SmiIsImported(
                module.UnderlyingPtr,
                importedModule.UnderlyingPtr,
                node);

            return imported == 1;
        }

        public static unsafe SmiRevision? GetFirstRevision(this SmiModule module)
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            var revPtr = LibSmiNative.SmiGetFirstRevision(module.UnderlyingPtr);

            return SmiRevision.FromPtr(revPtr);
        }

        public static unsafe SmiRevision? GetNextRevision(this SmiRevision revision)
        {
            ArgumentNullException.ThrowIfNull(revision, nameof(revision));

            var revPtr = LibSmiNative.SmiGetNextRevision(revision.UnderlyingPtr);

            return SmiRevision.FromPtr(revPtr);
        }

        public static unsafe int GetRevisionLine(this SmiRevision revision)
        {
            ArgumentNullException.ThrowIfNull(revision, nameof(revision));

            return LibSmiNative.SmiGetRevisionLine(revision.UnderlyingPtr);
        }

        public static unsafe SmiImport? GetFirstImport(this SmiModule module)
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            var importPtr = LibSmiNative.SmiGetFirstImport(module.UnderlyingPtr);

            return SmiImport.FromPtr(importPtr);
        }

        public static unsafe SmiImport? GetNextImport(this SmiImport import)
        {
            ArgumentNullException.ThrowIfNull(import, nameof(import));

            var importPtr = LibSmiNative.SmiGetNextImport(import.UnderlyingPtr);

            return SmiImport.FromPtr(importPtr);
        }

        
        public static unsafe SmiNode? GetIdentityNode(this SmiModule module)
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            var nodePtr = LibSmiNative.SmiGetModuleIdentityNode(module.UnderlyingPtr);

            return SmiNode.FromPtr((SmiNodeStruct*) nodePtr);
        }
    }
}
