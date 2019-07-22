using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Neurotoxin.Roentgen.Sql
{
    public class SqlParserResult
    {
        public TSqlTokenType Type { get; set; }
        public HashSet<string> Tables { get; }
        public Dictionary<string, string> Aliases { get; }
        public List<SqlColumnDefinition> Fields { get; }
        public string Target { get; set; }

        public SqlParserResult()
        {
            Tables = new HashSet<string>();
            Aliases = new Dictionary<string, string>();
            Fields = new List<SqlColumnDefinition>();
        }
    }
}