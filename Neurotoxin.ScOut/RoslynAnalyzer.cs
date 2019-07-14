using System;
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
    public class RoslynAnalyzer
    {
        private readonly IMapper<Microsoft.CodeAnalysis.Solution, Solution> _solutionMapper;
        private readonly IDependencyFiltering _dependencyFiltering;

        private Solution _solution;

        public RoslynAnalyzer(IMapper<Microsoft.CodeAnalysis.Solution, Solution> solutionMapper, IDependencyFiltering dependencyFiltering)
        {
            _solutionMapper = solutionMapper;
            _dependencyFiltering = dependencyFiltering;
        }

        public RoslynAnalyzer LoadSolution(string path)
        {
            MSBuildLocator.RegisterDefaults();
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

        public AnalysisResult Analyze()
        {
            var interfaces = _solution.Classes.Values.SelectMany(c => c.Implements.Select(i => new {i, c})).GroupBy(p => p.i).ToDictionary(g => g.Key, g => g.Select(p => p.c).ToArray());
            var methods = _solution.Classes.Values.SelectMany(c => c.Methods.SelectMany(m => m.Value)).ToDictionary(m => m.FullName, m => m);
            foreach (var method in methods.Values)
            {
                foreach (var invocation in method.Declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    var info = method.ParentClass.Model.GetSymbolInfo(invocation);
                    if (info.Symbol == null)
                    {
                        //TODO: log/handle anomalies
                        method.ExternalCalls.Add(new MethodCall(method, invocation));
                        continue;
                    }

                    var callSymbol = info.Symbol as IMethodSymbol;
                    if (callSymbol.IsGenericMethod) callSymbol = callSymbol.OriginalDefinition;

                    if (_dependencyFiltering.ExcludeAssemblies.Contains(callSymbol.ContainingType.ContainingAssembly.Name)) continue;

                    if (!Equals(callSymbol.ContainingType, callSymbol.ContainingSymbol)) Debugger.Break();

                    if (methods.ContainsKey(callSymbol.ToString()))
                    {
                        var callee = methods[callSymbol.ToString()];
                        callee.Callers.Add(method);
                        method.InternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition, callee));
                    }
                    else
                    {
                        var declaringTypeSymbol = callSymbol.ContainingSymbol;
                        if (interfaces.ContainsKey(declaringTypeSymbol.ToString()))
                        {
                            var methodInfo = callSymbol.ToString().Replace(declaringTypeSymbol.ToString(), string.Empty);
                            foreach (var implementation in interfaces[declaringTypeSymbol.ToString()])
                            {
                                var implementedMethod = implementation.Methods.SelectMany(m => m.Value).SingleOrDefault(m => m.FullName == $"{implementation.FullName}{methodInfo}");
                                if (implementedMethod == null)
                                {
                                    Debugger.Break();
                                }
                                implementedMethod.Callers.Add(method);
                                method.InternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition, implementedMethod));
                            }
                        }
                        else
                        {
                            method.ExternalCalls.Add(new MethodCall(method, invocation, callSymbol.OriginalDefinition));
                        }
                    }
                }
            }

            return new AnalysisResult
            {
                Solution = _solution,
                Interfaces = interfaces,
                Methods = methods,
                ProjectCount = _solution.Projects.Length,
                ClassCount = _solution.Classes.Count,
                MethodCount = methods.Count,
                DeadMethodCount = methods.Values.Count(m => !m.Callers.Any()),
                MaxInternalCalls = methods.Values.Max(m => m.InternalCalls.Count),
                MaxExternalCalls = methods.Values.Max(m => m.ExternalCalls.Count),
                AvgInternalCalls = methods.Values.Average(m => m.InternalCalls.Count),
                AvgExternalCalls = methods.Values.Average(m => m.ExternalCalls.Count),
                MaxInternallyCalled = methods.Values.Select(m => new { Name = m.FullName, CallCount = m.Callers.Count }).OrderByDescending(p => p.CallCount).First(),
                MaxExternallyCalled = methods.Values.SelectMany(m => m.ExternalCalls).GroupBy(m => m).Select(g => new { Method = g.Key, Count = g.Count() }).OrderByDescending(p => p.Count).First()
            };
        }
    }
}