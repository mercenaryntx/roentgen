using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class ChildLink : LinkBase
    {
        public ICodePart Parent { get; }
        public ICodePart Child { get; }

        public ChildLink(ICodePart parent, ICodePart child) : base(parent, child)
        {
            Parent = parent;
            Child = child;
        }
    }
}