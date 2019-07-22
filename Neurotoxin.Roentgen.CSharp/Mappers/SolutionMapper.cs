using System.Linq;
using Microsoft.CodeAnalysis.MSBuild;
using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Mappers
{
    public class SolutionMapper : ISolutionMapper
    {
        private readonly IProjectMapper _projectMapper;
        private readonly AnalysisWorkspace _workspace;

        public SolutionMapper(IProjectMapper projectMapper, AnalysisWorkspace workspace)
        {
            _projectMapper = projectMapper;
            _workspace = workspace;
        }

        public Solution Map(string path)
        {
            var workspace = MSBuildWorkspace.Create();
            var sln = workspace.OpenSolutionAsync(path).GetAwaiter().GetResult();
            var solution = new Solution
            {
                FullName = sln.FilePath,
                Children = sln.Projects.Select(_projectMapper.Map).Cast<ICodePart>().ToList()
            };
            _workspace.Register(solution);
            return solution;
        }
    }
}