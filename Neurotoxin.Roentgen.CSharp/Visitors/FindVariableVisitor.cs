using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen.CSharp.Extensions;

namespace Neurotoxin.Roentgen.CSharp.Visitors
{
    public class FindVariableVisitor : VisitorBase<SyntaxNode>
    {
        private SyntaxToken _identifier;

        public SyntaxNode FindVariable(SyntaxNode node, SyntaxToken identifier)
        {
            _identifier = identifier;
            return Visit(node);
        }

        protected override SyntaxNode ContinueWith(SyntaxNode node)
        {
            return Visit(node.Parent);
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node == null) return null;
            //TODO: First isn't good enough - we have to know where we came from
            return VisitTyped(node)
                ?? node.FindNodes<VariableDeclaratorSyntax>().Select(Visit).SingleOrDefault(v => v != null)
                ?? node.FindNodes<AssignmentExpressionSyntax>().Select(Visit).FirstOrDefault(v => v != null)
                ?? ContinueWith(node);
        }

        private SyntaxNode Visit(VariableDeclaratorSyntax declarator)
        {
            return declarator.Identifier.ValueText == _identifier.ValueText ? declarator.Initializer?.Value : null;
        }

        private SyntaxNode Visit(AssignmentExpressionSyntax assignment)
        {
            var left = assignment.Left as IdentifierNameSyntax;
            return left?.Identifier.ValueText == _identifier.ValueText ? assignment.Right : null;
        }

        private SyntaxNode Visit(MemberAccessExpressionSyntax node)
        {
            return node.Name.Identifier.ValueText == _identifier.ValueText
                ? node
                : null;
        }

        private SyntaxNode Visit(MethodDeclarationSyntax methodDeclaration)
        {
            return methodDeclaration.ParameterList.Parameters.SingleOrDefault(p => p.Identifier.ValueText == _identifier.ValueText);
        }

        private SyntaxNode Visit(ForEachStatementSyntax forEachStatement)
        {
            return forEachStatement.Expression;
        }

        private SyntaxNode Visit(QueryExpressionSyntax queryExpression)
        {
            return queryExpression.FromClause.Identifier.ValueText == _identifier.ValueText ? queryExpression.FromClause.Expression : null;
        }
    }
}