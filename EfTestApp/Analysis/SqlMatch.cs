using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.Roentgen.Data.Constants;

namespace EfTestApp.Analysis
{
    public class SqlMatch
    {
        public QueryType Type { get; set; }
        public string[] Targets { get; set; }
    }
}
