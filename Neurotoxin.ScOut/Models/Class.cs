using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Class : CodePart
    {
        public Dictionary<string, Property> Properties => Children.OfType<Property>().ToDictionary(p => p.Name, p => p);
        public Dictionary<string, Method[]> Methods => Children.OfType<Method>().GroupBy(m => m.Name).ToDictionary(g => g.Key, g => g.ToArray());
        public Class[] Subclasses => Children.OfType<Class>().ToArray();
        public string[] Implements { get; private set; }

        public string[] SourceFiles { get; set; }
        public bool IsGenerated { get; set; }

        public override string ToString() => FullName;

        protected override void ParseFromSymbol(ISymbol symbol)
        {
            base.ParseFromSymbol(symbol);
            Implements = ((INamedTypeSymbol)symbol).AllInterfaces.Select(i => i.ToString()).ToArray();
        }
    }
}