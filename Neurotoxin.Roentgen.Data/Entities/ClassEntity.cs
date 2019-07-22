using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("component.png")]
    [DisplayName("Class")]
    public class ClassEntity: CodeEntityBase
    {
        public bool IsGenerated { get; set; }
    }
}