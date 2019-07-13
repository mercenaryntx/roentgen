using System.Linq;
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

        public Class Map(ClassDeclarationSyntax syntax, string @namespace)
        {
            var name = syntax.Identifier.ToString();
            if (syntax.TypeParameterList != null)
            {
                name += syntax.TypeParameterList.ToString();
            }
            var cls = new Class
            {
                Name = name,
                Namespace = @namespace
            };
            cls.Properties = syntax.ChildNodes().OfType<PropertyDeclarationSyntax>().Select(s => _propertyMapper.Map(s, cls)).Where(p => p != null).ToDictionary(p => p.Name, p => p);
            cls.Methods = syntax.ChildNodes().OfType<MethodDeclarationSyntax>().Select(s => _methodMapper.Map(s, cls)).GroupBy(m => m.Name).ToDictionary(m => m.Key, m => m.ToArray());
            return cls;
        }
    }
}
