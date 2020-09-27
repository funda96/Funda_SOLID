namespace Funda_SOLID.Interfaces
{
    // API interface to get koopwoningen
    public interface IAPI_GetKoopWoningen
    {
        string GetKoopWoningen(string sQuery, int nPage, int nPageSize);
    }
}
