using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Visitors
{
    public class SourceFileVisitor : VisitorBase<IEnumerable<CodePart>>
    {
        private SemanticModel _model;

        public IEnumerable<Class> Discover(SyntaxTree tree, Compilation compilation)
        {
            Reset();
            _model = compilation.GetSemanticModel(tree);
            return Visit(tree.GetRoot()).Cast<Class>();
        }

        private IEnumerable<CodePart> Visit(ClassDeclarationSyntax node)
        {
            var cls = CodePart.Create<Class>(node, _model);
            cls.Children = node.ChildNodes().SelectMany(Visit).ToArray();
            yield return cls;
        }

        private IEnumerable<CodePart> Visit(MethodDeclarationSyntax node)
        {
            var method = CodePart.Create<Method>(node, _model);
            method.Declaration = node;
            yield return method;
        }

        private IEnumerable<CodePart> Visit(PropertyDeclarationSyntax node)
        {
            yield return CodePart.Create<Property>(node, _model);
        }

        private IEnumerable<CodePart> Visit(InterfaceDeclarationSyntax node)
        {
            yield break;
        }

        private IEnumerable<CodePart> Visit(StructDeclarationSyntax node)
        {
            yield break;
        }

        protected override IEnumerable<CodePart> ContinueWith(SyntaxNode node)
        {
            return node.ChildNodes().SelectMany(Visit);
        }

    }
}