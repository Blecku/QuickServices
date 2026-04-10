using System.Reflection;

namespace QuickServices.Core
{
    internal static class AssemblyScanner
    {
        public static HashSet<Assembly> GetReferencingAssemblies(Assembly root, Assembly searching)
        {
            var quickServicesName = searching.GetName().Name!;
            var cache = new Dictionary<Assembly, bool>();

            bool Visit(Assembly assembly)
            {
                if (cache.TryGetValue(assembly, out var cached)) return cached;
                cache[assembly] = false;

                var referencesQuickServices = false;
                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (reference.Name == quickServicesName)
                    {
                        referencesQuickServices = true;
                        continue;
                    }

                    try
                    {
                        if (Visit(Assembly.Load(reference)))
                            referencesQuickServices = true;
                    }
                    catch { }
                }

                cache[assembly] = referencesQuickServices;
                return referencesQuickServices;
            }

            Visit(root);

            var result = new HashSet<Assembly>();
            foreach (var entry in cache)
                if (entry.Value) result.Add(entry.Key);

            result.Add(root);
            result.RemoveWhere(a => a.GetName().Name == quickServicesName);
            return result;
        }
    }
}
