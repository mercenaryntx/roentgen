using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.Extensions;

namespace Neurotoxin.Roentgen.Visitors
{
    public class FindLiteralVisitor : VisitorBase<IEnumerable<string>>
    {
        private readonly ILogger<FindLiteralVisitor> _logger;
        private readonly FindVariableVisitor _findVariableVisitor;
        private Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> _invocations;

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

            if (!(parameterList?.Parent is MethodDeclarationSyntax methodDeclaration) || !_invocations.ContainsKey(methodDeclaration)) return null;
            return _invocations[methodDeclaration]
                    .Select(call => call.ArgumentList.Arguments[parameterIndex].Expression).Where(p => p != null)
                    .SelectMany(Visit);
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
            return node.Initializer.Expressions.SelectMany(Visit);
        }

        protected override IEnumerable<string> ContinueWith(SyntaxNode node)
        {
            return node.ChildNodes().SelectMany(Visit);
        }
    }
}