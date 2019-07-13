using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public interface IPropertyMapper
    {
        Property Map(PropertyDeclarationSyntax syntax, Class parent);
    }
}