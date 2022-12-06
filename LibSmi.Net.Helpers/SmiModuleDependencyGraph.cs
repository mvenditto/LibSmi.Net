using LibSmi.Net.Module;
using System.Diagnostics;
using System.Text;

namespace LibSmi.Net.Helpers
{
    public class SmiModuleDependency
    {
        public int Id { get; init;  }

        public static readonly SmiModuleDependency MissingDependency = new();

        public SmiModule? Module { get; set; }

        public bool IsResolved => Module != null;

        public bool IsMissing => !IsResolved || Module?.SmiIdentifier == string.Empty;

        public IEnumerable<string>? ImportedNames { get; set; }

        public ICollection<SmiModuleDependency>? Children { get; set; }
    }

    public class SmiModuleDependencyGraph
    {
        private readonly SmiModuleDependency _root;

        private SmiModuleDependencyGraph(SmiModule module)
        {
            _root = Initialize(module);
        }

        public static SmiModuleDependencyGraph Create(SmiModule module)
        {
            return new(module);
        }

        public string ToDotViz()
        {
            var nodes = new HashSet<string>();
            var edges = new HashSet<string>();

            var stack = new Stack<SmiModuleDependency>();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                var dep = stack.Pop();

                var name = dep.IsMissing
                    ? $"MISSING_{dep.Id}"
                    : dep.Module?.SmiIdentifier;

                //nodes.Add($"{name?.Replace("-", "_")} [shape=record label=\"{name}\"]");

                if (dep.Children?.Any() == false)
                {
                    continue;
                }

                foreach(var c in dep.Children!)
                {
                    var childName = c.IsMissing
                        ? $"MISSING_{c.Id}"
                        : c.Module?.SmiIdentifier?.Replace("-", "_");

                    edges.Add($"{name?.Replace("-","_")} -> {childName}");

                    stack.Push(c);
                    
                }
            }

            //var nodesS = nodes.Aggregate((a, b) => a + "\n" + b);
            var edgesS = edges.Aggregate((a, b) => a + "\n" + b);

            return $"digraph D {{\n{edgesS}\n}}";
        }

        public SmiModuleDependency Initialize(SmiModule module)
        {
            var root = new SmiModuleDependency
            {
                Id = 0,
                Module = module,
                Children = new List<SmiModuleDependency>(),
                ImportedNames = new List<string>()
            };

            var stack = new Stack<SmiModuleDependency>();

            stack.Push(root);

            var id = 1;
            var missingId = 1;

            while (stack.Count > 0)
            {
                var curr = stack.Pop();

                var imports = curr.Module?.GetModuleImports();

                if (imports == null)
                {
                    continue;
                }

                var deps = imports
                    .GroupBy(x => x.Module)
                    .Where(x => x.Key != null)
                    .ToDictionary(x => x.Key!, x => x.ToList());

                foreach(var(mod, importedNames) in deps)
                {
                    var loadResult = SmiHelpers.LoadModulePedantic(mod);

                    var missing = loadResult.Module?.SmiIdentifier != "";

                    var d = new SmiModuleDependency
                    {
                        Id = missing ? missingId++ : id++,
                        Module = loadResult.Module,
                        Children = new List<SmiModuleDependency>(),
                        ImportedNames = importedNames.Select(x => x.Name!)
                    };

                    curr.Children?.Add(d);

                    stack.Push(d);
                }
            }

            return root;
        }
    }
}