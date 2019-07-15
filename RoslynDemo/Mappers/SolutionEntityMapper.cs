using System.IO;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo.Mappers
{
    public class SolutionEntityMapper : IMapper<Solution, SolutionEntity>
    {
        public SolutionEntity Map(Solution input) => new SolutionEntity
        {
            Name = Path.GetFileName(input.Path),
            SolutionPath = input.Path
        };
    }
}