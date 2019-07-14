namespace Neurotoxin.ScOut.Models
{
    public class SourceFile
    {
        public string Path { get; set; }
        public Class[] Classes { get; set; }

        public override string ToString() => Path;
    }
}