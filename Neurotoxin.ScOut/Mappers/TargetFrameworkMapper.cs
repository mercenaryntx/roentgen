using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Filtering;

namespace Neurotoxin.ScOut.Mappers
{
    public class TargetFrameworkMapper : ITargetFrameworkMapper
    {
        private readonly Dictionary<string, string> _excludeRules;

        public TargetFrameworkMapper(ISourceFileFiltering rules)
        {
            _excludeRules = rules.ExcludeRules;
        }

        public string Map(IEnumerable<SyntaxTree> trees)
        {
            var assemblyAttributesRule = new Regex(_excludeRules["AssemblyAttributes"]);
            var tree = trees.SingleOrDefault(t => assemblyAttributesRule.IsMatch(t.FilePath));
            if (tree == null) return null;
            var root = tree.GetRootAsync().GetAwaiter().GetResult();
            var expression = root.DescendantNodes()
                .OfType<AttributeSyntax>()
                .Single()
                .ArgumentList
                .Arguments
                .Single(arg => arg.NameEquals != null && arg.NameEquals.Name.Identifier.ValueText == "FrameworkDisplayName")
                .Expression as LiteralExpressionSyntax;
            return expression?.Token.ValueText;
        }
    }
}
