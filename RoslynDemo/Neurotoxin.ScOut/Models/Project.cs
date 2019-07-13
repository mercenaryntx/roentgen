using System.Collections.Generic;

namespace Neurotoxin.ScOut.Models
{
    public class Project
    {
        public string Path { get; set; }
        public string Language { get; set; }
        public string LanguageVersion { get; set; }
        public string TargetFramework { get; set; }
        public Dictionary<string, Class> Classes { get; set; }

        public override string ToString() => Path;
    }
}