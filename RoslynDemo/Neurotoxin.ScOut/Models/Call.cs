using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.ScOut.Models
{
    public class Call
    {
        public InvocationExpressionSyntax Ref { get; set; }
        public Method Target { get; set; }
    }
}