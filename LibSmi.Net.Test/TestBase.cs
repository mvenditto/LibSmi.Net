using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace LibSmi.Net
{
    public abstract class TestBase: IDisposable
    {
        protected readonly ITestOutputHelper _testOutput;
        protected readonly string _mibsTestPath;
        protected readonly string _smiTestConfig;

        protected TestBase(ITestOutputHelper testOutputHelper)
        {
            _testOutput = testOutputHelper;

            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);
            _smiTestConfig = Path.Combine(path, "smi.conf");
            _mibsTestPath = Path.Combine(path, "mibs");
            Smi.Initialize("test");
        }

        public void Dispose()
        {
            Smi.Cleanup();
            GC.SuppressFinalize(this);
        }
    }
}