using Xunit;
using Xunit.Abstractions;

namespace LibSmi.Net
{
    [Collection("Sequential")]
    public class SmiModule_Native: TestBase
    {
        public SmiModule_Native(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            // avoid crashing e.g on Smi.GetModule failed to locate MIB module: ...
            Smi.SetErrorLevel(0);
            Smi.SetPath(_mibsTestPath);
        }

        [Fact]
        public void SmiGetModuleShouldReturnExistingModule()
        {
            var module = Smi.GetModule("IF-MIB");
            Assert.NotNull(module);
        }
        
        [Fact]
        public void SmiGetModuleShouldReturnNullModuleNotFound()
        {
            var module = Smi.GetModule("NOT-EXISTING-MIB");
            Assert.Null(module);
        }

        [Fact]
        public void SmiGetShouldGetImports()
        {
            var module = Smi.GetModule("IF-MIB");
            Assert.NotNull(module);
            Assert.Equal("IF-MIB", module.SmiIdentifier);
            var import = module.GetFirstImport();
            Assert.Equal("SNMPv2-SMI", import.Module);
            Assert.Equal("MODULE-IDENTITY", import.Name);
            var nextImport = import.GetNextImport();
            Assert.Equal("SNMPv2-SMI", nextImport.Module);
            Assert.Equal("OBJECT-TYPE", nextImport.Name);
        }

        [Fact]
        public void SmiIsImported()
        {
            var ifModule = Smi.GetModule("IF-MIB");
            Assert.NotNull(ifModule);
            Assert.Equal("IF-MIB", ifModule.SmiIdentifier);

            var snmpV2SmiModule = Smi.GetModule("SNMPv2-SMI");
            Assert.Equal("SNMPv2-SMI", snmpV2SmiModule.SmiIdentifier);

            Assert.True(Smi.IsImported(ifModule, snmpV2SmiModule, "Counter32"));
        }

        [Fact]
        public void SmiRetrieveRevisions()
        {
            var ifModule = Smi.GetModule("IF-MIB"); 
            Assert.NotNull(ifModule);
            Assert.Equal("IF-MIB", ifModule.SmiIdentifier);

            // first revision == most recent revision
            var rev = Smi.GetFirstRevision(ifModule);
            var lastRev = long.MaxValue;

            do
            {
                Assert.True(rev.Date < lastRev);
                lastRev = rev.Date;
                rev = rev.GetNextRevision();
            }
            while (rev != null);
        }

        [Fact]
        public void SmiRetrieveModuleNode()
        {
            var mibPath = Path.Combine(_mibsTestPath, "IDRAC-MIB-SMIv2");
            var snmpV2SmiModule = Smi.GetModule(mibPath);
            Assert.NotNull(snmpV2SmiModule);
            Assert.Equal("IDRAC-MIB-SMIv2", snmpV2SmiModule.SmiIdentifier);


            var idNode = Smi.GetIdentityNode(snmpV2SmiModule);
            Assert.NotNull(snmpV2SmiModule);
            Assert.Equal(SmiDecl.ModuleIdentity, idNode.Decl);
            Assert.Equal("outOfBandGroup", idNode.Name);

            Assert.Equal(".1.3.6.1.4.1.674.10892.5", idNode.ToOidString());
        }
    }
}