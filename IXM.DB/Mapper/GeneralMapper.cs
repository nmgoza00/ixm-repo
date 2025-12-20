using IXM.Models.Core;

namespace IXM.DB.Mapper
{
    public class GeneralMapper
    {


        public async Task<TRMBL> RemmittancePostMaptoDB(TRMBL_POST rMBL)
        {
            
            TRMBL lv = new TRMBL
            {
                RMBLID = rMBL.RMBLID,
                RMBLNUM = rMBL.RMBLNUM,
                RSTYPEID = (int)rMBL.RSTYPEID,
                COMPANYID = rMBL.COMPANYID,
                OBJECTID = rMBL.OBJECTID,
                POPOBJECTID = rMBL.POPOBJECTID,
                EMLOBJECTID = rMBL.EMLOBJECTID,
                STATUSID = rMBL.STATUSID,
                PAYMENTID = rMBL.PAYMENTID,
                PAYMENTREF = rMBL.PAYMENTREF,
                INSBY = rMBL.INSBY,
                INSDAT = rMBL.INSDAT,
                MODAT = DateTime.Now,
                PERIODID = rMBL.PERIODID,
                MODBY = rMBL.MODBY,
                FLOADS = rMBL.FLOADS,
                MEMBERS = rMBL.MEMBERS,
                ADMINFEE = rMBL.ADMINFEE,
                ADMINFEETYPE = rMBL.ADMINFEETYPE,
                IAMOUNT = rMBL.IAMOUNT
            };

            return lv;

        }
    }
}
