using System;
using Neurotoxin.Roentgen.CSharp.Models;
using Neurotoxin.Roentgen.Data.Entities;

namespace EfTestApp
{
    public class EntityMapper
    {
        public SolutionEntity Map(Solution input) => MapDefault(new SolutionEntity
        {
            Path = input.FullName
        });

        public ProjectEntity Map(Project input) => MapDefault(new ProjectEntity
        {
            Path = input.FullName,
            Language = input.Language,
            TargetFramework = input.TargetFramework
        });

        public ClassEntity Map(Class input)
        {
            return MapDefault(new ClassEntity
            {
                Name = input.FullName,
                ClassType = (Neurotoxin.Roentgen.Data.Constants.ClassType)input.ClassType,
                Length = input.Length,
                Loc = input.Loc
            });
        }

        public MethodEntity Map(Method input) => MapDefault(new MethodEntity
        {
            Name = input.FullName,
            Length = input.Length,
            Loc = input.Loc
        });

        private T MapDefault<T>(T entity) where T : EntityBase
        {
            entity.EntityId = Guid.NewGuid();
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = @"BUDAPEST\Zsolt_Bangha";
            return entity;
        }
    }
}