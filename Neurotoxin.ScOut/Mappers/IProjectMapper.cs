using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public interface IProjectMapper
    {
        Project Map(Microsoft.CodeAnalysis.Project project);
    }
}