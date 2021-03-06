﻿using System.Linq;

namespace Neurotoxin.Roentgen.CSharp.Models
{
    public class Solution : FileCodePart
    {
        public Project[] Projects => Children.Cast<Project>().ToArray();
        public override string ToString() => FullName;
    }
}