using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IXM.Models
{


    public class REP_PROCESSOR_LOADEDSCHEDULES
    {

        [Key]
        public string UUID { get; set; }
        public string INSBY { get; set; }
        public DateTime INSDAT { get; set; }
        public int MYEAR { get; set; }
        public string? LOADYEARMONTH { get; set; }
        public string? MYEARMONTH { get; set; }
        public string? PAYMENTNUM { get; set; }
        public string DEBTORNAME { get; set; }
        public string PROCESSOR_NAME { get; set; }
        public string LOCALOFFICE { get; set; }
        public int TRANCOUNT { get; set; }
        public int SCHEDULES { get; set; }
        public int? MEMCOUNT { get; set; }        
        public decimal? IAMOUNT { get; set; }
        public string? IADMINFEE { get; set; }

        public DateOnly InsertDate 
        {
            get { return DateOnly.FromDateTime(INSDAT.Date); }
        }

    }

    public class REP_DEPT_LOADEDSCHEDULES
    {

        [Key]
        public string UUID { get; set; }
        public string INSBY { get; set; }
        public string FULLNAME { get; set; }
        public string LOCALOFFICE { get; set; }
        public string MYEAR { get; set; }
        public string? LOADYEARMONTH { get; set; }
        public string? MYEARMONTH { get; set; }
        public int TRANCOUNT { get; set; }
        public int? MEMCOUNT { get; set; }
        public string? IAMOUNT { get; set; }

    }
    public class REP_FINANCE_MONTHLY
    {

        [Key]
        public string? UUID { get; set; }
        public int? COMPANYID { get; set; }
        public int? BCOMPANYID { get; set; }
        public int? LOCALITYID { get; set; }
        public string? CITYID { get; set; }
        public int? PAYMENTID { get; set; }
        public int? PERIODID { get; set; }
        public int? MEMBERID { get; set; }
        public int? NOFPAYMENTS { get; set; }
        public int? NOOFENTRIES { get; set; }
        public string? COMPANYNAME { get; set; }
        public string? COMPANYNUM { get; set; }
        public string? BCOMPANYNAME { get; set; }
        public string? SECTORNAME { get; set; }
        public string? REGIONAME { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? GENDER { get; set; }
        public int? MAGE { get; set; }
        public string? AGEGROUPING { get; set; }
        public string? IDNUMBER { get; set; }
        public string? EMPNUMBER { get; set; }
        public string? MYEARMONTH { get; set; }
        public double? IAMOUNT { get; set; }
        public double? LTAMOUNT { get; set; }
        public int? LT { get; set; }
        public string? IDVALID { get; set; }
        public int? CALCPERIODID { get; set; }
        public string? CALCPERIOD { get; set; }
        public DateTime? RUNDATE { get; set; }

    }

    public class MREPORT
    {

        [Key]
        public int REPORTID { get;  set; }
        public string? DESCRIPTION { get; set; }
        public string? TECHNICALNAME { get; set; }
        public string? FILENAME { get; set; }
        public string? OBJECTYPE { get; set; }
        public string? NOTES { get; set; }
        public string? DTL { get; set; }
        public string? PUI { get; set; }
        public string? BISF { get; set; }
        public int? CRT_PERIOD { get; set; }
        public int? CRT_LOCALITY { get; set; }

    }
    public class LIST_MONTHLYREPORTS
    {

        [Key]
        public string GUID { get; set; }
        public string? SYSTEMNAME { get; set; }
        public string REPORTNAME { get; set; }
        public string? PERIOD { get; set; }
        public int? PERIODID { get; set; }
        public string? RUNDATE { get; set; }
        public string? FILENAME { get; set; }
        public string? RESULTTYPE { get; set; }
        public int? MEMBERS { get; set; }
        public int? COMPANIES { get; set; }
        public int? GENDER_MALE { get; set; }
        public int? GENDER_FEMALE { get; set; }
        public int? GENDER_UNKNOWN { get; set; }

    }

}