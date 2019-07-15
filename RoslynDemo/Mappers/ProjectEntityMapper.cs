using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo.Mappers
{
    public class ProjectEntityMapper : IMapper<Project, ProjectEntity>
    {
        public ProjectEntity Map(Project input) => new ProjectEntity
        {
            Name = Path.GetFileName(input.Path),
            ProjectPath = input.Path,
            Language = input.LanguageVersion,
            TargetFramework = input.TargetFramework
        };
    }
}