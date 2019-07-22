using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Mappers
{
    public interface ISolutionMapper
    {
        Solution Map(string path);
    }
}