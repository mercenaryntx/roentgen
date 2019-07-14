using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Analysis
{
    public class AnalysisResult
    {
        public Solution Solution { get; set; }
        public Dictionary<string, Class[]> Interfaces { get; set; }
        public Dictionary<string, Method> Methods { get; set; }

        public int ProjectCount { get; set; }
        public int ClassCount { get; set; }
        public int MethodCount { get; set; }
        public int DeadMethodCount { get; set; }
        public int MaxInternalCalls { get; set; }
        public int MaxExternalCalls { get; set; }
        public double AvgInternalCalls { get; set; }
        public double AvgExternalCalls { get; set; }
        public object MaxInternallyCalled { get; set; }
        public object MaxExternallyCalled { get; set; }
    }
}