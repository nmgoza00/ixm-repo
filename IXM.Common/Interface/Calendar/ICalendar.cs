using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IXM.Models;

namespace IXM.Common.Interfaces
{
    public interface ICalendar
    {
        Task<IEnumerable<MPERIOD>> GetPeriodsAll();
        Task<IEnumerable<MPERIOD>> GetPeriodsFrom(int? PeriodId);
        //IEnumerable<MP_MONTHS> GetMonthsAll();
        //IEnumerable<MP_YEARS> GetYearsAll();

    }

}
