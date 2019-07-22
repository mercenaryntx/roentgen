using Neurotoxin.Roentgen.CSharp.Analysis;

namespace Neurotoxin.Roentgen.CSharp.PostProcessors
{
    public abstract class PostProcessorBase
    {
        protected AnalysisWorkspace Workspace;
        protected readonly ExcludingRules ExcludingRules;

        protected PostProcessorBase(AnalysisWorkspace workspace, ExcludingRules excludingRules)
        {
            Workspace = workspace;
            ExcludingRules = excludingRules;
        }

        public abstract void Process();
    }
}