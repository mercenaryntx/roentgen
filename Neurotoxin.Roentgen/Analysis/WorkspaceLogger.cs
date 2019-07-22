using System;

namespace Neurotoxin.Roentgen.Analysis
{
    public class WorkspaceLogger<TSource> : ILogger<TSource>
    {
        private readonly AnalysisWorkspace _workspace;
        private readonly string _source;

        public WorkspaceLogger(AnalysisWorkspace workspace)
        {
            _workspace = workspace;
            _source = typeof(TSource).Name;
        }

        public void Info(string message)
        {
            _workspace.Diagnostics.Add(new LogMessage(_source, LogLevel.Info, message));
        }

        public void Warning(string message)
        {
            _workspace.Diagnostics.Add(new LogMessage(_source, LogLevel.Warning, message));
        }

        public void Error(Exception exception)
        {
            _workspace.Diagnostics.Add(new LogMessage(_source, exception));
        }
    }
}