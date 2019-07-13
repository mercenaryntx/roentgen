using System.Collections.Generic;

namespace Neurotoxin.ScOut.Models
{
    public class Property : Member
    {
        public Method Getter { get; set; }
        public Method Setter { get; set; }

        public IEnumerable<Method> Accessors
        {
            get
            {
                if (Getter != null) yield return Getter;
                if (Setter != null) yield return Setter;
            }
        }

        public override string ToString() => $"{ParentClass.FullName}.{Name}"; // {{ {(Getter != null ? "get; " : "")}{(Setter != null ? "set; " : "")}}}
    }
}