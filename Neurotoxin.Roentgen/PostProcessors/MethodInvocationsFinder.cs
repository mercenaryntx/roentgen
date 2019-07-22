using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.Analysis;
using Neurotoxin.Roentgen.Models;

namespace Neurotoxin.Roentgen.PostProcessors
{
    public class MethodInvocationsFinder : PostProcessorBase
    {
        private readonly ILogger<MethodInvocationsFinder> _logger;

        public MethodInvocationsFinder(AnalysisWorkspace workspace, ExcludingRules excludingRules, ILogger<MethodInvocationsFinder> logger) : base(workspace, excludingRules)
        {
            _logger = logger;
        }

        public override void Process()
        {
            foreach (var source in Workspace.SourceFiles.Values)
            {
                foreach (var method in source.Children.SelectMany(c => c.Children).OfType<Method>())
                {
                    foreach (var invocation in method.Declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
                    {
                        var symbol = source.Model.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
                        if (symbol == null)
                        {
                            _logger.Warning($"Method symbol not found: {invocation}. Find the corresponding UnknownCall instance for more details.");
                            Workspace.Register(new UnknownCall(method, invocation));
                            continue;
                        }

                        if (symbol.IsGenericMethod) symbol = symbol.OriginalDefinition;
                        if (ExcludingRules.ExcludeLibraries.Contains(symbol.ContainingType.ContainingAssembly.Name)) continue;

                        if (!Equals(symbol.ContainingType, symbol.ContainingSymbol)) Debugger.Break();

                        if (Workspace.Methods.ContainsKey(symbol.ToString()))
                        {
                            var callee = Workspace.Methods[symbol.ToString()];
                            Workspace.Register(new InternalCall(method, callee, invocation));
                        }
                        else
                        {
                            var declaringTypeSymbol = symbol.ContainingSymbol;
                            if (Workspace.Interfaces.ContainsKey(declaringTypeSymbol.ToString()))
                            {
                                var methodInfo = symbol.ToString().Replace(declaringTypeSymbol.ToString(), string.Empty);
                                foreach (var implementation in Workspace.Interfaces[declaringTypeSymbol.ToString()])
                                {
                                    var target = $"{implementation.FullName}{methodInfo}";
                                    var implementedMethod = implementation.Methods.SelectMany(m => m.Value).SingleOrDefault(m => m.FullName == target);
                                    if (implementedMethod == null)
                                    {
                                        _logger.Info($"Class {implementation} doens't implement the {methodInfo} method declared in {declaringTypeSymbol}.");
                                        continue;
                                    }
                                    Workspace.Register(new InternalCall(method, implementedMethod, invocation));
                                }
                            }
                            else
                            {
                                Workspace.Register(new ExternalCall(method, symbol.OriginalDefinition, invocation));
                            }
                        }
                    }
                }
            }
        }
    }
}