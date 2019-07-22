using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("assembly.png")]
    [DisplayName("Assembly")]
    public class AssemblyEntity : EntityBase
    {
        public string FullyQualifiedName { get; set; }
    }
}