using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{

     public class MPERIOD
     {
         [Key]
         public int PRID { get; set; }
         public required string MYEAR { get; set; }
         public required string MIMONTH { get; set; }
         public string? MMONTH { get; set; }
         public required string FYEAR { get; set; }
         public required string FMONTH { get; set; }
         public DateOnly? SDATE { get; set; }
         public DateOnly? EDATE { get; set; }
         public int? PSTATUSID { get; set; }
         public required string FDESCRIPTION { get; set; }
         public string? MDESCRIPTION { get; set; }
         public required string MYEARMONTH { get; set; }
         public required string FYEARMONTH { get; set; }
         public int? QT { get; set; }
        public int? ISACTIVE { get; set; }
    }


    public class MP_MONTHS
    {
        [Key]
        public int PRID { get; set; }
        public string? SDATE { get; set; }
        public string? EDATE { get; set; }
        public required string? FDESCRIPTION { get; set; }
        public string? MDESCRIPTION { get; set; }
        public required string? MYEARMONTH { get; set; }
        public required string? FYEARMONTH { get; set; }
    }


    public class MP_YEARS
    {
        [Key]
        public int PRID { get; set; }
        public required string MYEAR { get; set; }
        public required string FYEAR { get; set; }
        public string? SDATE { get; set; }
        public string? EDATE { get; set; }
    }

}
