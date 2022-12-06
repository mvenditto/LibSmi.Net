using LibSmi.Net.Error;
using LibSmi.Net.Module;

namespace LibSmi.Net.Helpers
{
    public static class SmiHelpers
    {

        public static IEnumerable<SmiImport> GetModuleImports(this SmiModule module)
        {
            var import = module.GetFirstImport();

            if (import == null)
            {
                return Enumerable.Empty<SmiImport>();
            }

            var moduleImports = new List<SmiImport>();

            while (import != null)
            {
                moduleImports.Add(import);
                import = import.GetNextImport();
            }

            return moduleImports;
        }

        public static IEnumerable<SmiRevision> GetModuleRevisions(this SmiModule module)
        {
            var revision = module.GetFirstRevision();

            if (revision == null)
            {
                return Enumerable.Empty<SmiRevision>();
            }

            var moduleRevisions = new List<SmiRevision>();

            while (revision != null)
            {
                moduleRevisions.Add(revision);
                revision = revision.GetNextRevision();
            }

            return moduleRevisions;
        }

        public static SmiModuleDependencyGraph BuildModuleDependencyGraph(this SmiModule module)
        {
            return SmiModuleDependencyGraph.Create(module);
        }

        public static SmiModuleLoadResult LoadModulePedantic(
            string modulePath,
            int loadErrorLevel = 3,
            int restoreErrorLevel = 0,
            bool strict = false)
        {
            var flags = Smi.GetFlags();
            var errors = new List<SmiError>();

            void ErrorHandler(object sender, SmiErrorEventArgs args)
            {
                errors.Add(args);
            }

            try
            {
                Smi.SetErrorLevel(loadErrorLevel);
                Smi.SetFlags(flags | SmiFlags.Errors | SmiFlags.Recursive);
                Smi.EnableErrorHandler();
                Smi.OnError += ErrorHandler;

                var module = Smi.GetModule(modulePath);

                if (strict)
                {
                    var firstError = errors.FirstOrDefault(e => e.Severity < 3);

                    if (firstError != null)
                    {
                        throw new SmiMibParsingException(firstError);
                    }
                }

                var result = new SmiModuleLoadResult
                {
                    Module = module,
                    Errors = errors
                };

                return result;

            }
            catch
            {
                return new();
            }
            finally
            {
                Smi.SetErrorLevel(restoreErrorLevel);
                Smi.SetFlags(flags);
                // Smi.DisableErrorHandler();
                Smi.OnError -= ErrorHandler;
            }
        }
    }
}
