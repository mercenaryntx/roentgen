using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurotoxin.ScOut.Models
{
    public class Solution
    {
        public string Path { get; set; }
        public Project[] Projects { get; set; }
        public Dictionary<string, Class> Classes => Projects.SelectMany(p => p.Classes).ToDictionary(c => c.Key, c => c.Value);

        public override string ToString() => Path;
    }
}