using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Scanning;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Patterns
{
    public interface IDependencyScanner
    {
        string[] MethodCall { get; }
        string Scan(MethodCall call, Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> invocations);
    }
}