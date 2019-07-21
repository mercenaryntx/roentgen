using System;
using System.Diagnostics;
using System.Linq;

namespace Neurotoxin.ScOut.Models
{
    public class Solution : FileCodePart
    {
        public Project[] Projects => Children.Cast<Project>().ToArray();
        //public Dictionary<string, Class> Classes
        //{
        //    get
        //    {
        //        //HACK
        //        var q = Projects.SelectMany(p => p.Classes).Where(c => !c.Key.StartsWith("XamlGeneratedNamespace"));
        //        var x = q.GroupBy(c => c.Key).Where(g => g.Count() > 1).ToDictionary(g => g.Key, g => g.ToArray());
        //        if (x.Any()) Debugger.Break();
        //        return q.ToDictionary(c => c.Key, c => c.Value);
        //    }
        //}

        public override string ToString() => FullName;
    }
}