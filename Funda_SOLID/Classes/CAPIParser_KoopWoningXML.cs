using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes
{
    public class CAPIParser_KoopWoningXML : IAPIParser_KoopWoningParser
    {
        // Convert xml data to a list of KoopWoningen
        List<CKoopWoning> IAPIParser_KoopWoningParser.ParseAPIData(string sAPIData)
        {
            List<CKoopWoning> oWoningen = new List<CKoopWoning>();
            XmlNamespaceManager oXMLNameSpace;
            XmlDocument oXML = new XmlDocument();
            XmlNodeList oNodes;
            CKoopWoning oWoning;
            string sMakelaarName;
            string sWoningID;
            int nMakelaarID;

            if (sAPIData.Length > 0)
            {
                // Load XML document, and XML-namespace
                oXML.LoadXml(sAPIData);
                oXMLNameSpace = new XmlNamespaceManager(oXML.NameTable);
                oXMLNameSpace.AddNamespace("Funda", "http://schemas.datacontract.org/2004/07/FundaAPI.Feeds.Entities");

                oNodes = oXML.GetElementsByTagName("Object");

                // Loop through all Woningen and get the relevant information (Id, MakerlaarId, MakelaarNaam)
                foreach (XmlNode oNode in oNodes)
                {
                    sWoningID = oNode.SelectSingleNode("Funda:Id", oXMLNameSpace).InnerText;
                    nMakelaarID = int.Parse(oNode.SelectSingleNode("Funda:MakelaarId", oXMLNameSpace).InnerText);
                    sMakelaarName = oNode.SelectSingleNode("Funda:MakelaarNaam", oXMLNameSpace).InnerText;

                    oWoning = new CKoopWoning(sWoningID, nMakelaarID, sMakelaarName);
                    oWoningen.Add(oWoning);
                }
            }

            // Return the list of all Woningen
            return oWoningen;
        }

    }
}
