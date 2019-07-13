using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Mappers
{
    public class SourceFileMapper : ISourceFileMapper // IMapper<SyntaxTree, SourceFile>
    {
        private readonly IClassMapper _classMapper;
        private readonly IMapper<UsingDirectiveSyntax, Using> _usingMapper;

        public SourceFileMapper(IClassMapper classMapper, IMapper<UsingDirectiveSyntax, Using> usingMapper)
        {
            _classMapper = classMapper;
            _usingMapper = usingMapper;
        }

        public SourceFile Map(SyntaxTree tree, Compilation compilation)
        {
            var root = tree.GetRootAsync().GetAwaiter().GetResult();
            var model = compilation.GetSemanticModel(tree);
            var namespaceDeclarationSyntaxes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().ToList();
            var namespaces = namespaceDeclarationSyntaxes.Any()
                ? namespaceDeclarationSyntaxes.ToDictionary(s => s.Name.ToString(), s => (CSharpSyntaxNode)s)
                : new Dictionary<string, CSharpSyntaxNode> { { string.Empty, (CSharpSyntaxNode)root } };
            return new SourceFile
            {
                Path = tree.FilePath,
                Usings = root.DescendantNodes().OfType<UsingDirectiveSyntax>().Select(_usingMapper.Map).ToArray(),
                Classes = namespaces.SelectMany(ns => ns.Value.DescendantNodes().OfType<ClassDeclarationSyntax>().Select(s => _classMapper.Map(s, ns.Key, model))).ToArray(),
            };
        }
    }

    public interface ISourceFileMapper
    {
        SourceFile Map(SyntaxTree tree, Compilation compilation);
    }
}