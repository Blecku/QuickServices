using System.Reflection;

namespace QuickServices;

internal static class AssemblyScanner
{
    public static HashSet<Assembly> GetReferencingAssemblies(Assembly root, Assembly searching)
    {
        var targetName = searching.GetName().Name!;
        var cache = new Dictionary<Assembly, bool>();

        bool Visit(Assembly assembly)
        {
            if (cache.TryGetValue(assembly, out var cached)) return cached;
            cache[assembly] = false; // break cycles

            var referencesTarget = false;
            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                if (reference.Name == targetName)
                {
                    referencesTarget = true;
                    continue;
                }

                try
                {
                    if (Visit(Assembly.Load(reference)))
                        referencesTarget = true;
                }
                catch { }
            }

            cache[assembly] = referencesTarget;
            return referencesTarget;
        }

        Visit(root);

        var result = new HashSet<Assembly>();
        foreach (var entry in cache)
            if (entry.Value) result.Add(entry.Key);

        result.Add(root);
        result.RemoveWhere(a => a.GetName().Name == targetName);
        return result;
    }
}
