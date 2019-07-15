using System;
using System.Linq;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Extensions
{
    public static class EntityBaseExtensions
    {
        public static T Clone<T>(this T entity) where T : EntityBase
        {
            var clone = Activator.CreateInstance<T>();

            foreach (var pi in typeof(T).GetProperties().Where(pi => pi.Name != "RowId"))
            {
                pi.SetValue(clone, pi.GetValue(entity));
            }

            return clone;
        }

        public static string GetPropertyValue<T>(this T entity, string propertyName) where T : EntityBase
        {
            if (entity == null)
                return String.Empty;

            var propertyValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);

            // Returns String.Empty if propertyValue is null.
            return Convert.ToString(propertyValue);
        }
    }
}