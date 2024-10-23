using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Checkmarx.API.Tests.Utils
{
    public static class TestUtils
    {
        public static string ReadEmbeddedFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var assemblyAndFile = GetFileAssembly(fileName);

            Assembly detectedAssembly = assemblyAndFile.Item1;
            string resourceName = assemblyAndFile.Item2;
            using (Stream stream = detectedAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");

                // Read the embedded file's content as text
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return content;
                }
            }

            // If the resource is not found, you could either return default(T) or throw an exception
            throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }

        public static Tuple<Assembly, string> GetFileAssembly(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            string resourceName = string.Empty;
            Assembly detectedAssembly = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                resourceName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(fileName));
                if (!string.IsNullOrWhiteSpace(resourceName))
                {
                    detectedAssembly = assembly;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(resourceName) || detectedAssembly == null)
                throw new Exception($"No resource found with the name \"{fileName}\"");

            return new Tuple<Assembly, string>(detectedAssembly, resourceName);
        }
    }
}
