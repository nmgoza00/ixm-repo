

namespace IXM.GeneralSQL
{
    public interface IGeneralSQL
    {

        string GetPeriodByValue(string Period);
        string GetPeriodPayment(string CompanyId, string PeriodId);
        string GetNotInRemittanceMembers(string RMBLID);
        public string GetRemittanceError(string COMPANYID);

        string GetDocumentByObjectId(string FileId);
        string GetMemberPayments(string MEMBERID);

        string GetPayment(string UserGuid);
        string GetPaymentConfirmation(string UserGuid);

        string GetRemittanceForPaymentDetail(string RMBLID);
        string StatusTriggerCreate(string TBNAME, string CHKFIELD, string KEYFIELD);
        string StatusTriggerDrop(string TBNAME, string KEYFIELD);

        string GetSystemUserGroup(string UserGuId);
        string GetUserLinkedCompanies(Guid UserGuid);

        public string GetBlockExecution(string FullFileName);
        public string CheckIfRemittanceFullyProcessed(string RMBLID);
        string GetCompanyMembers(string CompanyGUID);
        string GetCompanyPayments(string ComapnyGUID);

        string GetCompaniesList(string pUName);
        string GetCompanyById(string ComapnyGUID);
        string GetOrganiser_Companies(string pUName);
        string GetOrganiser_UnmappedCompanies();
        string GetOrganiser_Branches(string pUName);
        string GetOrganiser_UnmappedBranches();

        public string GetProcessorLoadedSchedules(string pUserId, string pCompanyId, string pYearmonth);
        public string PostPaymentConfirmExecution();
        public string Payment_UpdateLatestPayment(int PeriodIdFrom, int PeriodIdTo);
        public string FinanceReport_GenerateMonthly(int PeriodIdFrom, int PeriodIdTo);
        public string FinanceReport_GenerateMonthlyToTable(int PeriodIdTo);
        
        public string DatabaseBackup(string DBNAME, string PARAMS);
    }
}
