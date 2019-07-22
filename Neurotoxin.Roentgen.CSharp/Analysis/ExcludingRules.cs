using System.Collections.Generic;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class ExcludingRules
    {
        public List<string> ExcludeFiles { get; } = new List<string>
        {
            @"AssemblyInfo.cs$",
            @"[^\w]AssemblyAttributes.cs$",
            @"Settings.Designer.cs$",
            @" References\\.*?\\Reference.cs$" //TODO: temporary filtering only
        };

        public List<string> ExcludeLibraries { get; } = new List<string>
        {
            "mscorlib",
            "PresentationFramework"
        };
    }
}