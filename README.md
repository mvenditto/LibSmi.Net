# LibSmi.Net
.NET wrapper for [libsmi](https://gitlab.ibr.cs.tu-bs.de/nm/libsmi)

```csharp
using LibSmi.Net;

Smi.Initialize("hello-world");

const string INDENT = "  ";

try
{
    // if SMIPATH env var is not set 
    // the mibs path must be manually set
    // Smi.SetPath("path/to/mibs/dir")
    
    // retrieve the module object
    var module = Smi.GetModule("SNMPV2-SMI");
    
    // get the 'org' node
    var node = module.GetNode("org");
    
    // alternatively module.GetFirstNode could
    // have been used to retrieve the first node
    // var node = module.GetFirstNode();

    var baseLevel = (int) node.OidLen;
    var level = 0;
    
    // traverse the subtree in lexicographic order
    while (node != null)
    {
        level = (int)node.OidLen - baseLevel;

        var indent = level > 0 
            ? Enumerable.Repeat(INDENT, level).Aggregate((a,b) => a + b)
            : string.Empty;

        Console.WriteLine($"{indent}{node.Name} ({node.ToOidString()})");

        node = node.GetNextNode();
    }
}
finally
{
    Smi.Cleanup();
}
```
### Output
```
org (.1.3)
  dod (.1.3.6)
    internet (.1.3.6.1)
      directory (.1.3.6.1.1)
      mgmt (.1.3.6.1.2)
        mib-2 (.1.3.6.1.2.1)
          transmission (.1.3.6.1.2.1.10)
      experimental (.1.3.6.1.3)
      private (.1.3.6.1.4)
        enterprises (.1.3.6.1.4.1)
      security (.1.3.6.1.5)
      snmpV2 (.1.3.6.1.6)
        snmpDomains (.1.3.6.1.6.1)
        snmpProxys (.1.3.6.1.6.2)
        snmpModules (.1.3.6.1.6.3)
```
