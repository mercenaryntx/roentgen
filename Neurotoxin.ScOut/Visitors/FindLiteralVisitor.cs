using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.ScOut.Visitors
{
    public class FindLiteralVisitor : VisitorBase<IEnumerable<string>>
    {
        private readonly FindVariableVisitor _findVariableVisitor = new FindVariableVisitor();
        private Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> _invocations;

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
                    .Select(call =>
                {
                    //TODO: Remove
                    var m = methodDeclaration;
                    var n = node;
                    if (call.ArgumentList.Arguments.Count <= parameterIndex) Debugger.Break();
                    return call.ArgumentList.Arguments[parameterIndex].Expression;
                }).Where(p => p != null)
                    .SelectMany(Visit);
        }

        private IEnumerable<string> Visit(InvocationExpressionSyntax node)
        {
            var memberAccess = node.Expression as MemberAccessExpressionSyntax;
            if (memberAccess?.ToString() == "string.Join<string>")
            {
                return Visit(node.ArgumentList.Arguments[1]);
            }
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