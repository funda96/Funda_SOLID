using System;
using System.Collections.Generic;
using System.Text;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes
{
    // Class to simulate HttpClient / return XML data needed for unit testing
    public class CAPI_TestData : IAPI_GetKoopWoningen
    {
        Dictionary<string, string> moData = new Dictionary<string, string>();

        public void AddData(int nPage, int nPageSize, string sXML)
        {
            moData.Add(nPage.ToString() + "_" + nPageSize.ToString(), sXML);
        }

        string IAPI_GetKoopWoningen.GetKoopWoningen(string sQuery, int nPage, int nPageSize)
        {
            return moData[nPage.ToString() + "_" + nPageSize.ToString()];
        }
    }
}
