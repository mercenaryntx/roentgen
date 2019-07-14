using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Property
    {
        public Class ParentClass { get; set; }
        public IPropertySymbol Symbol { get; set; }
        public string Name => Symbol.Name;
        public string Type => Symbol.Type.Name;
        public string FullName => Symbol.ToString();

        public override string ToString() => Symbol.ToString();

        //public Method Getter { get; set; }
        //public Method Setter { get; set; }

    }
}