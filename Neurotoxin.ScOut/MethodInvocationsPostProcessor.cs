using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut
{
    public class MethodInvocationsPostProcessor : PostProcessor
    {
        public MethodInvocationsPostProcessor(AnalysisWorkspace workspace, ExcludingRules excludingRules) : base(workspace, excludingRules)
        {
        }

        public override void Process()
        {
            foreach (var source in Workspace.SourceFiles.Values)
            {
                //var models = @class.SourceFiles.Select(file => Workspace.Models[file]).ToArray();
                foreach (var method in source.Children.SelectMany(c => c.Children).OfType<Method>())
                {
                    foreach (var invocation in method.Declaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
                    {
                        var symbol = source.Model.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
                        if (symbol == null)
                        {
                            //TODO: log/handle anomalies
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
                            //method.InternalCalls.Add(new MethodCall(method, invocation, symbol.OriginalDefinition, callee));
                            //if (invocations.ContainsKey(callee.Declaration)) invocations[callee.Declaration].Add(invocation);
                            //else invocations[callee.Declaration] = new List<InvocationExpressionSyntax> { invocation };
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
                                        //TODO: log
                                        //Console.WriteLine($"[Warning] Can't find {target}. Call ignored.");
                                        continue;
                                    }
                                    Workspace.Register(new InternalCall(method, implementedMethod, invocation));
                                    //implementedMethod.Callers.Add(method);
                                    //method.InternalCalls.Add(new MethodCall(method, invocation, symbol.OriginalDefinition, implementedMethod));
                                    //if (invocations.ContainsKey(implementedMethod.Declaration)) invocations[implementedMethod.Declaration].Add(invocation);
                                    //else invocations[implementedMethod.Declaration] = new List<InvocationExpressionSyntax> { invocation };
                                }
                            }
                            else
                            {
                                Workspace.Register(new ExternalCall(method, symbol.OriginalDefinition, invocation));
                                //method.ExternalCalls.Add(new MethodCall(method, invocation, symbol.OriginalDefinition));
                            }
                        }
                    }
                }
            }
        }
    }

    public class InternalCall : UnknownCall
    {
        public Method Callee { get; }

        public InternalCall(Method caller, Method callee, InvocationExpressionSyntax invocation) : base(caller, invocation)
        {
            Callee = callee;
        }
    }

    public class ExternalCall : UnknownCall
    {
        public IMethodSymbol CalleeSymbol { get; }

        public ExternalCall(Method caller, IMethodSymbol symbol, InvocationExpressionSyntax invocation) : base(caller, invocation)
        {
            CalleeSymbol = symbol;
        }
    }

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