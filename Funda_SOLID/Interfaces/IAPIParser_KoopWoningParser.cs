using System.Collections.Generic;
using Funda_SOLID.Classes;

namespace Funda_SOLID.Interfaces
{
    // Parser to convert information received from the API to a List of CKoopWoningen
    public interface IAPIParser_KoopWoningParser
    {
        List<CKoopWoning> ParseAPIData(string sAPIData);
    }
}
