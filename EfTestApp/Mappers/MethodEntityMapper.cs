using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Mappers;
using Neurotoxin.Roentgen.Models;

namespace EfTestApp.Mappers
{
    public class MethodEntityMapper : IMapper<Method, MethodEntity>
    {
        public MethodEntity Map(Method input) => new MethodEntity
        {
            Name = input.FullName,
            Length = input.Length,
            Loc = input.Loc
        };
    }
}