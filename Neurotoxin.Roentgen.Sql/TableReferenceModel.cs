using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Neurotoxin.Roentgen.Sql
{
    public class TableReferenceModel
    {
        public SchemaObjectName SchemaObject { get; }
        public Identifier Alias { get; }

        public TableReferenceModel(SchemaObjectName schemaObject, Identifier @alias)
        {
            SchemaObject = schemaObject;
            Alias = alias;
        }
    }
}