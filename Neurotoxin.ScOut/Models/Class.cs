using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Class : SyntaxCodePart
    {
        public Dictionary<string, Property> Properties => Children.OfType<Property>().ToDictionary(p => p.Name, p => p);
        public Dictionary<string, Method[]> Methods => Children.OfType<Method>().GroupBy(m => m.Name).ToDictionary(g => g.Key, g => g.ToArray());
        public Class[] Subclasses => Children.OfType<Class>().ToArray();
        public string[] Implements { get; private set; }

        public List<string> SourceFiles { get; } = new List<string>();
        public ClassType ClassType { get; set; }

        public override string ToString() => FullName;

        protected override void ParseFromSymbol(ISymbol symbol)
        {
            base.ParseFromSymbol(symbol);
            Implements = ((INamedTypeSymbol)symbol).AllInterfaces.Select(i => i.ToString()).ToArray();
        }

        public void Merge(Class otherClass)
        {
            Length += otherClass.Length;
            Loc += otherClass.Loc;
            Children = Children.Concat(otherClass.Children).ToArray();
            ClassType |= otherClass.ClassType;
        }
    }
}