using Neurotoxin.ScOut.Analysis;

namespace Neurotoxin.ScOut
{
    public abstract class PostProcessor
    {
        protected AnalysisWorkspace Workspace;
        protected readonly ExcludingRules ExcludingRules;

        protected PostProcessor(AnalysisWorkspace workspace, ExcludingRules excludingRules)
        {
            Workspace = workspace;
            ExcludingRules = excludingRules;
        }

        public abstract void Process();
    }
}