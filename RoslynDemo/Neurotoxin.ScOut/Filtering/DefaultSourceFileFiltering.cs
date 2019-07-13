using System.Collections.Generic;

namespace Neurotoxin.ScOut.Filtering
{
    public class DefaultSourceFileFiltering : ISourceFileFiltering
    {
        public Dictionary<string, string> IncludeRules { get; } = new Dictionary<string, string>
        {
            { "C# Files", @"\.cs$" },
        };

        public Dictionary<string, string> ExcludeRules { get; } = new Dictionary<string, string>
        {
            { "AssemblyInfo", "AssemblyInfo.cs" },
            { "AssemblyAttributes", @"[^\w]AssemblyAttributes.cs$" }
        };
    }
}