using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Neurotoxin.Roentgen.Sql.Extensions
{

    public static class TableReferenceExtensions
    {
        public static TableReferenceModel ToTableReferenceModel(this TableReference reference)
        {
            switch (reference)
            {
                case SchemaObjectFunctionTableReference schemaObjectFunctionTableReference:
                    return new TableReferenceModel(schemaObjectFunctionTableReference.SchemaObject, schemaObjectFunctionTableReference.Alias);
                case NamedTableReference namedTableReference:
                    return new TableReferenceModel(namedTableReference.SchemaObject, namedTableReference.Alias);
            }

            return null;
        }
    }
        
}
