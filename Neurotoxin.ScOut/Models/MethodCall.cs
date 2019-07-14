using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.ScOut.Models
{
    public class MethodCall
    {
        public Method Caller { get; }
        public Method Callee { get; }
        public InvocationExpressionSyntax Invocation { get; }
        public IMethodSymbol TargetSymbol { get; }

        public MethodCall(Method caller, InvocationExpressionSyntax invocation, IMethodSymbol targetSymbol = null, Method callee = null)
        {
            Caller = caller;
            Invocation = invocation;
            TargetSymbol = targetSymbol;
            Callee = callee;
        }
    }
}