using Neurotoxin.Roentgen.Models;

namespace Neurotoxin.Roentgen.Mappers
{
    public interface IProjectMapper
    {
        Project Map(Microsoft.CodeAnalysis.Project project);
    }
}