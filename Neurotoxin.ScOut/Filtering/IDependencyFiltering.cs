using System.Collections.Generic;

namespace Neurotoxin.ScOut.Filtering
{
    public interface IDependencyFiltering
    {
        List<string> ExcludeAssemblies { get; }
    }
}