using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Mappers;
using Neurotoxin.Roentgen.Models;

namespace EfTestApp.Mappers
{
    public class ProjectEntityMapper : IMapper<Project, ProjectEntity>
    {
        public ProjectEntity Map(Project input) => new ProjectEntity
        {
            Path = input.Path,
            Language = input.LanguageVersion,
            TargetFramework = input.TargetFramework
        };
    }
}