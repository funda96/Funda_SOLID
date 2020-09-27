using System;
using System.Collections.Generic;
using System.Text;

namespace Funda_SOLID.Interfaces
{
    interface IThrottle
    {
        // Limit the maximum rate/sec of calls to make
        void RateLimitCalls(int nMaxCalls, int nNumSeconds);
    }
}
