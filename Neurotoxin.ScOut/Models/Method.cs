using System.Linq;

namespace Neurotoxin.ScOut.Models
{
    public class Method : Member
    {
        public string[] TypeParameters { get; set; }
        public Argument[] Arguments { get; set; }
        public Call[] Calls { get; set; }
        public override string FullName => $"{base.FullName}{T}({string.Join<string>(", ", Arguments.Select(a => a.Type))})";
        private string T => TypeParameters != null ? $"<{string.Join<string>(",", TypeParameters)}>" : string.Empty;

        public override string ToString() => $"{Type} {ParentClass.FullName}.{Name}({string.Join<Argument>(", ", Arguments)})";
    }
}