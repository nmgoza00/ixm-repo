using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Common.MimeTypeMapper
{

    public class MimeTypeToExtensionMapper
    {
        private static readonly Dictionary<string, List<string>> MimeTypeToExtensions =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        static MimeTypeToExtensionMapper()
        {
            // Initialize with default mappings
            var provider = new FileExtensionContentTypeProvider();

            // Reverse the default mappings
            foreach (var mapping in provider.Mappings)
            {
                if (!MimeTypeToExtensions.TryGetValue(mapping.Value, out var extensions))
                {
                    extensions = new List<string>();
                    MimeTypeToExtensions[mapping.Value] = extensions;
                }
                extensions.Add(mapping.Key);
            }

            // Add common missing mappings
            AddCustomMappings();
        }

        private static void AddCustomMappings()
        {
            AddMapping("application/json", new[] { ".json" });
            AddMapping("text/markdown", new[] { ".md" });
            AddMapping("application/octet-stream", new[] { ".bin" });
        }

        private static void AddMapping(string mimeType, IEnumerable<string> extensions)
        {
            if (!MimeTypeToExtensions.ContainsKey(mimeType))
            {
                MimeTypeToExtensions[mimeType] = new List<string>();
            }
            MimeTypeToExtensions[mimeType].AddRange(extensions);
        }

        public static List<string> GetExtensions(string mimeType)
        {
            if (MimeTypeToExtensions.TryGetValue(mimeType, out var extensions))
            {
                return extensions.Distinct().ToList();
            }
            return new List<string> { ".bin" }; // Default fallback
        }

        public static string GetPrimaryExtension(string mimeType)
        {
            var extensions = GetExtensions(mimeType);
            return extensions.FirstOrDefault() ?? ".bin";
        }
    }
}
