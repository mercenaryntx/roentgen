using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.ScOut.Models
{
    public class Method
    {
        public Class ParentClass { get; set; }
        public MethodDeclarationSyntax Declaration { get; set; }
        public IMethodSymbol Symbol { get; set; }
        public string Name => Symbol.Name;
        public string FullName => Symbol.ToString();

        public int Length { get; set; }
        public int Loc { get; set; }

        public List<Method> Callers { get; } = new List<Method>();
        public List<MethodCall> InternalCalls { get; } = new List<MethodCall>();
        public List<MethodCall> ExternalCalls { get; } = new List<MethodCall>();

        public override string ToString() => Symbol.ToString();
    }
}