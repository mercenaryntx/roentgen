using System.Collections.Generic;
using System.Linq;

namespace Neurotoxin.Roentgen.CSharp.Models
{
    public class Project : FileCodePart
    {
        public Dictionary<string, SourceFile> SourceFiles => Children.Cast<SourceFile>().ToDictionary(c => c.FullName, c => c);

        public string Language { get; set; }
        public string TargetFramework { get; set; }
        public IEnumerable<Class> AllClasses => Children.SelectMany(GetClasses);

        public override string ToString() => FullName;

        private IEnumerable<Class> GetClasses(ICodePart parent)
        {
            foreach (var @class in parent.Children.OfType<Class>())
            {
                yield return @class;
                foreach (var subClass in GetClasses(@class))
                {
                    yield return subClass;
                }
            }
        }
    }
}