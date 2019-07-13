using System.Collections.Generic;

namespace Neurotoxin.ScOut.Filtering
{
    public interface ISourceFileFiltering
    {
        Dictionary<string, string> IncludeRules { get; }
        Dictionary<string, string> ExcludeRules { get; }
    }
}