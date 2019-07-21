using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class SourceFile : FileCodePart
    {
        public SemanticModel Model { get; set; }
        public bool IsGenerated { get; set; }
        public Dictionary<string, Class> Classes => Children.Cast<Class>().ToDictionary(c => c.FullName, c => c);
    }
}