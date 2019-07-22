using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Mappers
{
    public interface IProjectMapper
    {
        Project Map(Microsoft.CodeAnalysis.Project project);
    }
}