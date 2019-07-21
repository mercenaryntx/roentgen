using System.Collections.Generic;

namespace Neurotoxin.ScOut.Models
{
    public interface ICodePart
    {
        string Name { get; }
        string FullName { get; }
        int Length { get; }
        int Loc { get; }

        IList<ICodePart> Children { get; set; }

    }
}