using System.Collections.Generic;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes
{
    public class CGet_TopNMakelaars : IGet_TopNMakelaars
    {
        IAPI_GetKoopWoningen moAPI = null;
        IAPIParser_KoopWoningParser moParser = null;

        // Constructor
        public CGet_TopNMakelaars(IAPI_GetKoopWoningen oAPI, IAPIParser_KoopWoningParser oParser)
        {
            moAPI = oAPI;
            moParser = oParser;
        }

        // Get the top N makelaars for a given query
        List<CMakelaar> IGet_TopNMakelaars.GetTopNMakelaars(string sQuery, int nTopN)
        {
            List<CKoopWoning> oWoningen;
            Dictionary<int, Dictionary<string, CKoopWoning>> oWoningenByMakelaar = new Dictionary<int, Dictionary<string, CKoopWoning>>();
            Dictionary<int, string> oMakelaarNames = new Dictionary<int, string>();
            IThrottle oThrottle = new CThrottle();

            int nPage = 1;
            
            do
            {
                string sXML = moAPI.GetKoopWoningen(sQuery, nPage, 25);
                oWoningen = moParser.ParseAPIData(sXML);
                CollectInfo(oWoningen, oWoningenByMakelaar, oMakelaarNames);
                oThrottle.RateLimitCalls(100, 60);       // Rate limit to 100 requests per 60 seconds
                nPage += 1;
            }
            while (oWoningen.Count>0) ;

            // Return the Top N makelaars
            return LimitToTopN(oMakelaarNames, oWoningenByMakelaar, nTopN); ;
        }           

        // Process all the woningen found, and collate by makelaar
        void CollectInfo(List<CKoopWoning> oWoningen, 
                         Dictionary<int, Dictionary<string, CKoopWoning>> oWoningenByMakelaar,
                         Dictionary<int, string> oMakelaarNames)
        {
            foreach (CKoopWoning oWoning in oWoningen)
            {
                if (!oWoningenByMakelaar.ContainsKey(oWoning.MakelaarID))
                {
                    oWoningenByMakelaar.Add(oWoning.MakelaarID, new Dictionary<string, CKoopWoning>());
                    oMakelaarNames.Add(oWoning.MakelaarID, oWoning.MakelaarName);
                }
                oWoningenByMakelaar[oWoning.MakelaarID][oWoning.WoningID] = oWoning;
            }
        }

        // Sort, but keep the top 10 only. Sorting 10 elements repeatedly is still O(1)
        // with a total runtime of O(N)
        // Because multiple makelaars can have the same number of houses we use:
        //    oMost     Number of houses -> List of makelaars
        // Multiple makelaars might have the same number of houses available. 
        // Limit to exactly the nTopN makelaars.
        List<CMakelaar> LimitToTopN(Dictionary<int, string> oMakelaarNames, 
                                    Dictionary<int, Dictionary<string, CKoopWoning>> oWoningenByMakelaar,
                                    int nTopN) 
        {
            List<CMakelaar> oMakelaars = new List<CMakelaar>();

            SortedList<int, List<int>> oMost = new SortedList<int, List<int>>();

            foreach (int nMakelaarID in oWoningenByMakelaar.Keys)
            {
                int nNumWoningen = oWoningenByMakelaar[nMakelaarID].Count;

                // Add to the list of the top makelaars, if better than the bottom makelaar we have stored
                if (oMost.Count < nTopN || nNumWoningen > oMost.Keys[0])
                {
                    // Add new entry if not already existing. Remove the worst one from the list 
                    if (!oMost.ContainsKey(nNumWoningen))
                    {
                        if (oMost.Count >= nTopN)
                            oMost.Remove(oMost.Keys[0]);

                        oMost.Add(nNumWoningen, new List<int>());
                    }

                    oMost[nNumWoningen].Add(nMakelaarID);
                }
            }

            // Limit to topN
            for (int i = oMost.Count - 1; i >= 0; i--)
            {
                foreach (int nMakelaarID in oMost[oMost.Keys[i]])
                {
                    oMakelaars.Add(new CMakelaar(nMakelaarID, oMakelaarNames[nMakelaarID], oWoningenByMakelaar[nMakelaarID].Count));
                    if (oMakelaars.Count == nTopN)
                        return oMakelaars;                  // We're done
                }
            }

            // Return all the makelaars, if there are fewer than the requested number
            return oMakelaars;
        }
    }
}
