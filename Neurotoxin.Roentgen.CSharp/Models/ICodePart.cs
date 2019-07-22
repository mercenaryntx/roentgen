using System.Collections.Generic;

namespace Neurotoxin.Roentgen.CSharp.Models
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