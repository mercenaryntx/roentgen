using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Call")]
    [Icon("link.png")]
    //[AllowedTargetEntityType(typeof(StoredProcedureEntity), typeof(UserDefinedFunctionEntity), typeof(DbXsltRequestEntity))]
    public class CallRelation : RelationBase
    {
        public string ParameterTypes { get; set; }
    }
}