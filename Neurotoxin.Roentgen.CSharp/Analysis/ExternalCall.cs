using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class ExternalCall : UnknownCall
    {
        public IMethodSymbol CalleeSymbol { get; }

        public ExternalCall(Method caller, IMethodSymbol symbol, InvocationExpressionSyntax invocation) : base(caller, invocation)
        {
            CalleeSymbol = symbol;
        }
    }
}