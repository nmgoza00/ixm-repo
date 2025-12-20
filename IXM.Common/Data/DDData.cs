using IXM.Models;
namespace IXM.Web.Components.DataProviders
{


    public class DDData
    {

        public List<IXMDDType> GetDD_Periods(IEnumerable<MPERIOD> value)
        {
            if (value != null)
            {
                List<IXMDDType> tt = value.Select(a => new IXMDDType
                {
                    ID = a.PRID.ToString(),
                    Description = a.FYEARMONTH
                }).Distinct().ToList();
                return tt;

            }
            else return null;


        }

        public List<IXMDDType> GetDD_Union(IEnumerable<MUNION> value)
        {
            if (value != null)
            {
                List<IXMDDType> tt = value.Select(a => new IXMDDType
                {
                    ID = a.UNIONID.ToString(),
                    Description = a.DESCRIPTION
                }).Distinct().ToList();
                return tt;

            }
            else return null;


        }
    }
}
