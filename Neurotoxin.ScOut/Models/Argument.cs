namespace Neurotoxin.ScOut.Models
{
    public class Argument
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type} {Name}";
    }
}