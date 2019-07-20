using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class ClassMapper : IClassMapper
    {
        private readonly IPropertyMapper _propertyMapper;
        private readonly IMethodMapper _methodMapper;

        public ClassMapper(IPropertyMapper propertyMapper, IMethodMapper methodMapper)
        {
            _propertyMapper = propertyMapper;
            _methodMapper = methodMapper;
        }

        public Class Map(ClassDeclarationSyntax syntax, SemanticModel model)
        {
            throw new NotSupportedException();
            //var code = syntax.ToString();
            //var r = new Regex("\r\n");
            //var cls = new Class
            //{
            //    Model = model,
            //    Symbol = (INamedTypeSymbol)model.GetDeclaredSymbol(syntax),
            //    Length = code.Length,
            //    Loc = r.Split(code.Trim()).Length
            //};
            //cls.Properties = syntax.ChildNodes().OfType<PropertyDeclarationSyntax>().Select(s => _propertyMapper.Map(s, cls)).Where(p => p != null).ToDictionary(p => p.Name, p => p);
            //cls.Methods = syntax.ChildNodes().OfType<MethodDeclarationSyntax>().Select(s => _methodMapper.Map(s, cls)).GroupBy(m => m.Name).ToDictionary(m => m.Key, m => m.ToArray());
            //return cls;
        }
    }
}
