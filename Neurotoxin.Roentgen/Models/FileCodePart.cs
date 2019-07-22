using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Neurotoxin.Roentgen.Models
{
    public abstract class FileCodePart : ICodePart
    {
        public string Name => Path.GetFileName(FullName);
        public string FullName { get; set; }
        public int Length => Children.Sum(c => c.Length);
        public int Loc => Children.Sum(c => c.Loc);
        public IList<ICodePart> Children { get; set; }
    }
}