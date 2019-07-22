using Neurotoxin.Roentgen.Models;

namespace Neurotoxin.Roentgen.Mappers
{
    public interface ISolutionMapper
    {
        Solution Map(string path);
    }
}