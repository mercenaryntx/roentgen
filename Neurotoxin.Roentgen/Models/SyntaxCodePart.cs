using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.Roentgen.Models
{
    public abstract class SyntaxCodePart : ICodePart
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public int Length { get; protected set; }
        public int Loc { get; protected set; }

        public IList<ICodePart> Children { get; set; }

        public void Parse(SyntaxNode node, SemanticModel model)
        {
            var code = node.ToString();
            var r = new Regex(Environment.NewLine);
            Length = code.Length;
            Loc = r.Split(code.Trim()).Length;
            ParseFromSymbol(model.GetDeclaredSymbol(node));
        }

        protected virtual void ParseFromSymbol(ISymbol symbol)
        {
            Name = symbol.Name;
            FullName = symbol.ToString();
        }

        public static T Create<T>(SyntaxNode node, SemanticModel model) where T : SyntaxCodePart, new()
        {
            var instance = new T();
            instance.Parse(node, model);
            return instance;
        }
    }
}