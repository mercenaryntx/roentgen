using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class MethodMapper : IMethodMapper
    {
        public Method Map(MethodDeclarationSyntax declaration, Class parentClass)
        {
            var symbol = parentClass.Model.GetDeclaredSymbol(declaration);
            return new Method
            {
                Declaration = declaration,
                ParentClass = parentClass,
                Symbol = symbol
            };
        }

        //private Argument Map(ParameterSyntax syntax) => new Argument
        //{
        //    Name = syntax.Identifier.ToString(),
        //    Type = syntax.Type.ToString()
        //};

    }
}
