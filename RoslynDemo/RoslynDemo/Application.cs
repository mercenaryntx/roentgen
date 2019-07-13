using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.ScOut;

namespace RoslynDemo
{
    public class Application
    {
        private readonly RoslynAnalyzer _analyzer;

        public Application(RoslynAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public void Run()
        {
            var sln = _analyzer.LoadSolution(@"c:\Work\NTX-SCT\EF\elektra-wpf-ls\EFLT.EO.sln").Analyze();

            var allMethods = sln.Classes.Values.SelectMany(c => c.Properties.Values.SelectMany(p => p.Accessors).Concat(c.Methods.SelectMany(m => m.Value)));
            var dependencyMatrix = allMethods.ToDictionary(m => m.FullName, m => m.Calls.Where(c => c.Target != null).Select(c => c.Target.FullName).ToArray());

            foreach (var kvp in dependencyMatrix)
            {
                Console.WriteLine(kvp.Key);
                foreach (var dep in kvp.Value)
                {
                    Console.WriteLine($"  {dep}");
                }
            }
        }
    }
}