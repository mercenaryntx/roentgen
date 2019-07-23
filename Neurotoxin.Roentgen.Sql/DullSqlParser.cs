using System;
using System.Collections.Generic;
using System.Linq;
using TSQL;
using TSQL.Tokens;

namespace Neurotoxin.Roentgen.Sql
{
    public class DullSqlParser
    {
        private readonly string[] _keywords =
        {
            "UPDATE",
            "FROM",
            "JOIN",
            "INTO"
        };

        private readonly string[] _ignore =
        {
            "NOLOCK"
        };

        public IEnumerable<SqlMatch> Parse(string sqlText)
        {
            foreach (var tokens in TSQLStatementReader.ParseStatements(sqlText).Select(s => s.Tokens))
            {
                var targets = new HashSet<string>();
                var collect = false;
                string tmp = null;

                var previous = tokens.First();
                var queryType = GetQueryType(previous);
                foreach (var token in tokens.Skip(1))
                {
                    switch (token)
                    {
                        case TSQLKeyword keyword:
                            collect = _keywords.Contains(keyword.Text, StringComparer.InvariantCultureIgnoreCase);
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                targets.Add(tmp);
                                tmp = null;
                            }
                            break;
                        case TSQLIdentifier identifier:
                            if (!collect || _ignore.Contains(identifier.Text, StringComparer.InvariantCultureIgnoreCase)) continue;
                            if (previous is TSQLIdentifier)
                            {
                                targets.Add(tmp);
                                tmp = null;
                                continue;
                            }
                            tmp = previous.Text == "." ? $"{tmp}.{identifier.Name}" : identifier.Name;
                            break;
                    }
                    previous = token;
                }
                if (!string.IsNullOrEmpty(tmp)) targets.Add(tmp);
                yield return new SqlMatch
                {
                    Targets = targets.ToArray(),
                    Type = queryType
                };
            }
        }

        private static QueryType GetQueryType(TSQLToken previous)
        {
            QueryType queryType;
            switch (previous.Text.ToUpper())
            {
                case "SELECT":
                    queryType = QueryType.Select;
                    break;
                case "INSERT":
                    queryType = QueryType.Insert;
                    break;
                case "UPDATE":
                    queryType = QueryType.Update;
                    break;
                case "DELETE":
                    queryType = QueryType.Delete;
                    break;
                default:
                    throw new NotSupportedException("Invalid SQL statement: " + previous.Text);
            }

            return queryType;
        }
    }
}