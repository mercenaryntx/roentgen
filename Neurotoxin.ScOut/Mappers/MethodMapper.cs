using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class MethodMapper : IMethodMapper
    {
        private readonly IMapper<InvocationExpressionSyntax, Call> _callMapper;

        public MethodMapper(IMapper<InvocationExpressionSyntax, Call> callMapper)
        {
            _callMapper = callMapper;
        }

        public Method Map(MethodDeclarationSyntax syntax, Class parentClass) => new Method
        {
            ParentClass = parentClass,
            Name = syntax.Identifier.ToString(),
            Type = syntax.ReturnType.ToFullString(),
            TypeParameters = syntax.TypeParameterList?.Parameters.Select(p => p.Identifier.ValueText).ToArray(),
            Arguments = syntax.ParameterList.Parameters.Select(Map).ToArray(),
            Calls = syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Select(_callMapper.Map).ToArray()
        };

        private Argument Map(ParameterSyntax syntax) => new Argument
        {
            Name = syntax.Identifier.ToString(),
            Type = syntax.Type.ToString()
        };

    }
}
