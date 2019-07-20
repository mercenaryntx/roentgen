using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class MethodMapper : IMethodMapper
    {
        public Method Map(MethodDeclarationSyntax declaration, Class parentClass)
        {
            throw new NotSupportedException();
            //var code = declaration.ToString();
            //var r = new Regex("\r\n");
            //var symbol = parentClass.Model.GetDeclaredSymbol(declaration);
            //return new Method
            //{
            //    Declaration = declaration,
            //    ParentClass = parentClass,
            //    Symbol = symbol,
            //    Length = code.Length,
            //    Loc = r.Split(code.Trim()).Length
            //};
        }

        //private Argument Map(ParameterSyntax syntax) => new Argument
        //{
        //    Name = syntax.Identifier.ToString(),
        //    Type = syntax.Type.ToString()
        //};

    }
}
