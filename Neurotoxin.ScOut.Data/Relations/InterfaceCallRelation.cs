using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Call")]
    [Icon("link.png")]
    public class InterfaceCallRelation : RelationBase
    {
        public string ParameterTypes { get; set; }
    }
}