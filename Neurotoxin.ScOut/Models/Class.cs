using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Class
    {
        public SemanticModel Model { get; set; }
        public string[] SourceFiles { get; set; }
        public Using[] Usings { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string FullName => $"{Namespace}.{Name}";
        public Dictionary<string, Property> Properties { get; set; }
        public Dictionary<string, Method[]> Methods { get; set; }
        //public Dictionary<string, Member> Members => Properties.Cast<Member>().Concat(Methods).ToDictionary(m => m.FullName, m => m);

        public override string ToString() => FullName;
    }
}