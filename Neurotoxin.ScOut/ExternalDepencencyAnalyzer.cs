using System;
using System.Collections.Generic;
using System.Linq;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Models;
using Neurotoxin.ScOut.Patterns;

namespace Neurotoxin.ScOut
{
    public class ExternalDepencencyAnalyzer
    {
        public Dictionary<Method, string> Analyze<T>(AnalysisResult analysisResult) where T : IDependencyScanner, new()
        {
            var rule = new T();
            var calls = analysisResult.Methods.Values.SelectMany(m => m.ExternalCalls.Where(c => rule.MethodCall.Any(p => c.TargetSymbol?.ToString().Contains(p) == true))).ToArray();
            return calls.Select(call => new { call.Caller, Result = rule.Scan(call) })
                        .Where(res => !string.IsNullOrEmpty(res.Result))
                        .GroupBy(c => c.Caller)
                        .ToDictionary(g => g.Key, g => string.Join(Environment.NewLine, g.Select(c => c.Result)));
        }
    }
}