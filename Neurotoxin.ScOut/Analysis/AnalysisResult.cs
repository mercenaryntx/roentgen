using System.Collections.Generic;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Analysis
{
    public class AnalysisResult
    {
        public Solution[] Solutions { get; set; }
        public LinkBase[] Links { get; set; }
    }
}