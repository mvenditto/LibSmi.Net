using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace LibSmi.Net
{
    [Collection("Sequential")]
    public class SmiConfig_Native: TestBase
    {
        public SmiConfig_Native(ITestOutputHelper testOutputHelper): base(testOutputHelper)
        {
            // avoid crashing e.g on Smi.GetModule failed to locate MIB module: ...
            Smi.SetErrorLevel(0);
            Smi.SetPath(_mibsTestPath);
        }

        [Fact]
        public void SmiGetSetUserFlags()
        {
            var userFlags = Smi.GetFlags();
            Assert.Equal(0u, (uint) userFlags);
            Smi.SetFlags(SmiFlags.Errors | SmiFlags.Stats);
            userFlags = Smi.GetFlags();
            Assert.True(userFlags.HasFlag(SmiFlags.Errors));
            Assert.True(userFlags.HasFlag(SmiFlags.Stats));
        }

        [Fact]
        public void SmiShouldLoadModule()
        {
            Assert.True(Smi.LoadModule("IF-MIB"));
            Assert.True(Smi.IsModuleLoaded("IF-MIB"));
            Assert.False(Smi.IsModuleLoaded("SNMPv2-SMI"));
        }

        [Fact]
        public void SmiShouldLoadModuleFromFullPath()
        {
            var mib = "SNMP-TARGET-MIB";
            var ifMib = Path.Combine(_mibsTestPath, mib);
            var module = Smi.GetModule(ifMib);
            Assert.Equal(mib, module.SmiIdentifier);
        }

        [Fact]
        public void SmiLoadModuleShouldBeNullIfNotFound()
        {
            Assert.False(Smi.LoadModule("NOT-EXISTING-MIB"));
        }

        [Fact]
        public void SmiReadConfigShouldApplyConfig()
        {
            var configLines = File.ReadAllLines(_smiTestConfig);

            Smi.LoadConfiguration(_smiTestConfig,  null);

            // check that any load <module> command in config has been executed
            var loads = configLines
                .Where(l => l.StartsWith("load "))
                .Select(l => l.Split())
                .Where(x => x.Length == 2)
                .Select(x => x[1]);

            foreach(var load in loads)
            {
                Assert.True(Smi.IsModuleLoaded(load));
            }
        }
    }
}