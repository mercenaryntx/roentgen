using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Neurotoxin.ScOut.Models
{
    public class Project : FileCodePart
    {
        public Dictionary<string, SourceFile> SourceFiles => Children.Cast<SourceFile>().ToDictionary(c => c.FullName, c => c);

        public string Language { get; set; }
        public string TargetFramework { get; set; }

        public override string ToString() => FullName;
    }
}