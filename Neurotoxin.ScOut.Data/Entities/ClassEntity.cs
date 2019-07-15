using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("component.png")]
    [DisplayName("Class")]
    public class ClassEntity: CodeEntityBase
    {
        public bool IsGenerated { get; set; }
    }
}