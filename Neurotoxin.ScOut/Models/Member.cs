using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Models
{
    public abstract class Member
    {
        public Class ParentClass { get; set; }

        public abstract string Name { get; }
        public abstract string FullName { get; }
        public abstract string Type { get; }

        public override string ToString() => Name;
    }
}