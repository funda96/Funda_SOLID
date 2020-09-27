using System;
using System.Collections.Generic;
using System.Text;
using Funda_SOLID.Classes;

namespace Funda_SOLID.Interfaces
{
    // Get the top N makelaars for a given query
    public interface IGet_TopNMakelaars
    {
        List<CMakelaar> GetTopNMakelaars(string sQuery, int nTopN);
    }
}
