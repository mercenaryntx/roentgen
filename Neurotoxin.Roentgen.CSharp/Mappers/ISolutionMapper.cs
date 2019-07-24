using System.Collections.Generic;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Mappers
{
    public interface ISolutionMapper
    {
        Solution Map(string path);
        IEnumerable<Solution> Map(IEnumerable<string> solutions);
    }
}