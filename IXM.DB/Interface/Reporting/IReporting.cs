using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IXM.Models;

namespace IXM.DB
{
    public interface IReporting
    {
        Task Payment_UpdateLatestPayment(int PeriodIdFrom, int PeriodIdTo, string UserName);
        Task<List<REP_FINANCE_MONTHLY>>FinanceReport_GenerateMonthly(int PeriodIdFrom, int PeriodIdTo, string UserName);
        Task<int> FinanceReport_GenerateMonthlyToTable(int PeriodIdTo, string UserName);
        Task<List<REP_FINANCE_MONTHLY>> FinanceReport_FromTable(int PeriodIdTo, string UserName);
        Task<int> ExportReportToFile(System.Data.DataTable DataValues, string PhysicalFileName, string SheetName);
        Task<List<LIST_MONTHLYREPORTS>> ListMonthlyReports();
    }
}
