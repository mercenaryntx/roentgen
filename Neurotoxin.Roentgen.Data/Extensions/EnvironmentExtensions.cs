using System;

namespace Neurotoxin.Roentgen.Data.Extensions
{
    public static class EnvironmentExtensions
    {
        public static string GetCurrentUserName()
        {
            try
            {
                return String.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
            }
            catch
            {
                return String.Format("UNKNOWN\\{0}", Environment.UserName);
            }
        }
    }
}
