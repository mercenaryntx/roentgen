using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Filtering;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;
using Solution = Neurotoxin.ScOut.Models.Solution;

namespace Neurotoxin.ScOut
{
    public class RoslynAnalyzerOld
    {
        private readonly IMapper<Microsoft.CodeAnalysis.Solution, Solution> _solutionMapper;
        private readonly IDependencyFiltering _dependencyFiltering;

        private Solution _solution;

        static RoslynAnalyzerOld()
        {
            MSBuildLocator.RegisterDefaults();
        }

        public RoslynAnalyzerOld(IMapper<Microsoft.CodeAnalysis.Solution, Solution> solutionMapper, IDependencyFiltering dependencyFiltering)
        {
            _solutionMapper = solutionMapper;
            _dependencyFiltering = dependencyFiltering;
        }

        public RoslynAnalyzerOld LoadSolution(string path)
        {
            var workspace = MSBuildWorkspace.Create();
            var sln = workspace.OpenSolutionAsync(path).GetAwaiter().GetResult();
            //TODO: logger
            //foreach (var log in workspace.Diagnostics.Where(d => d.Kind == WorkspaceDiagnosticKind.Failure))
            //{
            //    Console.WriteLine(log);
            //}
            _solution = _solutionMapper.Map(sln);
            return this;
        }

        //public AnalysisResult Analyze()
        //{
        //    var interfaces = _solution.Classes.Values.SelectMany(c => c.Implements.Select(i => new {i, c})).GroupBy(p => p.i).ToDictionary(g => g.Key, g => g.Select(p => p.c).ToArray());
        //    var methods = _solution.Classes.Values.SelectMany(c => c.Methods.SelectMany(m => m.Value)).ToDictionary(m => m.FullName, m => m);
        //    var invocations = new Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>>();
        //    foreach (var method in methods.Values)
        //    {
        //        foreach (var invocation in method.Declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
        //        {
        //            var info = method.ParentClass.Model.GetSymbolInfo(invocation);
        //            if (info.Symbol == null)
        //            {
        //                //TODO: log/handle anomalies
        //                method.ExternalCalls.Add(new MethodCall(method, invocation));
        //                continue;
        //            }

        //            var callSymbol = info.Symbol as IMethodSymbol;
        //            if (callSymbol.IsGenericMethod) callSymbol = callSymbol.OriginalDefinition;

        //            if (_dependencyFiltering.ExcludeAssemblies.Contains(callSymbol.ContainingType.ContainingAssembly.Name)) continue;

        //            if (!Equals(callSymbol.ContainingType, callSymbol.ContainingSymbol)) Debugger.Break();

        //            if (methods.ContainsKey(callSymbol.ToString()))
        //            {
        //                var callee = methods[callSymbol.ToString()];
        //                callee.Callers.Add(method);
        //                method.InternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition, callee));
        //                if (invocations.ContainsKey(callee.Declaration)) invocations[callee.Declaration].Add(invocation);
        //                else invocations[callee.Declaration] = new List<InvocationExpressionSyntax> { invocation };
        //            }
        //            else
        //            {
        //                var declaringTypeSymbol = callSymbol.ContainingSymbol;
        //                if (interfaces.ContainsKey(declaringTypeSymbol.ToString()))
        //                {
        //                    var methodInfo = callSymbol.ToString().Replace(declaringTypeSymbol.ToString(), string.Empty);
        //                    foreach (var implementation in interfaces[declaringTypeSymbol.ToString()])
        //                    {
        //                        var target = $"{implementation.FullName}{methodInfo}";
        //                        var implementedMethod = implementation.Methods.SelectMany(m => m.Value).SingleOrDefault(m => m.FullName == target);
        //                        if (implementedMethod == null)
        //                        {
        //                            //TODO: log
        //                            Console.WriteLine($"[Warning] Can't find {target}. Call ignored.");
        //                            continue;
        //                        }
        //                        implementedMethod.Callers.Add(method);
        //                        method.InternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition, implementedMethod));
        //                        if (invocations.ContainsKey(implementedMethod.Declaration)) invocations[implementedMethod.Declaration].Add(invocation);
        //                        else invocations[implementedMethod.Declaration] = new List<InvocationExpressionSyntax> { invocation };
        //                    }
        //                }
        //                else
        //                {
        //                    method.ExternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition));
        //                }
        //            }
        //        }
        //    }

        //    return new AnalysisResult
        //    {
        //        //Solution = _solution,
        //        Interfaces = interfaces,
        //        Methods = methods,
        //        Invocations = invocations,
        //    };
        //}
    }
}