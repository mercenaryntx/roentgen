using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo.Mappers
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