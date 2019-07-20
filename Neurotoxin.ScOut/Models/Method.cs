using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.ScOut.Models
{
    public class Method : CodePart
    {
        //TODO: temporary
        public MethodDeclarationSyntax Declaration { get; set; }
        public List<Method> Callers { get; } = new List<Method>();
        public List<MethodCall> InternalCalls { get; } = new List<MethodCall>();
        public List<MethodCall> ExternalCalls { get; } = new List<MethodCall>();

        public override string ToString() => FullName;
    }
}