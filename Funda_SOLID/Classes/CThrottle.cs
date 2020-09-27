using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes
{
    class CThrottle : IThrottle
    {
        // Limit number of calls to the specified rate
        void IThrottle.RateLimitCalls(int nMaxCalls, int nNumSeconds)
        {
            System.Threading.Thread.Sleep(nNumSeconds*1000/ nMaxCalls);         // Rate limit to 100 requests per 60 seconds
        }
    }
}
