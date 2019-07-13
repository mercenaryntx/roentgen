namespace Neurotoxin.ScOut.Models
{
    public class Using
    {
        public string Alias { get; set; }
        public string Namespace { get; set; }

        public override string ToString() => $"using {(Alias != null ? Alias + " = " : "")}{Namespace}";
    }
}