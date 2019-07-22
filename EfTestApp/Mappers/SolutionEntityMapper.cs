using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Mappers;
using Neurotoxin.Roentgen.Models;

namespace EfTestApp.Mappers
{
    public class SolutionEntityMapper : IMapper<Solution, SolutionEntity>
    {
        public SolutionEntity Map(Solution input) => new SolutionEntity
        {
            Path = input.Path
        };
    }
}