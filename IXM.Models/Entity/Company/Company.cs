using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{

    public static class MCOMPANYExtension
    {
        public static IQueryable<MCOMPANY> ForBasicData(this IQueryable<MCOMPANY> source)
        {
            return source.Select(b => new MCOMPANY
            {
                COMPANYID = b.COMPANYID,
                HGUID = b.HGUID,
                COMPANYNUM = b.COMPANYNUM,
                CNAME = b.CNAME
            });
        }
        public static IQueryable<MCOMPANY> ForRemitInfo(this IQueryable<MCOMPANY> source)
        {
            return source.Select(b => new MCOMPANY
            {
                COMPANYID = b.COMPANYID,
                COMPTYPID = b.COMPTYPID,
                MSTATUSID = b.MSTATUSID,
                HGUID = b.HGUID,
                COMPANYNUM = b.COMPANYNUM,                
                CNAME = b.CNAME,
                STREETNO = b.STREETNO,
                STREETNAME = b.STREETNAME,
                CONTACT_PERSON = b.CONTACT_PERSON,
                E_MAIL = b.E_MAIL,
                FAX_NUMBER = b.FAX_NUMBER,
                REG_NUM = b.REG_NUM,
                BCITYID = b.BCITYID,
                BUILDINGNAME = b.BUILDINGNAME,
                POSTALCODE = b.POSTALCODE,
                TEL_NUMBER = b.TEL_NUMBER,
            });
        }

        public static IQueryable<USER_COMPANY> ReadUserCompanies(this IQueryable<USER_COMPANY> source)
        {
            return source.Select(b => new USER_COMPANY
            {
                AUTHCODE = b.AUTHCODE,
                COMPANYID = b.COMPANYID,
                COMPANYNUM = b.COMPANYNUM,
                HGUID = b.HGUID,
                CNAME = b.CNAME,
                PROCESSOR_CELLNUMBER = b.PROCESSOR_CELLNUMBER,
                PROCESSOR_NAME = b.PROCESSOR_NAME,
                PROCESSOR_EMAIL = b.PROCESSOR_EMAIL,
                MMEMCOUNT = b.MMEMCOUNT,
                ACTIVEMEMCOUNT = b.ACTIVEMEMCOUNT,
                ALLMEMCOUNT = b.ALLMEMCOUNT

            });
        }

    }
    public class MCOMPANY
    {
        [Key]
        public int COMPANYID { get; set; }
        public string HGUID { get; set; }

        [Required(ErrorMessage = "The Company name is required.")]
        [StringLength(100)]
        public string? CNAME { get; set; }
        //[Required]
        [Required(ErrorMessage = "The Company number is required.")]
        public string? COMPANYNUM { get; set; }
        public int? RCOMPANYID { get; set; }
        public string? EXT_COCODE { get; set; }
        public string? OLD_CCODE { get; set; }
        public int? SECTORID { get; set; }
        public string? REG_NUM { get; set; }
        public string? FAX_NUMBER { get; set; }
        public string? TEL_NUMBER { get; set; }
        public string? CELL_NUMBER { get; set; }
        public string? CONTACT_PERSON { get; set; }
        public string? E_MAIL { get; set; }
        public string? WEBADDRESS { get; set; }
        public int? STREETNO { get; set; }
        public string? STREETNAME { get; set; }
        public string? MAREA { get; set; }
        public int? BUILDINGNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? POSTALCODE { get; set; }
        public string? POBOXNAME { get; set; }
        public string? POBOXPREFIX { get; set; }
        public int? POBOXNO { get; set; }
        public string? POPOSTALCODE { get; set; }
        public string? BCITYID { get; set; }
        public string? BPROVINCEID { get; set; }
        public string? COMPANYINFO { get; set; }
        public string? TO_INVOICE { get; set; }
        public int? COMDTYID { get; set; }
        public string? SECTORNAME { get; set; }
        public int? MEMCOUNT { get; set; }
        public int? HMEMCNT { get; set; }
        public int? HMEMAVG { get; set; }
        public int? ACTIVEMEMCOUNT { get; set; }
        public int? ALLMEMCOUNT { get; set; }
        public int? DBFACCTID { get; set; }
        public int? CRFACCTID { get; set; }
        public int? MSTATUSID { get; set; }
        public string? WREGCODE { get; set; }
        public int? COMPTYPID { get; set; }
        public int? BANK_REFNR { get; set; }
        public string? PAYMENTNUM { get; set; }
        public int? TRANCOUNT { get; set; }

        [ForeignKey(nameof(COMPTYPID))]
        public virtual MCOMPTYPE? CompanyType { get; set; }
        [ForeignKey(nameof(SECTORID))]
        public virtual MSECTOR? Sector { get; set; }

        [ForeignKey(nameof(BCITYID))]
        public virtual MCITY? Location { get; set; }

        [ForeignKey(nameof(COMPANYID))]
        public virtual ICollection<MOBJECT_NOT>? CompanyNotes { get; set; }

        [NotMapped]
        public string? STIC { get; set; }
        [NotMapped]
        public int? PERIODID { get; set; }
    }

    public enum CompanyType { Company, Branch, JointVenture }


    public class MCOMPTYPE
    {
        [Key]
        public int COMPTYPID { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string DCODE { get; set; }
        public int? LEVEL_N { get; set; }
        public int? ALVL { get; set; }
        public int? ADLVL { get; set; }

    }


    public class USER_COMPANY
    {
        [Key]
        public int COMPANYID { get; set; }
        public string HGUID { get; set; }
        public string? AUTHCODE { get; set; }
        public string CNAME { get; set; }
        public string? COMPANYNUM { get; set; }
        public string? EXT_COCODE { get; set; }
        public string? REG_NUM { get; set; }
        public string? E_MAIL { get; set; }
        public string? CONTACT_PERSON { get; set; }
        public string? TEL_NUMBER { get; set; }
        public string? CELL_NUMBER { get; set; }
        public string? WEBADDRESS { get; set; }
        public int? PMEMCOUNT { get; set; }
        public int? MMEMCOUNT { get; set; }
        public int? ACTIVEMEMCOUNT { get; set; }
        public int? ALLMEMCOUNT { get; set; }
        public int? PERIODID { get; set; }
        public int? EMPID { get; set; }
        public string? MYEARMONTH { get; set; }
        public string? PROCESSOR_NAME { get; set; }
        public string? PROCESSOR_CELLNUMBER { get; set; }
        public string? PROCESSOR_EMAIL { get; set; }
        public int? COMPTYPID { get; set; }
        public string? STIC { get; set; }
        [NotMapped]
        public string ScheduleCompliant
        {
            get { if (String.IsNullOrEmpty(STIC)) { return "na"; } return STIC.ToLower(); }
        }


    }
    public class ORGANISER_COMPANY
    {
        [Key]
        public int COMPANYID { get; set; }
        public string? AUTHCODE { get; set; }
        public string CNAME { get; set; }
        public string? COMPANYNUM { get; set; }
        public string? EXT_COCODE { get; set; }
        public string? REG_NUM { get; set; }
        public string? E_MAIL { get; set; }
        public string? CONTACT_PERSON { get; set; }
        public string? TEL_NUMBER { get; set; }
        public string? CELL_NUMBER { get; set; }
        public string? FAX_NUMBER { get; set; }
        public string? WEBADDRESS { get; set; }
        public int? PAYMENTID { get; set; }
        public string? PAYMENTNUM { get; set; }
        public string? IAMOUNT { get; set; }
        public int? PMEMCOUNT { get; set; }
        public int? MMEMCOUNT { get; set; }
        public int? PERIODID { get; set; }
        public int? EMPID { get; set; }
        public string? MYEARMONTH { get; set; }
        public string? PROCESSOR_NAME { get; set; }
        public string? PROCESSOR_CELLNUMBER { get; set; }
        public string? PROCESSOR_EMAIL { get; set; }
        public int? COMPTYPID { get; set; }
        public string? STIC { get; set; }
        public string? WREGCODE { get; set; }
        [NotMapped]
        public string ScheduleCompliant
        {
            get { if (String.IsNullOrEmpty(STIC)) { return "na"; } return STIC.ToLower(); }
        }


    }

}
