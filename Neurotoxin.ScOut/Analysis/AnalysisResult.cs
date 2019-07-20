using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Analysis
{
    public class AnalysisResult
    {
        public Solution[] Solutions { get; set; }
        public Dictionary<string, Class[]> Interfaces { get; set; }
        public Dictionary<string, Method> Methods { get; set; }
        public Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> Invocations { get; set; }
    }
}