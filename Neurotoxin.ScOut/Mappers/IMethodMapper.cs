using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public interface IMethodMapper
    {
        Method Map(MethodDeclarationSyntax declaration, Class parent);
    }
}