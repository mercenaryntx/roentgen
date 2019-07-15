using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Contact Person")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(PersonEntity))]
    public class ContactPersonRelation : RelationBase
    {
    }
}