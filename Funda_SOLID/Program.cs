using System;
using System.Collections.Generic;
using System.Configuration;
using Funda_SOLID.Classes;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID
{
    class Program
    {
        // Get the top N makelaars using the API and output to the console
        static void Main(string[] args)
        {
            // Get API URL and Key from config file
            string APIURL = ConfigurationManager.AppSettings.Get("APIURL");
            string APIKEY = ConfigurationManager.AppSettings.Get("APIKEY");

            IAPI_GetKoopWoningen oAPI = new CAPI_GetKoopWoningen(APIURL, APIKEY);
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();
            IGet_TopNMakelaars oReport = new CGet_TopNMakelaars(oAPI, oParser);
            IDisplay_TopNMakelaar oDisplay = new CDisplay_TopNMakelaarConsole();
            
            List<CMakelaar> oMakelaars;

            oDisplay.Starting("Retrieving information.Please wait...\n");

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/", 10);
            oDisplay.DisplayTopNMakelaar(oMakelaars, "Top 10 Makelaars in Amsterdam:");

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/tuin/", 10);
            oDisplay.DisplayTopNMakelaar(oMakelaars, "Top 10 Makelaars in Amsterdam with tuin:");

            oDisplay.Done();
        }
    }
}
