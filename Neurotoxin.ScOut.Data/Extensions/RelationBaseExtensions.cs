using System;
using System.Linq;
using Neurotoxin.ScOut.Data.Relations;

namespace Neurotoxin.ScOut.Data.Extensions
{
    public static class RelationBaseExtensions
    {
        public static T CloneRelation<T>(this T relation) where T : RelationBase
        {
            return (T)CloneRelation(relation, typeof(T));
        }

        public static RelationBase CloneRelation(this RelationBase relation, Type type)
        {
            var clone = Activator.CreateInstance(type);

            foreach (var pi in type.GetProperties().Where(pi => pi.Name != "Id"))
            {
                pi.SetValue(clone, pi.GetValue(relation));
            }

            return (RelationBase)clone;
        }
    }
}