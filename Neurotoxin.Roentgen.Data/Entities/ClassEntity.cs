using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Constants;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("component.png")]
    [DisplayName("Class")]
    public class ClassEntity: CodeEntityBase
    {
        public ClassType ClassType { get; set; }
    }
}