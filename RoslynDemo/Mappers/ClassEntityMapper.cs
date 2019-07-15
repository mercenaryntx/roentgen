using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo.Mappers
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