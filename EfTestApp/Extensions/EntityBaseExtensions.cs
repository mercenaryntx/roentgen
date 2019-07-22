using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Data.Relations;

namespace EfTestApp.Extensions
{
    public static class EntityBaseExtensions
    {
        public static EntityBase Defaults(this EntityBase entity, IList<EntityBase> entities)
        {
            entity.EntityId = Guid.NewGuid();
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = "Zsolt_Bangha@epam.com";
            entities?.Add(entity);
            return entity;
        }

        public static EntityBase RelateTo<T>(this EntityBase entity, EntityBase parent, IList<RelationBase> relations) where T : RelationBase, new()
        {
            var relation = new T
            {
                LeftEntityId = parent.EntityId,
                RightEntityId = entity.EntityId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Zsolt_Bangha@epam.com"
            };
            relations?.Add(relation);
            return entity;
        }
    }
}