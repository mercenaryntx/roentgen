using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class InternalCall : UnknownCall
    {
        public Method Callee { get; }

        public InternalCall(Method caller, Method callee, InvocationExpressionSyntax invocation) : base(caller, invocation)
        {
            Callee = callee;
        }
    }
}