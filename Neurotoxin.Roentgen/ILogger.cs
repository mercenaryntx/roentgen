using System;

namespace Neurotoxin.Roentgen
{
    public interface ILogger<TSource>
    {
        void Info(string message);
        void Warning(string message);
        void Error(Exception exception);
    }
}