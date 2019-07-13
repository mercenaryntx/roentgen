namespace Neurotoxin.ScOut.Models
{
    public abstract class Member
    {
        public Class ParentClass { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public virtual string FullName => $"{ParentClass.FullName}.{Name}";

        public override string ToString() => Name;
    }
}