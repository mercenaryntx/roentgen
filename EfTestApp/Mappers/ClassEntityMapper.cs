using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Mappers;
using Neurotoxin.Roentgen.Models;

namespace EfTestApp.Mappers
{
    public class ClassEntityMapper : IMapper<Class, ClassEntity>
    {
        public ClassEntity Map(Class input)
        {
            return new ClassEntity
            {
                Name = input.FullName,
                IsGenerated = input.IsGenerated,
                Length = input.Length,
                Loc = input.Loc
            };
        }
    }
}