using LibSmi.Net.Module;
using LibSmi.Net.Node;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace LibSmi.Net
{
    [Collection("Sequential")]
    public class SmiNode_Native : TestBase
    {
        private readonly SmiModule _ifModule;

        public SmiNode_Native(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Smi.SetErrorLevel(0);
            Smi.SetPath(_mibsTestPath);
            _ifModule = Smi.GetModule("IF-MIB");
            Assert.NotNull(_ifModule);
        }

        [Fact]
        public void ShouldRetrieveExplicitModuleNode()
        {
            var nodeName = "ifPhysAddress";
            var node = Smi.GetNode(_ifModule, nodeName);
            Assert.NotNull(node);
            Assert.Equal(nodeName, node!.Name);
            Assert.NotNull(node.Description);
        }

        [Fact]
        public void Equality()
        {
            var nodeName = "internet";
            var a = Smi.GetNode(null, nodeName);
            var b = Smi.GetNode(null, nodeName);
            var c = Smi.GetNode(null, nodeName);
            Assert.Equal(a, b);
            Assert.Equal(b, c);
        }

        [Fact]
        public void HashCode()
        {
            var nodeName = "internet";
            var a = Smi.GetNode(null, nodeName).GetHashCode();
            var b = Smi.GetNode(null, nodeName).GetHashCode();
            var c = Smi.GetNode(null, nodeName).GetHashCode(); ;
            Assert.Equal(a, b);
            Assert.Equal(b, c);
        }
    }
}