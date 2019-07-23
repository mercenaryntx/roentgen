using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            foreach (var tokens in TSQLStatementReader.ParseStatements(CleanUp(sqlText)).Select(s => s.Tokens))
            {
                var previous = tokens.First();
                var queryType = GetQueryType(previous);
                if (queryType == QueryType.Unknown) continue;

                var targets = new HashSet<string>();
                var collect = false;
                string tmp = null;

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
                            if (previous.Text == ".")
                            {
                                tmp += $".{identifier.Name}";
                            }
                            else
                            {
                                if (tmp != null) targets.Add(tmp);
                                tmp = previous is TSQLIdentifier ? null : identifier.Name;
                            }
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

        private static string CleanUp(string sql)
        {
            var cteHack = new Regex(@"^;?WITH .*? AS \(\s+(?=SELECT)", RegexOptions.IgnoreCase);
            var nolock = new Regex(@"(with )?\(nolock\)", RegexOptions.IgnoreCase);
            sql = cteHack.Replace(sql, string.Empty);
            sql = nolock.Replace(sql, string.Empty);
            return sql.Replace(" UNION ", string.Empty);
        }

        private static QueryType GetQueryType(TSQLToken token)
        {
            QueryType queryType;
            switch (token.Text.ToUpper())
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
                    return QueryType.Unknown;
            }

            return queryType;
        }
    }
}