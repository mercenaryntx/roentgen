using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public class Property : Member
    {
        public IPropertySymbol Symbol { get; set; }
        public override string Name => Symbol.Name;
        public override string Type => Symbol.Type.Name;
        public override string FullName => Symbol.ToString();


        //public Method Getter { get; set; }
        //public Method Setter { get; set; }

        //public IEnumerable<Method> Accessors
        //{
        //    get
        //    {
        //        if (Getter != null) yield return Getter;
        //        if (Setter != null) yield return Setter;
        //    }
        //}

        //public override string ToString() => $"{ParentClass.FullName}.{Name}"; // {{ {(Getter != null ? "get; " : "")}{(Setter != null ? "set; " : "")}}}
    }
}