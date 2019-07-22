using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using EfTestApp.Analysis;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Models;
using Neurotoxin.Roentgen.Data.Constants;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Data.Relations;
using Neurotoxin.Roentgen.Sql;

namespace EfTestApp
{
    public class RelationMapper
    {
        private readonly Dictionary<ICodePart, Guid> _modelToEntityId = new Dictionary<ICodePart, Guid>();
        private readonly List<EntityBase> _entities = new List<EntityBase>();

        public EntityBase RegisterEntity(ICodePart model, EntityBase entity)
        {
            _modelToEntityId.Add(model, entity.EntityId);
            return entity;
        }

        public void RegisterEntity(EntityBase entity)
        {
            _entities.Add(entity);
        }

        public IEnumerable<RelationBase> Map(LinkBase link)
        {
            switch (link)
            {
                case ChildLink childLink:
                    yield return MapDefault<ParentChildRelation>(_modelToEntityId[childLink.Parent], _modelToEntityId[childLink.Child]);
                    break;
                case InternalCall internalCall:
                    yield return MapDefault<CallRelation>(_modelToEntityId[internalCall.Caller], _modelToEntityId[internalCall.Callee]);
                    break;
                case SqlCommandCall sqlCmmandCall:
                    foreach (var relation in MapToSqlEntities(sqlCmmandCall))
                    {
                        yield return relation;
                    }
                    break;
                case ExternalCall _:
                case UnknownCall _:
                    break;
            }
            throw new NotSupportedException("Unknown link: " + link.GetType());
        }

        private IEnumerable<RelationBase> MapToSqlEntities(SqlCommandCall call)
        {
            var matches = ParseSqlPart(call.Command);
            foreach (var match in matches)
            {
                foreach (var matchTarget in match.Targets)
                {
                    var entities = _entities.Where(e => e.Name.EndsWith(matchTarget));
                    var x = entities.ToArray();
                    if (x.Length > 1) Debugger.Break();
                    foreach (var entity in entities)
                    {
                        switch (match.Type)
                        {
                            case QueryType.Call:
                                yield return MapDefault<CallRelation>(_modelToEntityId[call.Caller], entity.EntityId);
                                break;
                            case QueryType.Select:
                                yield return MapDefault<SelectRelation>(_modelToEntityId[call.Caller], entity.EntityId);
                                break;
                            case QueryType.Insert:
                                yield return MapDefault<InsertRelation>(_modelToEntityId[call.Caller], entity.EntityId);
                                break;
                            case QueryType.Update:
                                yield return MapDefault<UpdateRelation>(_modelToEntityId[call.Caller], entity.EntityId);
                                break;
                            case QueryType.Delete:
                                yield return MapDefault<DeleteRelation>(_modelToEntityId[call.Caller], entity.EntityId);
                                break;
                            default:
                                throw new NotSupportedException("Unknown query type: " + match.Type);
                        }
                    }
                }
            }
        }

        private SqlMatch[] ParseSqlPart(string command)
        {
            var cmd = new Regex(@"[\[\]]").Replace(command, string.Empty);
            if (cmd.Contains(" "))
            {
                var parser = SqlParser.Parse(cmd);
                return parser.Result
                             .Select(r => new SqlMatch
                             {
                                 Type = MapSqlToken(r.Type),
                                 Targets = r.Tables.ToArray()
                             })
                             .ToArray();
            }
            return new[] {new SqlMatch {Type = QueryType.Call, Targets = new[] {cmd}}};
        }

        private QueryType MapSqlToken(TSqlTokenType tokenType)
        {
            switch (tokenType)
            {
                case TSqlTokenType.Select:
                    return QueryType.Select;
                case TSqlTokenType.Insert:
                    return QueryType.Insert;
                case TSqlTokenType.Update:
                    return QueryType.Update;
                case TSqlTokenType.Delete:
                    return QueryType.Delete;
                default:
                    throw new NotSupportedException("Not supported SQL token: " + tokenType);
            }
        }

        private TRelation MapDefault<TRelation>(Guid parentEntityId, Guid childEntityId)
            where TRelation : RelationBase, new()
        {
            return new TRelation
            {
                LeftEntityId = parentEntityId,
                RightEntityId = childEntityId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Zsolt_Bangha@epam.com"
            };
        }
    }
}