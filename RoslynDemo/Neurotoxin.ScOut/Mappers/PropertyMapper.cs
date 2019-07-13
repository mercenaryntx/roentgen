using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class PropertyMapper : IPropertyMapper
    {
        private readonly IMapper<InvocationExpressionSyntax, Call> _callMapper;

        public PropertyMapper(IMapper<InvocationExpressionSyntax, Call> callMapper)
        {
            _callMapper = callMapper;
        }

        public Property Map(PropertyDeclarationSyntax syntax, Class parentClass)
        {
            //TODO: temporary removal
            if (syntax.ExplicitInterfaceSpecifier != null) return null;

            var accessors = syntax.DescendantNodes().OfType<AccessorListSyntax>().Single().Accessors;
            var getter = accessors.SingleOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration);
            var setter = accessors.SingleOrDefault(a => a.Kind() == SyntaxKind.SetAccessorDeclaration);
            var prop = new Property
            {
                ParentClass = parentClass,
                Name = syntax.Identifier.ToString(),
                Type = syntax.Type.ToFullString()
            };
            if (getter != null) prop.Getter = Map(getter, prop);
            if (setter != null) prop.Setter = Map(setter, prop);
            return prop;
        }

        private Accessor Map(AccessorDeclarationSyntax syntax, Property parentProperty) => new Accessor
        {
            ParentProperty = parentProperty,
            ParentClass = parentProperty.ParentClass,
            Name = $"{syntax.Keyword}_{parentProperty.Name}",
            Calls = syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Select(_callMapper.Map).ToArray()
        };
    }
}
