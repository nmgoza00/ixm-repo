using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace IXM.GeneralSQL
{

    public class GeneralSQL : IGeneralSQL
    {

        public string GetPeriodByValue(string Period)
        {
            if (int.TryParse(Period, out int j))
            {

                return "SELECT * FROM MPERIOD WHERE PRID = " + Period;
            }
            else
            {

                return "SELECT * FROM MPERIOD WHERE FYEARMONTH = '" + Period + "'";
            }

        }
        public string GetPeriodPayment(string CompanyId, string PeriodId)
        {
            string lval = "SELECT * FROM TPAYMENT WHERE CUSTOMERID = " + CompanyId + " AND PERIODID = " + PeriodId + " AND PSTATUSID = " +
                "(SELECT STATUSID FROM MSTATUS WHERE STATUS_TYPE = 'TPAYMENT'AND STATUS_SEQ = 6)";
            return lval;

        }
        public string GetNotInRemittanceMembers(string RMBLID)
        {
            string lval = "SELECT MMEMBER.* FROM MMEMBER INNER JOIN TRMBL ON TRMBL.RMBLID = " + RMBLID +
                " AND TRMBL.COMPANYID = MMEMBER.COMPANYID " +
                " LEFT JOIN TRMBLD ON TRMBL.RMBLID = TRMBLD.RMBLID " +
                " WHERE TRMBLD.RMBLID IS NULL";

            return lval;

        }

        public string CheckIfRemittanceFullyProcessed(string RMBLID)
        {
            string lval = "SELECT COUNT(1) COUNTER FROM TRMBL, TPAYMENT WHERE TRMBL.PAYMENTID = TPAYMENT.PAYMENTID AND TRMBL.RMBLID = " + RMBLID;

            return lval;

        }

        public string GetRemittanceError(string COMPANYID)
        {
            string lval = "SELECT TRMBLE.* FROM TRMBLE, TRMBL WHERE TRMBL.RMBLID = TRMBLE.RMBLID AND TRMBL.COMPANYID = " + COMPANYID;

            return lval;

        }


        public string GetDocumentByObjectId(string ObjectId)
        {

            var lSql = " SELECT MD.*,m.CODE_TEXT, M.CODE_FPATH FROM MOBJECT_DOC md LEFT JOIN MCODE m ON m.CODE_VALUE = md.DOCTYPE WHERE md.SOURCEOBJ NOT IN('TPAYMENT') " +
                       " AND md.OBJECTID = " + ObjectId;

            return lSql;

        }

        public string GetMemberPayments(string MEMBERID)
        {
            var lSql = " SELECT m.COMPANYID, m.HGUID, m.CNAME,m.COMPANYNUM,t.PAYMENTID, td.IAMOUNT PAMOUNT, t.PAYMENTNUM, " +
                       " p.FYEARMONTH PYEARMONTH, t.PERIODID, td.MEMBERID " +
                       " FROM TPAYMENT t " +
                       " INNER JOIN TPAYMENT_DET td ON t.PAYMENTID = td.PAYMENTID " +
                       " LEFT JOIN MCOMPANY m ON t.CUSTOMERID = m.COMPANYID " +
                       " INNER JOIN MPERIOD p ON p.PRID = t.PERIODID " +
                       " WHERE td.MEMBERID = " + MEMBERID +
                       " ORDER BY P.FYEARMONTH ";
            return lSql;
        }

        public string GetPayment(string PaymentGuid)
        {
            var lSql = " SELECT TPAYMENT_DET.*, TPAYMENT.PAYMENTNUM,MPERIOD.MYEARMONTH,TPAYMENTSC.MEMCOUNT,TPAYMENT.DEBTORNAME, TPAYMENT.IAMOUNT HEADER_IAMOUNT,TPAYMENT.iadminfee " +
            " FROM TPAYMENT_DET " +
            " INNER JOIN TPAYMENT ON TPAYMENT.PAYMENTID = TPAYMENT_DET.PAYMENTID " +
            " LEFT OUTER JOIN TPAYMENTSC ON TPAYMENT.PAYMENTID = TPAYMENTSC.PAYMENTID and TPAYMENTSC.DETYPE = 'HD' " +
            " INNER JOIN MUSER ON MUSER.UNAME = TPAYMENT.INSERTED_BY " +
            " LEFT OUTER JOIN MPERIOD ON TPAYMENT.PERIODID = MPERIOD.PRID " +
            " LEFT OUTER JOIN MOBJECT_DOC MDOC ON MDOC.sourceid = TPAYMENT.PAYMENTID AND MDOC.sourcefld = 'PAYMENTID' AND MDOC.sourceobj='TPAYMENT' AND MDOC.LT = 1 AND MDOC.doctype = 'PAYXLS' " +
            " WHERE TPAYMENT.HGUID = '" + PaymentGuid + "'";

            return lSql;

        }
        public string GetPaymentConfirmation(string UserGuid)
        {
            // This exclused Bank Payments for Comparison
            // The Bank Amounts will still need to be included in this script. I  Infinit....A Block |execute was used, however, calling blocks in Entity Framework seems to be having an issue.
            var lSql = " SELECT TPAYMENT.PAYMENTID,TPAYMENT.HGUID,TPAYMENT.PAYMENTNUM,TPAYMENT.PERIODID,MPERIOD.MYEARMONTH,TPAYMENTSC.MEMCOUNT,TPAYMENT.customerid,TPAYMENT.DEBTORNAME,TPAYMENT.INSERTED_BY,TPAYMENT.MODIFIED_BY,TPAYMENT.insert_date, TPAYMENT.MODIFIED_DATE, TPAYMENT.IAMOUNT,TPAYMENT.iadminfee " +
            " FROM TPAYMENT " +
            " LEFT OUTER JOIN TPAYMENTSC ON TPAYMENT.PAYMENTID = TPAYMENTSC.PAYMENTID and TPAYMENTSC.DETYPE = 'HD' " +
            " INNER JOIN MUSER ON MUSER.UNAME = TPAYMENT.INSERTED_BY " +
            " LEFT OUTER JOIN MPERIOD ON TPAYMENT.PERIODID = MPERIOD.PRID " +
            " LEFT OUTER JOIN MOBJECT_DOC MDOC ON MDOC.sourceid = TPAYMENT.PAYMENTID AND MDOC.sourcefld = 'PAYMENTID' AND MDOC.sourceobj='TPAYMENT' AND MDOC.LT = 1 AND MDOC.doctype = 'PAYXLS' " +
            " WHERE TPAYMENT.PSTATUSID IN (SELECT STATUSID FROM MSTATUS WHERE STATUS_TYPE = 'TPAYMENT' AND STATUS_SEQ=1)";

            if (UserGuid != "")
            {
                lSql += " AND MUSER.AUTHCODE = '" + UserGuid + "'";
            }

            return lSql;
        }


        public string GetRemittanceForPaymentDetail(string RMBLID)
        {
            var lSql = "SELECT " +
                " DB_MEMBERID, " +
                " TRMBLD.RMBLID, " +
                " MAX(RMBLDID) RMBLDID, " +
                " DB_COMPANYID, " +
                " DB_BCOMPANYID, " +
                " DB_CITYID, " +
                " DB_LOCALITYID, " +
                " DB_PROVINCEID, " +
                " DB_MEMSTATUSID, " +
                " TRMBL.PERIODID , " +
                " TRMBL.INSBY, " +
                " DTF_IDNUMBER , " +
                " MAX(DTF_MNAME) DTF_MNAME ," +
                " MAX(DTF_MSURNAME) DTF_MSURNAME , " +
                " SUM(TRL_EAMOUNT) TRL_EAMOUNT, " +
                " SUM(TRL_IAMOUNT) TRL_IAMOUNT, " +
                " SUM(TRL_SALARY) TRL_SALARY, " +
                " '' ANYPROB, " +
                " '' CIT, " +
                " 1 LT, " +
                " COUNT(1) LTCNT" +
                " FROM TRMBLD " +
                " INNER JOIN TRMBL ON TRMBL.RMBLID = TRMBLD.RMBLID " +
                " WHERE TRMBLD.RMBLID = " + RMBLID +
                " GROUP BY DB_MEMBERID,TRMBL.PERIODID, TRMBLD.RMBLID, DB_COMPANYID, DTF_IDNUMBER, DB_BCOMPANYID, DB_PROVINCEID, DB_CITYID, DB_LOCALITYID, DB_MEMSTATUSID, TRMBL.INSBY";
            return lSql;
        }
        public string StatusTriggerCreate(string TBNAME, string CHKFIELD, string KEYFIELD)
        {
            var lSQL = " CREATE TRIGGER " + TBNAME + "_" + CHKFIELD + "_IU FOR " + TBNAME +
                    " AFTER INSERT OR UPDATE " +
                    " AS BEGIN " +
                    " IF ((OLD." + CHKFIELD + " <> NEW." + CHKFIELD + ") OR (OLD." + CHKFIELD + " IS NULL AND NEW." + KEYFIELD + " IS NOT NULL)) THEN BEGIN" +
                    "    UPDATE MOBJECT_STA SET LT = 0 WHERE SOURCEOBJ = '" + TBNAME + "' AND SOURCEFLD = '" + KEYFIELD + "' AND SOURCEID = NEW." + KEYFIELD + "; " +
                    "    INSERT INTO MOBJECT_STA (OBJECTID,SOURCEOBJ,SOURCEFLD,SOURCEID,FSTATUSID,TSTATUSID,INSDT,INSBY,LT) VALUES (" +
                    "    GEN_ID(SEQOBJECT_STA, 1),'" + TBNAME + "','" + KEYFIELD + "',NEW." + KEYFIELD + ",OLD." + CHKFIELD + ",NEW." + CHKFIELD + ",current_timestamp, NEW.MODBY,1);" +
                    " END END;";

            return lSQL;
        }
        public string StatusTriggerDrop(string TBNAME, string KEYFIELD)
        {


            var lSql = "EXECUTE BLOCK AS BEGIN " +
                       "IF (EXISTS(SELECT 1 FROM rdb$triggers where rdb$trigger_name = '" + TBNAME + "_" + KEYFIELD + "_IU') ) then " +
                       " EXECUTE STATEMENT 'DROP TRIGGER " + TBNAME + "_" + KEYFIELD + "_IU';" +
                       " END";

            return lSql;
        }

        public string GetSystemUserGroup(string UserGuId)
        {
            var lSql = "SELECT u.USERID , u.AUTHCODE, c.UGID, mg.DESCRIPTION, mg.UGCODE FROM MUSER u INNER JOIN CUSER_GROUP c ON c.USERID = u.USERID INNER JOIN MUSER_GROUP mg ON mg.UGID = c.UGID" +
                        " WHERE u.AUTHCODE = '" + UserGuId + "'";

            return lSql;
        }

        public string GetUserLinkedCompanies(Guid UserGuid)
        {
            var lSql = "SELECT u.AUTHCODE, m.COMPANYID, m.HGUID, m.CNAME, m.COMPANYNUM, m.EXT_COCODE, m.REG_NUM, m.E_MAIL, m.CONTACT_PERSON, m.TEL_NUMBER, CAST(m.CELL_NUMBER AS VARCHAR(20)) CELL_NUMBER, " +
                       " m.WEBADDRESS, m.COMPTYPID, m.STIC, u.NAME ||' '||u.SURNAME PROCESSOR_NAME, u.EMAILADDRESS PROCESSOR_EMAIL, u.CELLNUMBER PROCESSOR_CELLNUMBER, m.MMEMCOUNT, m.ACTIVEMEMCOUNT, m.ALLMEMCOUNT " +
                       " FROM MUSER u INNER JOIN CCUR c ON c.SRCOBJ = 'MUSER' AND c.SRCID = u.USERID INNER JOIN VMCOMPANY m ON m.COMPANYID = c.COMPANYID " +
                       " WHERE u.AUTHCODE = '" + UserGuid + "'";
            return lSql;
        }


        public string GetCompanyMembers(string CompanyGUID)
        {
            var lSql = "SELECT FIRST 1000 v.*, mc.DESCRIPTION CITYNAME, m.PYEAR || ' ' || m.PMONTH PYEARMONTH, CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END MemCardReceived " +
                       " FROM MMEMBER v " +
                       " INNER JOIN MCOMPANY c ON v.COMPANYID = c.COMPANYID " +
                       " LEFT JOIN MCITY mc ON v.CITYID = mc.CITYID " +
                       " LEFT JOIN MMCDD mcd ON v.MEMBERID = mcd.MEMBERID " +
                       " LEFT JOIN MMLP m ON m.MEMBERID = v.MEMBERID WHERE c.HGUID = '" + CompanyGUID + "'";
            return lSql;
        }

        public string GetCompanyPayments(string ComapnyGUID)
        {
            //For the sake of EF Core....I neeed tro use t.* - Will investigate furthr pn how to mitigate
            var lSql = "SELECT t.*, m.COMPANYID, m.CNAME, m.COMPANYNUM,t.IAMOUNT PAMOUNT, " +
                                            $" p.FYEARMONTH PYEARMONTH, " +
                                            $" (SELECT COUNT(DISTINCT MEMBERID) FROM TPAYMENT_DET td WHERE td.PAYMENTID = t.PAYMENTID AND td.LT = 1) MEMCOUNT " +
                                            $" FROM TPAYMENT t " +
                                            $" LEFT JOIN MCOMPANY m ON t.CUSTOMERID = m.COMPANYID " +
                                            $" INNER JOIN MPERIOD p ON p.PRID = t.PERIODID " +
                                            $" WHERE t.CUSTOMERID = " + ComapnyGUID +
                                            $" ORDER BY P.FYEARMONTH ";
            return lSql;

        }


        public string GetCompaniesList(string pUName)
        {
            var lSql = " WITH RECURSIVE Payments AS(SELECT b1.*FROM tpaymentsc b1 " +
                       " WHERE b1.PAYMENTSCID = (SELECT MAX(b2.PAYMENTSCID) FROM TPAYMENTSC b2 " +
                       " WHERE b2.CUSTOMERID = b1.CUSTOMERID)) " +
                       " SELECT FIRST 300 TP.PAYMENTNUM, TP.TRANCOUNT, TP.MEMCOUNT PMEMCOUNT, TP.LINUM, TP.DETYPE, VMCOMPANY.* FROM vmcompany " +
                       " LEFT JOIN Payments TP ON TP.CUSTOMERID = vmcompany.COMPANYID ";
            return lSql;
        }


        public string GetCompanyById(string CompanyGUID)
        {
            var lSql = " WITH RECURSIVE Payments AS(SELECT b1.*FROM tpaymentsc b1 " +
                       " WHERE b1.PAYMENTSCID = (SELECT MAX(b2.PAYMENTSCID) FROM TPAYMENTSC b2 " +
                       " WHERE b2.CUSTOMERID = b1.CUSTOMERID)) " +
                       " SELECT FIRST 2000 TP.PAYMENTNUM, TP.TRANCOUNT, TP.MEMCOUNT PMEMCOUNT, TP.LINUM, TP.DETYPE, VMCOMPANY.* FROM vmcompany " +
                       " LEFT JOIN Payments TP ON TP.CUSTOMERID = vmcompany.COMPANYID " +
                       " WHERE vmcompany.HGUID = '" + CompanyGUID + "'";
            return lSql;

        }


        public string GetOrganiser_Companies(string pUserId)
        {

            var lSql = " SELECT BCOMPANY.*,TPAYMENT.PAYMENTNUM, TPAYMENTSB.IAMOUNT, TPAYMENTSB.MEMCOUNT PMEMCOUNT, TPAYMENTSB.PERIODID, MPERIOD.MYEARMONTH " +
            " FROM (SELECT BCOMPANY.*, (SELECT MAX(PAYMENTID) FROM TPAYMENTSB WHERE TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID) PAYMENTID, " +
            " (SELECT COUNT(1) FROM MMEMBER WHERE MMEMBER.BCOMPANYID = BCOMPANY.COMPANYID AND MMEMBER.MEMSTATUSID NOT IN (3,12,18)) MMEMCOUNT " +
            " FROM (SELECT b.COMPANYID, b.CNAME, b.COMPANYNUM, b.EXT_COCODE, b.REG_NUM, b.E_MAIL, b.CONTACT_PERSON, b.TEL_NUMBER, CAST(b.CELL_NUMBER AS VARCHAR(20)) CELL_NUMBER, " +
            " b.WEBADDRESS, b.COMPTYPID, b.STIC, u.NAME ||' '||u.SURNAME PROCESSOR_NAME, e.EMPID, " +
            " u.EMAILADDRESS PROCESSOR_EMAIL, u.CELLNUMBER PROCESSOR_CELLNUMBER, b.FAX_NUMBER, b.WREGCODE, u.AUTHCODE " +
            " FROM CCUR c, MEMPLOYEE e, MCOMPANY b, MUSER u WHERE C.SRCOBJ= 'MUSER' " +
            " AND c.COMPANYID = b.COMPANYID AND b.COMPTYPID <> 4" +
            " AND c.SRCID = u.USERID " +
            " AND e.USERID = u.USERID AND u.AUTHCODE = '" + pUserId + "') BCOMPANY ) BCOMPANY " +
            " LEFT JOIN TPAYMENTSB ON TPAYMENTSB.PAYMENTID = BCOMPANY.PAYMENTID AND TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID" +
            " LEFT JOIN TPAYMENT ON TPAYMENT.PAYMENTID = BCOMPANY.PAYMENTID " +
            " LEFT JOIN MPERIOD ON MPERIOD.PRID = TPAYMENTSB.PERIODID; ";
            return lSql;

        }

        public string GetOrganiser_Branches(string pUserId)
        {

            var lSql = " SELECT BCOMPANY.*,TPAYMENT.PAYMENTNUM, TPAYMENTSB.IAMOUNT, TPAYMENTSB.MEMCOUNT PMEMCOUNT, TPAYMENTSB.PERIODID, MPERIOD.MYEARMONTH " +
            " FROM (SELECT BCOMPANY.*, (SELECT MAX(PAYMENTID) FROM TPAYMENTSB WHERE TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID) PAYMENTID, " +
            " (SELECT COUNT(1) FROM MMEMBER WHERE MMEMBER.BCOMPANYID = BCOMPANY.COMPANYID AND MMEMBER.MEMSTATUSID NOT IN (3,12,18)) MMEMCOUNT " +
            " FROM (SELECT b.COMPANYID, b.CNAME, b.COMPANYNUM, b.EXT_COCODE, b.REG_NUM, b.E_MAIL, b.CONTACT_PERSON, b.TEL_NUMBER, CAST(b.CELL_NUMBER AS VARCHAR(20)) CELL_NUMBER, " +
            " b.WEBADDRESS, b.COMPTYPID, b.STIC, u.NAME ||' '||u.SURNAME PROCESSOR_NAME, e.EMPID, " +
            " u.EMAILADDRESS PROCESSOR_EMAIL, u.CELLNUMBER PROCESSOR_CELLNUMBER, b.FAX_NUMBER, B.wregcode, u.AUTHCODE " +
            " FROM CCUR c, MEMPLOYEE e, MCOMPANY b, MUSER u WHERE C.SRCOBJ= 'MUSER' " +
            " AND c.COMPANYID = b.COMPANYID AND b.COMPTYPID = 4" +
            " AND c.SRCID = u.USERID " +
            " AND e.USERID = u.USERID AND u.AUTHCODE = '" + pUserId + "') BCOMPANY ) BCOMPANY " +
            " LEFT JOIN TPAYMENTSB ON TPAYMENTSB.PAYMENTID = BCOMPANY.PAYMENTID AND TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID" +
            " LEFT JOIN TPAYMENT ON TPAYMENT.PAYMENTID = BCOMPANY.PAYMENTID " +
            " LEFT JOIN MPERIOD ON MPERIOD.PRID = TPAYMENTSB.PERIODID; ";
            return lSql;

        }


        public string GetOrganiser_UnmappedCompanies()
        {
            var lSql = " SELECT b.*, '' SECTORNAME, 0 MEMCOUNT, 0 ACTIVEMEMCOUNT, 0 ALLMEMCOUNT, 0 TRANCOUNT, '' PAYMENTNUM " +
            " FROM MCOMPANY b " +
            " LEFT JOIN CCUR c ON c.COMPANYID = b.COMPANYID " +
            " WHERE c.COMPANYID IS NULL AND b.COMPTYPID <> 4 ";

            return lSql;
        }
        public string GetOrganiser_UnmappedBranches()
        {
            var lSql = " SELECT b.*, '' SECTORNAME, 0 MEMCOUNT, 0 ACTIVEMEMCOUNT, 0 ALLMEMCOUNT  " +
            " FROM MCOMPANY b " +
            " LEFT JOIN CCUR c ON c.COMPANYID = b.COMPANYID " +
            " WHERE c.COMPANYID IS NULL AND b.COMPTYPID = 4 ";

            return lSql;
        }


        public string GetProcessorLoadedSchedules(string pUserId, string pCompanyId, string pYearmonth)
        {

            var lSql = " SELECT GEN_UUID() UUID, TPAYMENTSC.INSBY, MPERIOD.myear, MPERIOD.MYEARMONTH, MPERIOD.MYEARMONTH LOADYEARMONTH, TPAYMENTSC.INSDAT, VMUSER.fullname PROCESSOR_NAME,vmuser.localoffice,TPAYMENTSC.PAYMENTNUM,TPAYMENTSC.debtorname,TPAYMENTSC.trancount,TPAYMENTSC.memcount,TPAYMENTSC.iamount, " +
                " TPAYMENTSC.iadminfee,1 SCHEDULES FROM TPAYMENTSC INNER JOIN MPERIOD ON TPAYMENTSC.PERIODID = MPERIOD.PRID " +
                //" INNER JOIN MPERIOD MP2 ON TPAYMENTSC.INSDAT BETWEEN MP2.SDATE AND MP2.EDATE " +   ,MP2.myearmonth LOADYEARMONTH,
                " INNER JOIN CCUR c ON c.COMPANYID = TPAYMENTSC.CUSTOMERID AND c.SRCOBJ = 'MUSER' AND c.SRCID = " + pUserId +
                " INNER JOIN VMUSER ON c.SRCID = VMUSER.USERID ";

            /*
            if (pCompanyId != null)
            {

                lSql = lSql + " AND TPAYMENTSC.CUSTOMERID = CASE WHEN " + pCompanyId + " = -1 THEN TPAYMENTSC.CUSTOMERID ELSE " + pCompanyId + " END";
            }

            if (pYearmonth != null)
            {
                lSql = lSql + " AND MP2.myearmonth = CASE WHEN '" + pYearmonth + "' = '-1' THEN MP2.MYEARMONTH ELSE '" + pYearmonth + "' END";
            }
            */

            lSql = lSql + " WHERE TPAYMENTSC.DETYPE = 'HD' ";

            return lSql;

        }




        public string GetBlockExecution(string FullFileName)
        {

            try
            {
                // Open the text file using a stream reader.
                using StreamReader reader = new(FullFileName);

                // Read the stream as a string.
                string text = reader.ReadToEnd();

                // Write the text to the console.
                Console.WriteLine(text);

                return text;
            }
            catch (IOException e)
            {
                return "";
            }
        }

        public string PostPaymentConfirmExecution()
        {

            var lSql = "EXECUTE PROCEDURE SP_UPD_TPAYMENTSC({0})";

            return lSql;

        }
        public string Payment_UpdateLatestPayment(int PeriodIdFrom, int PeriodIdTo)
        {

            var lSql = "EXECUTE PROCEDURE sp_upd_lt_paymentdetail(" + PeriodIdFrom.ToString() + "," + PeriodIdTo.ToString() + ")";
            //var lSql = "EXECUTE PROCEDURE sp_upd_lt_paymentdetail({0} ,{1})";

            return lSql;

        }
        public string FinanceReport_GenerateMonthly(int PeriodIdFrom, int PeriodIdTo)
        {

            var lSql = "select * FROM rep_inconstitutionstats(" + PeriodIdFrom.ToString() + "," + PeriodIdTo.ToString() + ") where lt=1";
            //var lSql = "select * FROM rep_inconstitutionstats({@FPERIODID} ,{@TPERIODID}) where lt=1";

            return lSql;

        }

        public string FinanceReport_GenerateMonthlyToTable(int PeriodIdTo)
        {

            var lSql = "EXECUTE PROCEDURE rep_fcreportstats(" + PeriodIdTo.ToString() + ", null)";
            //var lSql = "select * FROM rep_inconstitutionstats({@FPERIODID} ,{@TPERIODID}) where lt=1";

            return lSql;

        }

        public string DatabaseBackup(string DBNAME, string PARAMS)
        {
            var lSql = "gbak -b -v -ignore  -user SYSDBA -password a8ZXx7g2u6 C:\\APPS\\infinIT\\NUM\\Live\\NUM.FDB C:\\APPS\\infinIT\\NUM\\Live\\NUM.1.fbk";
            return lSql;


        }

    }
}
