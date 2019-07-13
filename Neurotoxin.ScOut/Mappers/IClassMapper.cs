using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public interface IClassMapper
    {
        Class Map(ClassDeclarationSyntax syntax, string @namespace, SemanticModel model);
    }
}