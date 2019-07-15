using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("solution.png")]
    [DisplayName("Solution")]
    public class SolutionEntity : EntityBase
    {
        public string SolutionPath { get; set; }
    }
}