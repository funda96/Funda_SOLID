using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes
{
    public class CAPI_GetKoopWoningen : IAPI_GetKoopWoningen
    {
        // API URL and Key
        private string APIURL = "";
        private string APIKEY = "";

        // HttpClient is designed for re-use. For better performance use one instance only
        private HttpClient moAPIClient;

        // Constructor: API Key and URL to connect to. Instantiate HttpClient
        public CAPI_GetKoopWoningen(string sAPIURL, string sAPIKEY)
        {
            APIURL = sAPIURL;
            APIKEY = sAPIKEY;
            moAPIClient = new HttpClient();
            moAPIClient.BaseAddress = new Uri(APIURL);
        }

        string IAPI_GetKoopWoningen.GetKoopWoningen(string sQuery, int nPage, int nPageSize)
        {
            string sURL = "feeds/Aanbod.svc/" + APIKEY + "/?type=koop&zo=" + sQuery + "&page=" + nPage.ToString() + "&pagesize=" + nPageSize.ToString();
            string sXML = "";
            int nNumTries = 0;
            Task<string> oTask;

            do
            {
                try
                {
                    oTask = moAPIClient.GetStringAsync(sURL);
                    oTask.Wait();
                    sXML = oTask.Result;
                }
                catch
                {
                    // Try again in 1 sec. Try 60 times. If still failure, then throw exception
                    System.Threading.Thread.Sleep(1000);
                    nNumTries += 1;
                    if (nNumTries > 60)
                        throw;
                }
            } while (sXML.Length == 0);

            return sXML;
        }
    }
}
