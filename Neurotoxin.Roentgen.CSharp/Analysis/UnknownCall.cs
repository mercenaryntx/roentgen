using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class UnknownCall : LinkBase
    {
        public Method Caller { get; }
        public InvocationExpressionSyntax Invocation { get; }

        public UnknownCall(Method caller, InvocationExpressionSyntax invocation) : base(caller, invocation)
        {
            Caller = caller;
            Invocation = invocation;
        }
    }
}