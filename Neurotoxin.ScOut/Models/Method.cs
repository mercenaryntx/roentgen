using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Neurotoxin.ScOut.Models
{
    public class Method : Member
    {
        public CSharpSyntaxNode Declaration { get; set; }
        public IMethodSymbol Symbol { get; set; }
        public override string Name => Symbol.Name;
        public override string FullName => Symbol.ToString();
        public override string Type => Symbol.ReturnType.Name;


        //public override string FullName => $"{base.FullName}{T}({string.Join<string>(", ", Arguments.Select(a => a.Type))})";
        //private string T => TypeParameters != null ? $"<{string.Join<string>(",", TypeParameters)}>" : string.Empty;

        //public override string ToString() => $"{Type} {ParentClass.FullName}.{Name}({string.Join<Argument>(", ", Arguments)})";
    }
}