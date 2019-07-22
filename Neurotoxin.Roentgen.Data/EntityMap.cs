using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Neurotoxin.Roentgen.Data
{
    public static class EntityMap
    {
        private static readonly Dictionary<Type, string> InsertCache = new Dictionary<Type, string>();

        public static string GetInsertQuery(string tableName, Type type)
        {
            if (!InsertCache.ContainsKey(type))
            {
                var map = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                              .Where(p => p.GetCustomAttribute<KeyAttribute>() == null)
                              .Select(p => p.Name)
                              .ToArray();
                var query = $"INSERT INTO {tableName} (Discriminator, {string.Join(", ", map)}) VALUES ('{type.Name}', {string.Join(", ", map.Select(m => "@" + m))})";
                InsertCache.Add(type, query);
            }

            return InsertCache[type];
        }

        public static string GetInsertQuery<T>(string tableName)
        {
            return GetInsertQuery(tableName, typeof(T));
        }
    }
}