using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class CallMapper : IMapper<InvocationExpressionSyntax, Call>
    {
        public Call Map(InvocationExpressionSyntax syntax) => new Call
        {
            Ref = syntax
        };
    }
}