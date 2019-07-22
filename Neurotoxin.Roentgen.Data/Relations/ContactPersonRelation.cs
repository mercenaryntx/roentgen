using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Contact Person")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(PersonEntity))]
    public class ContactPersonRelation : RelationBase
    {
    }
}