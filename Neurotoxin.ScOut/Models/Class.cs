using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Class
    {
        public SemanticModel Model { get; set; }

        public string[] SourceFiles { get; set; }
        public INamedTypeSymbol[] Symbols { get; set; }
        public INamedTypeSymbol Symbol
        {
            get => Symbols[0];
            set => Symbols = new[] {value};
        }

        public string Name => Symbol.Name;
        public string FullName => Symbol.ToString();

        public string[] Implements => Symbols.SelectMany(s => s.AllInterfaces).Select(i => i.ToString()).Distinct().ToArray();

        public Dictionary<string, Property> Properties { get; set; }
        public Dictionary<string, Method[]> Methods { get; set; }

        public override string ToString() => FullName;
    }
}