using System;
using System.Collections.Generic;
using System.Text;
using Funda_SOLID.Interfaces;

namespace Funda_SOLID.Classes 
{
    // Output information to the Console
    class CDisplay_TopNMakelaarConsole : IDisplay_TopNMakelaar
    {
        // 'Starting' message
        void IDisplay_TopNMakelaar.Starting(string sMessage)
        {
            Console.WriteLine(sMessage);
        }

        void IDisplay_TopNMakelaar.Done()
        {
            // nothing to output for the Console
        }

        void IDisplay_TopNMakelaar.DisplayTopNMakelaar(List<CMakelaar> oMakelaars, string sTitle)
        {
            Console.WriteLine(sTitle);
            foreach (CMakelaar oMakelaar in oMakelaars)
            {
                Console.WriteLine(oMakelaar.NumWoningen.ToString() + " huizen: " + oMakelaar.MakelaarID + " (" + oMakelaar.MakelaarName + ")");
            }
            Console.WriteLine("");
        }
    }
}
