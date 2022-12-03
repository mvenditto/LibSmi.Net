using LibSmi.Net.Module;
using LibSmi.Net.Node;

namespace LibSmi.Net
{
    public static partial class Smi
    {
        public static unsafe SmiNode? GetNode(this SmiModule? module, string node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var modulePtr = module != null 
                ? module.UnderlyingPtr
                : (SmiModuleStruct*)IntPtr.Zero;

            var nodePtr = LibSmiNative.SmiGetNode(modulePtr, node);

            return SmiNode.FromPtr(nodePtr);
        }
        public static unsafe SmiNode? GetFirstNode(this SmiModule module, SmiNodeKind kinds=SmiNodeKind.Any)
        {
            ArgumentNullException.ThrowIfNull(module);
            ArgumentNullException.ThrowIfNull(kinds);

            var nodePtr = LibSmiNative.SmiGetFirstNode(module.UnderlyingPtr, kinds);

            return SmiNode.FromPtr(nodePtr);
        }
        public static unsafe SmiNode? GetNextNode(this SmiNode node, SmiNodeKind kinds=SmiNodeKind.Any)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(kinds);

            var nodePtr = LibSmiNative.SmiGetNextNode(node.UnderlyingPtr, kinds);

            return SmiNode.FromPtr(nodePtr);
        }

        public static unsafe SmiNode? GetParentNode(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var nodePtr = LibSmiNative.SmiGetParentNode(node.UnderlyingPtr);

            return SmiNode.FromPtr(nodePtr);
        }

        public static unsafe SmiNode? GetRelatedNode(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var nodePtr = LibSmiNative.SmiGetRelatedNode(node.UnderlyingPtr);

            return SmiNode.FromPtr(nodePtr);
        }

        public static unsafe SmiNode? GetFirstChildNode(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var nodePtr = LibSmiNative.SmiGetFirstChildNode(node.UnderlyingPtr);

            return SmiNode.FromPtr(nodePtr);
        }
        public static unsafe SmiNode? GetNextChildNode(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var nodePtr = LibSmiNative.SmiGetNextChildNode(node.UnderlyingPtr);

            return SmiNode.FromPtr(nodePtr);
        }
        public static unsafe SmiModule? GetModule(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var modPtr = LibSmiNative.SmiGetNodeModule(node.UnderlyingPtr);

            return SmiModule.FromPtr(modPtr);
        }

        public static unsafe SmiNode? GetNodeByOid(uint[] oid)
        {
            ArgumentNullException.ThrowIfNull(oid);

            var nodePtr = LibSmiNative.SmiGetNodeByOid((uint) oid.Length, oid);

            return SmiNode.FromPtr(nodePtr);
        }

        public static unsafe SmiNode? GetNodeByOid(string oid)
        {
            ArgumentNullException.ThrowIfNull(oid);

            uint[] oid_ = Array.Empty<uint>();

            try
            {
                oid_ = oid.Split(".").Select(uint.Parse).ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid input OID.", ex);
            }

            var nodePtr = LibSmiNative.SmiGetNodeByOid((uint)oid.Length, oid_);

            return SmiNode.FromPtr(nodePtr);
        }

        public static unsafe SmiType? GetNodeType(this SmiNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var typePtr = LibSmiNative.SmiGetNodeType(node.UnderlyingPtr);

            return SmiType.FromPtr(typePtr);
        }

        public static unsafe int GetNodeLine(this SmiNode node)
        {
            return LibSmiNative.SmiGetNodeLine(node.UnderlyingPtr);
        }
    }
}
