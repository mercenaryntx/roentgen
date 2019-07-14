using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurotoxin.ScOut.Models
{
    public class Solution
    {
        public string Path { get; set; }
        public Project[] Projects { get; set; }
        public Dictionary<string, Class> Classes
        {
            get
            {
                //HACK
                return Projects.SelectMany(p => p.Classes).Where(c => !c.Key.StartsWith("XamlGeneratedNamespace")).ToDictionary(c => c.Key, c => c.Value);
            }
        }

        public override string ToString() => Path;
    }
}