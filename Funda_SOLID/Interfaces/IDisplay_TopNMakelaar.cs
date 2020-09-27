using System;
using System.Collections.Generic;
using System.Text;
using Funda_SOLID.Classes;

namespace Funda_SOLID.Interfaces
{
    // Display the top makelaars
    interface IDisplay_TopNMakelaar
    {
        // Let the user know we have started
        void Starting(string sMessage);

        // Let the user know we're done / hide any busy messages
        void Done();

        void DisplayTopNMakelaar(List<CMakelaar> oMakelaars, string sTitle);
    }
}
