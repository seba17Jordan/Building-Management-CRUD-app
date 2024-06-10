using ImportersInterface;
using System.Reflection;

namespace ImportersLogic
{
    public class ImporterManager
    {
        public static IBuildingImporter GetImporter(string directoryPath, string importerName)
        {
            foreach (var file in Directory.GetFiles(directoryPath, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    // Check if the type implements IBuildingImporter
                    if (typeof(IBuildingImporter).IsAssignableFrom(type) && !type.IsInterface)
                    {
                        if (type.Name.Equals(importerName, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Importer {type.FullName} found.");
                            return (IBuildingImporter)Activator.CreateInstance(type);
                        }
                    }
                }
            }

            Console.WriteLine("No importer found");
            return null;
        }

        public static IEnumerable<string> GetAvailableImporters(string directoryPath)
        {
            var importers = new List<string>();
            var dllFiles = Directory.GetFiles(directoryPath, "*.dll");

            foreach (var file in dllFiles)
            {
                var assembly = Assembly.LoadFrom(file);
                var importerTypes = assembly.GetTypes().Where(t => typeof(IBuildingImporter).IsAssignableFrom(t) && !t.IsInterface);
                importers.AddRange(importerTypes.Select(t => t.Name));
            }

            return importers;
        }

    }
}
