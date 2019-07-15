using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("assembly.png")]
    [DisplayName("Assembly")]
    public class AssemblyEntity : EntityBase
    {
        public string FullyQualifiedName { get; set; }
    }
}