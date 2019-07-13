using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class UsingMapper : IMapper<UsingDirectiveSyntax, Using>
    {
        public Using Map(UsingDirectiveSyntax syntax) => new Using
        {
            Alias = syntax.Alias?.Name.ToString(),
            Namespace = syntax.Name.ToString()
        };
    }
}