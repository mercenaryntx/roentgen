using System.Linq;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class SolutionMapper : IMapper<Microsoft.CodeAnalysis.Solution, Solution>
    {
        private readonly IMapper<Microsoft.CodeAnalysis.Project, Project> _projectMapper;

        public SolutionMapper(IMapper<Microsoft.CodeAnalysis.Project, Project> projectMapper)
        {
            _projectMapper = projectMapper;
        }

        public Solution Map(Microsoft.CodeAnalysis.Solution sln) => new Solution
        {
            Path = sln.FilePath,
            Projects = sln.Projects.Select(_projectMapper.Map).ToArray()
        };
    }
}