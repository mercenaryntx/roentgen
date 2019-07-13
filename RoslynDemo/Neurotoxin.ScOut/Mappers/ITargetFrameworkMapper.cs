using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.ScOut.Mappers
{
    public interface ITargetFrameworkMapper : IMapper<IEnumerable<SyntaxTree>, string>
    {
    }
}