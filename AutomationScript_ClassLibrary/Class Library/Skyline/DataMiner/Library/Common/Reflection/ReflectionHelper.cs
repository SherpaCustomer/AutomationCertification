namespace Skyline.DataMiner.Library.Common.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<string, List<Assembly>> clpCashedAssemblies = new ConcurrentDictionary<string, List<Assembly>>();

        public static List<Assembly> GetLoadedAssemblies()
        {
            var clpAssembly = typeof(ReflectionHelper).Assembly.GetName().FullName;
            return clpCashedAssemblies.GetOrAdd(clpAssembly, _ => Load(clpAssembly));
        }

        private static List<Assembly> Load(string clpAssembly)
        {
            List<Assembly> loadedAssemblies = new List<Assembly>();
            System.Diagnostics.Debug.WriteLine("CLP - InterApp - Load Assemblies");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (
                    assembly.ManifestModule.Name != "<In Memory Module>"
                    && !assembly.FullName.StartsWith("System", StringComparison.Ordinal)
                    && !assembly.FullName.StartsWith("Microsoft", StringComparison.Ordinal)
                    && !assembly.FullName.StartsWith("mscorlib", StringComparison.Ordinal)
                    && !assembly.FullName.StartsWith("SLNetTypes", StringComparison.Ordinal)
                    && !assembly.FullName.StartsWith("SLManagedScripting", StringComparison.Ordinal)
                    && !assembly.FullName.StartsWith("Newtonsoft.Json", StringComparison.Ordinal)
                    && assembly.Location.IndexOf("App_Web", StringComparison.Ordinal) == -1
                    && assembly.Location.IndexOf("App_global", StringComparison.Ordinal) == -1
                    && assembly.FullName.IndexOf("CppCodeProvider", StringComparison.Ordinal) == -1
                    && assembly.FullName.IndexOf("WebMatrix", StringComparison.Ordinal) == -1
                    && assembly.FullName.IndexOf("SMDiagnostics", StringComparison.Ordinal) == -1)
                {
                    var referencesClpAssembly = assembly.GetReferencedAssemblies().Select(p => p.FullName).Contains(clpAssembly);
                    var thisAssembly = assembly.GetName().FullName;

                    if (referencesClpAssembly || thisAssembly == clpAssembly
                    )
                    {
                        loadedAssemblies.Add(assembly);
                    }
                }
            }

            return loadedAssemblies;
        }
    }
}