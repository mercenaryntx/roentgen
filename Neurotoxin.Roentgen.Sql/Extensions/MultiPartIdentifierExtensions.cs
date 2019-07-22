using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Neurotoxin.Roentgen.Sql.Extensions
{
    public static class MultiPartIdentifierExtensions
    {
        public static string AsObjectName(this MultiPartIdentifier o)
        {
            return string.Join(".", o.Identifiers.Select(i => i.Value));
        }
    }
}