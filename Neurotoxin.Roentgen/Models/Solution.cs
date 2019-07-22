using System.Linq;

namespace Neurotoxin.Roentgen.Models
{
    public class Solution : FileCodePart
    {
        public Project[] Projects => Children.Cast<Project>().ToArray();
        public override string ToString() => FullName;
    }
}