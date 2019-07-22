using System;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class LogMessage
    {
        public string Source { get; }
        public LogLevel Level { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public LogMessage(string source, LogLevel level, string message)
        {
            Source = source;
            Level = level;
            Message = message;
        }

        public LogMessage(string source, Exception exception)
        {
            Source = source;
            Level = LogLevel.Error;
            Message = exception.Message;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"[{Source}] {Level.ToString().ToUpper()} {Message}";
        }
    }
}