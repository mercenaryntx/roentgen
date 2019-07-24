using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EfTestApp.Analysis;
using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Models;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Data.Relations;
using Neurotoxin.Roentgen.Sql;

namespace EfTestApp
{
    public class RelationMapper
    {
        private readonly DullSqlParser _sqlParser = new DullSqlParser();
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
                    if (_modelToEntityId.ContainsKey(childLink.Parent) && _modelToEntityId.ContainsKey(childLink.Child))
                    {
                        yield return MapDefault<ParentChildRelation>(_modelToEntityId[childLink.Parent], _modelToEntityId[childLink.Child]);
                    }
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
                default:
                    throw new NotSupportedException("Unknown link: " + link.GetType());
            }
        }

        private IEnumerable<RelationBase> MapToSqlEntities(SqlCommandCall call)
        {
            var matches = ParseSqlPart(call.Command);
            foreach (var match in matches)
            {
                foreach (var matchTarget in match.Targets)
                {
                    var identifier = matchTarget.Contains(".") ? matchTarget : ".dbo." + matchTarget;
                    var entities = _entities.Where(e => e.Name.EndsWith(identifier));
                    entities = match.Type == QueryType.Call
                        ? (IEnumerable<EntityBase>) entities.OfType<StoredProcedureEntity>()
                        : entities.OfType<TableEntity>();
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
                                throw new NotSupportedException("Invalid query type: " + match.Type);
                        }
                    }
                }
            }
        }

        private SqlMatch[] ParseSqlPart(string cmd)
        {
            cmd = new Regex(@"[\[\]]").Replace(cmd, string.Empty).Trim();
            return cmd.Contains(" ") 
                ? _sqlParser.Parse(cmd).ToArray() 
                : new[] {new SqlMatch {Type = QueryType.Call, Targets = new[] { cmd } }};
        }

        private TRelation MapDefault<TRelation>(Guid parentEntityId, Guid childEntityId)
            where TRelation : RelationBase, new()
        {
            return new TRelation
            {
                LeftEntityId = parentEntityId,
                RightEntityId = childEntityId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = @"BUDAPEST\Zsolt_Bangha"
            };
        }
    }
}