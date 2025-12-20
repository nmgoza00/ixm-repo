namespace IXM.DB.Services
{
    public interface IDataValidator
    {

        Tuple<int, int, string> HasRemmittanceLoaded(string pPeriod, string pCompanyId);
        Tuple<int, int, string> HasB2BRemmittanceLoaded(int pPeriod, int pCompanyId);

    }
}