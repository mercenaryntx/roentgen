using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.CSharp.Extensions;

namespace Neurotoxin.Roentgen.CSharp.Visitors
{
    public class FindLiteralVisitor : VisitorBase<IEnumerable<string>>
    {
        private readonly ILogger<FindLiteralVisitor> _logger;
        private readonly FindVariableVisitor _findVariableVisitor;
        private Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> _invocations;
        private string _prefix = "";

        public FindLiteralVisitor(ILogger<FindLiteralVisitor> logger, FindVariableVisitor findVariableVisitor)
        {
            _logger = logger;
            _findVariableVisitor = findVariableVisitor;
        }

        public IEnumerable<string> FindLiteral(SyntaxNode node, Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> invocations)
        {
            _invocations = invocations;
            Reset();
            return Visit(node).Where(v => v != null);
        }

        private IEnumerable<string> Visit(IdentifierNameSyntax node)
        {
            var variable = _findVariableVisitor.FindVariable(node, node.Identifier);
            return variable == null ? null : Visit(variable);
        }

        private IEnumerable<string> Visit(LiteralExpressionSyntax node)
        {
            yield return node.Token.ValueText;
        }

        private IEnumerable<string> Visit(ParameterSyntax node)
        {
            var parameterList = node.Parent as ParameterListSyntax;
            var parameterIndex = parameterList?.Parameters.IndexOf(node) ?? -1;
            if (parameterIndex < 0) return null;

            if (!(parameterList?.Parent is MethodDeclarationSyntax methodDeclaration))
            {

                Debugger.Break();
                return null;
            }

            if (!_invocations.ContainsKey(methodDeclaration))
            {
                //TODO: log
                return null;
            }

            _prefix += "  ";

            return _invocations[methodDeclaration]
                .Select(call => Visit(call.ArgumentList.Arguments[parameterIndex].Expression))
                .Where(v => v != null)
                .SelectMany(v => v);
        }

        private IEnumerable<string> Visit(InvocationExpressionSyntax node)
        {
            var memberAccess = node.Expression as MemberAccessExpressionSyntax;
            if (memberAccess?.ToString() == "string.Join<string>")
            {
                return Visit(node.ArgumentList.Arguments[1]);
            }
            _logger.Info($"Invociation ignored: {node} ({node.GetPosition()})");
            return null;
        }

        private IEnumerable<string> Visit(ArrayCreationExpressionSyntax node)
        {
            return node.Initializer.Expressions.Select(Visit).Where(v => v != null).SelectMany(v => v);
        }

        protected override IEnumerable<string> ContinueWith(SyntaxNode node)
        {
            return node.ChildNodes().Select(Visit).Where(v => v != null).SelectMany(v => v);
        }
    }
}