using Funda_SOLID.Classes;
using Funda_SOLID.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Funda_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetKoopWoningen_0()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            List<CKoopWoning> oResult;
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();

            // 0 Houses
            oResult = oParser.ParseAPIData(CreateXML(oWoningen.Count, oWoningen));
            Assert.AreEqual(oWoningen.Count, oResult.Count, "GetKoopWoningen '0 Houses' Failed");
        }

        [Test]
        public void GetKoopWoningen_3()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            List<CKoopWoning> oResult;
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();

            // 3 Houses
            oWoningen.Add(new CKoopWoning("a", 1, "Makelaar_1"));
            oWoningen.Add(new CKoopWoning("b", 2, "Makelaar_2"));
            oWoningen.Add(new CKoopWoning("c", 1, "Makelaar_1"));

            oResult = oParser.ParseAPIData(CreateXML(oWoningen.Count, oWoningen));

            Assert.AreEqual(oWoningen.Count, oResult.Count, "GetKoopWoningen '3 Houses' Failed");
            for (int i = 0; i < oWoningen.Count - 1; i++)
            {
                Assert.AreEqual(oWoningen[i].WoningID, oResult[i].WoningID, "GetKoopWoningen '3 Houses' Failed");
                Assert.AreEqual(oWoningen[i].MakelaarID, oResult[i].MakelaarID, "GetKoopWoningen '3 Houses' Failed");
                Assert.AreEqual(oWoningen[i].MakelaarName, oResult[i].MakelaarName, "GetKoopWoningen '3 Houses' Failed");
            }
        }

        // Test top 1 makelaar with 0 houses
        [Test]
        public void GetTopMakelaars_0()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            CAPI_TestData oAPI = new CAPI_TestData();
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();
            IGet_TopNMakelaars oReport = new CGet_TopNMakelaars(oAPI, oParser);
            List<CMakelaar> oMakelaars;

            oAPI.AddData(1, 25, CreateXML(oWoningen.Count, oWoningen));

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/", 10);

            Assert.AreEqual(oMakelaars.Count, 0, "GetTopMakelaars '0 Houses' Failed");
        }

        // Test top 1 makelaar
        [Test]
        public void GetTopMakelaars_1()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            CAPI_TestData oAPI = new CAPI_TestData();
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();
            IGet_TopNMakelaars oReport = new CGet_TopNMakelaars(oAPI, oParser);
            List<CMakelaar> oMakelaars;

            // 3 Houses
            oWoningen.Add(new CKoopWoning("a", 1, "Makelaar_1"));
            oWoningen.Add(new CKoopWoning("b", 2, "Makelaar_2"));
            oWoningen.Add(new CKoopWoning("c", 1, "Makelaar_1"));

            oAPI.AddData(1, 25, CreateXML(oWoningen.Count, oWoningen));
            oAPI.AddData(2, 25, CreateXML(0, new List<CKoopWoning>()));

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/", 1);

            Assert.AreEqual(oMakelaars.Count, 1, "GetTopMakelaars 'Top 1' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarID, 1, "GetTopMakelaars 'Top 1' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarName, "Makelaar_1", "GetTopMakelaars 'Top 1' Failed");
            Assert.AreEqual(oMakelaars[0].NumWoningen, 2, "GetTopMakelaars 'Top 1' Failed");
        }

        // Test top 2 makelaar
        [Test]
        public void GetTopMakelaars_2()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            CAPI_TestData oAPI = new CAPI_TestData();
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();
            IGet_TopNMakelaars oReport = new CGet_TopNMakelaars(oAPI, oParser);
            List<CMakelaar> oMakelaars;

            // 3 Houses
            oWoningen.Add(new CKoopWoning("a", 1, "Makelaar_1"));
            oWoningen.Add(new CKoopWoning("b", 2, "Makelaar_2"));
            oWoningen.Add(new CKoopWoning("c", 1, "Makelaar_1"));

            oAPI.AddData(1, 25, CreateXML(oWoningen.Count, oWoningen));
            oAPI.AddData(2, 25, CreateXML(0, new List<CKoopWoning>()));

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/", 2);

            Assert.AreEqual(oMakelaars.Count, 2, "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarID, 1, "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarName, "Makelaar_1", "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[0].NumWoningen, 2, "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[1].MakelaarID, 2, "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[1].MakelaarName, "Makelaar_2", "GetTopMakelaars 'Top 2' Failed");
            Assert.AreEqual(oMakelaars[1].NumWoningen, 1, "GetTopMakelaars 'Top 2' Failed");
        }

        /// Test top 10 makelaars, with only 2 makelaars
        [Test]
        public void GetTopMakelaars_10()
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            CAPI_TestData oAPI = new CAPI_TestData();
            IAPIParser_KoopWoningParser oParser = new CAPIParser_KoopWoningXML();
            IGet_TopNMakelaars oReport = new CGet_TopNMakelaars(oAPI, oParser);
            List<CMakelaar> oMakelaars;

            // 3 Houses
            oWoningen.Add(new CKoopWoning("a", 1, "Makelaar_1"));
            oWoningen.Add(new CKoopWoning("b", 2, "Makelaar_2"));
            oWoningen.Add(new CKoopWoning("c", 1, "Makelaar_1"));

            oAPI.AddData(1, 25, CreateXML(oWoningen.Count, oWoningen));
            oAPI.AddData(2, 25, CreateXML(0, new List<CKoopWoning>()));

            oMakelaars = oReport.GetTopNMakelaars("/amsterdam/", 10);

            Assert.AreEqual(oMakelaars.Count, 2, "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarID, 1, "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[0].MakelaarName, "Makelaar_1", "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[0].NumWoningen, 2, "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[1].MakelaarID, 2, "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[1].MakelaarName, "Makelaar_2", "GetTopMakelaars 'Top 10' Failed");
            Assert.AreEqual(oMakelaars[1].NumWoningen, 1, "GetTopMakelaars 'Top 10' Failed");
        }

        // Create cutdown version of XML used for testing
        private string CreateXML(int nNumWomingen, List<CKoopWoning> oWoningen)
        {
            string sWoningen = "";

            foreach (CKoopWoning oWoning in oWoningen)
            {
                sWoningen += "<Object><Id>" + oWoning.WoningID + "</Id>" +
                                     "<MakelaarId>" + oWoning.MakelaarID.ToString() + "</MakelaarId>" +
                                     "<MakelaarNaam>" + oWoning.MakelaarName + "</MakelaarNaam>" +
                             "</Object>";
            }
            return "<LocatieFeed xmlns =\"http://schemas.datacontract.org/2004/07/FundaAPI.Feeds.Entities\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><AccountStatus>Unknown</AccountStatus><EmailNotConfirmed>false</EmailNotConfirmed><ValidationFailed>false</ValidationFailed><ValidationReport i:nil=\"true\" xmlns:a=\"http://funda.nl/api/2010-05-11/validation\"/><Website>None</Website><Metadata><ObjectType>Koopwoningen</ObjectType><Omschrijving>Koopwoningen &gt; Hengelo (OV)</Omschrijving><Titel>Huizen te koop in Hengelo (OV)</Titel></Metadata><Objects>" +
                   sWoningen +
                   "</Objects><Paging><AantalPaginas>1</AantalPaginas><HuidigePagina>25</HuidigePagina><VolgendeUrl>/~/koop/hengelo-ov/p26/</VolgendeUrl><VorigeUrl>/~/koop/hengelo-ov/p24/</VorigeUrl></Paging><TotaalAantalObjecten>" + nNumWomingen.ToString() + "</TotaalAantalObjecten></LocatieFeed>";
        }
    }
}