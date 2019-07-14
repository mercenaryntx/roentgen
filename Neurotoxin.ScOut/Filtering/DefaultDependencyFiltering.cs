using System.Collections.Generic;

namespace Neurotoxin.ScOut.Filtering
{
    public class DefaultDependencyFiltering : IDependencyFiltering
    {
        public List<string> ExcludeAssemblies { get; } = new List<string>
        {
            "mscorlib",
            "PresentationFramework"
        };
    }
}