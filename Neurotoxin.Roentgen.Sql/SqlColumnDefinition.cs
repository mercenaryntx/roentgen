using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Neurotoxin.Roentgen.Sql
{
    public class SqlColumnDefinition
    {
        private readonly IList<Identifier> _identifiers;

        public string Column
        {
            get { return _identifiers.Last().Value; }
        }

        public string Alias
        {
            get { return _identifiers.Count > 1 ? _identifiers.First().Value : null; }
        }

        public string Table { get; set; }

        public string Prefix
        {
            get
            {
                return _identifiers.Count > 1
                    ? string.Join(".", _identifiers.Take(_identifiers.Count - 1).Select(i => i.Value))
                    : null;
            }
        }

        public SqlColumnDefinition(IList<Identifier> identifiers)
        {
            _identifiers = identifiers;
        }
    }
}