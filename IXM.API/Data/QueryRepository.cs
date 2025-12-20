namespace IXM.DB
{
    public class QueryRepository : IQueryRepository
    {

        public enum IXMFileType
        {
            Image,
            Document,
            Text
        }

        public string GetOrgnaniser_Company(string pUName)
        {
            var lSql = " SELECT BCOMPANY.*, TPAYMENT.PAYMENTNUM, TPAYMENTSB.IAMOUNT,  MPERIOD.MYEARMONTH " +
                " FROM (SELECT BCOMPANY.*, (SELECT MAX(PAYMENTID) FROM TPAYMENTSB WHERE TPAYMENTSB.BCOMPANYID = BCOMPANY.BCOMPANYID) PAYMENTID " +
                " FROM (SELECT b.COMPANYID BCOMPANYID, b.COMPANYNUM, b.EXT_COCODE, u.NAME ||' '||u.SURNAME PROCESSOR_NAME, " +
                " u.EMAILADDRESS PROCESSOR_EMAIL, u.CELLNUMBER PROCESSOR_CELLNUMBER FROM CCUR c, MCOMPANY b, MUSER u WHERE C.SRCOBJ= 'MUSER' " +
                " AND c.COMPANYID = b.COMPANYID AND b.COMPTYPID = 4" +
                "  AND c.USERID = u.USERID\r\n AND u.UNAME = '" + pUName + "') BCOMPANY ) BCOMPANY ";
            return lSql;
        }


        public string GetIDT_UerRoles(string pUName)
        {
            var lSql = " SELECT ur.*, r.\"Name\" RoleName, u.\"UserName\" FROM \"UserRoles\" ur, \"Users\" u, \"Roles\" r" +
                " WHERE ur.\"RoleId\" = r.\"Id\" AND ur.\"UserId\" = u.\"Id\"";

            if (!string.IsNullOrEmpty(pUName))
            {
                lSql = lSql + " AND u.\"UserName\" = " + pUName;

            }
            return lSql;
        }
    }
}
