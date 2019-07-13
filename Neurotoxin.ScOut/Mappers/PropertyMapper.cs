using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class PropertyMapper : IPropertyMapper
    {
        public Property Map(PropertyDeclarationSyntax declaration, Class parentClass)
        {
            //TODO: temporary removal
            if (declaration.ExplicitInterfaceSpecifier != null) return null;

            //var accessors = syntax.DescendantNodes().OfType<AccessorListSyntax>().Single().Accessors;
            //var getter = accessors.SingleOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration);
            //var setter = accessors.SingleOrDefault(a => a.Kind() == SyntaxKind.SetAccessorDeclaration);
            var symbol = parentClass.Model.GetDeclaredSymbol(declaration);
            var prop = new Property
            {
                ParentClass = parentClass,
                Symbol = symbol
                //Name = syntax.Identifier.ToString(),
                //Type = syntax.Type.ToFullString()
            };
            //if (getter != null) prop.Getter = Map(getter, prop);
            //if (setter != null) prop.Setter = Map(setter, prop);
            return prop;
        }

        //private Accessor Map(AccessorDeclarationSyntax declaration, Property parentProperty) => new Accessor
        //{
        //    ParentProperty = parentProperty,
        //    ParentClass = parentProperty.ParentClass,
        //    Name = $"{declaration.Keyword}_{parentProperty.Name}",
        //    Declaration = declaration
        //};
    }
}
