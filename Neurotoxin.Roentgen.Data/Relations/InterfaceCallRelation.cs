using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Call")]
    [Icon("link.png")]
    public class InterfaceCallRelation : RelationBase
    {
        public string ParameterTypes { get; set; }
    }
}