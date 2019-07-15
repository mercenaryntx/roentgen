using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Call")]
    [Icon("link.png")]
    //[AllowedTargetEntityType(typeof(StoredProcedureEntity), typeof(UserDefinedFunctionEntity), typeof(DbXsltRequestEntity))]
    public class CallRelation : RelationBase
    {
        public string ParameterTypes { get; set; }
    }
}