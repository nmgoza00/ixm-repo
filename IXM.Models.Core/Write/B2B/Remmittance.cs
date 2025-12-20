using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IXM.Models.Core
{

    public static class TRMBLExtension
    {
        public static IQueryable<TRMBL> ReadModel(this IQueryable<TRMBL> source)
        {
            return source.Select(b => new TRMBL
            {
                RMBLID = b.RMBLID,
                RMBLNUM = b.RMBLNUM,
                COMPANYID = b.COMPANYID,
                PERIODID = b.PERIODID,
                RSTYPEID = b.RSTYPEID,
                OBJECTID = b.OBJECTID,
                POPOBJECTID = b.POPOBJECTID,
                EMLOBJECTID = b.EMLOBJECTID,
                PAYMENTID = b.PAYMENTID,
                PAYMENTREF = b.PAYMENTREF,
                ADMINFEE = b.ADMINFEE,
                ADMINFEETYPE = b.ADMINFEETYPE,
                STATUSID = b.STATUSID,
                REMIT_DOCUMENTNAME = b.RemitDocument.DOCUMENTNAME,
                POP_DOCUMENTNAME = b.POPDocument.DOCUMENTNAME,
                CNAME = b.Company.CNAME,
                FYEARMONTH = b.Period.FYEARMONTH,
                PAYMENTNUM = b.Payment.PAYMENTNUM,
                STATUS_NAME = b.Status.DESCRIPTION,
                //STATUS_SEQ = b.Status.STATUS_SEQ,
                IAMOUNT = b.IAMOUNT,
                FLOADS = b.FLOADS,
                MEMBERS = b.MEMBERS,
                INSDAT = b.INSDAT,
                INSBY = b.INSBY,
                MODAT = b.INSDAT,
                MODBY = b.MODBY

            });
        }
    }
    public static class TRMBLDExtension
    {
        public static IQueryable<TRMBLD> ForPayment(this IQueryable<TRMBLD> source)
        {
            return source.Select(b => new TRMBLD
            {
                RMBLID = b.RMBLID,
                RMBLDID = b.RMBLDID,
                DB_MEMBERID = b.DB_MEMBERID,
                DB_MEMSTATUSID = b.DB_MEMSTATUSID,
                DB_COMPANYID = b.DB_COMPANYID,
                DB_BCOMPANYID = b.DB_BCOMPANYID,
                DB_CITYID = b.DB_CITYID,
                DB_PROVINCEID = b.DB_PROVINCEID,
                DB_LOCALITYID = b.DB_LOCALITYID,
                TRL_IAMOUNT = b.TRL_IAMOUNT,
                TRL_EAMOUNT = b.TRL_EAMOUNT,
                DTF_MNAME = b.DTF_MNAME,
                DTF_MSURNAME = b.DTF_MSURNAME,
                DTF_IDNUMBER = b.DTF_IDNUMBER,
                TRL_SALARY = b.TRL_SALARY,
                INSBY = b.INSBY,
                LT = b.LT,
                LTCNT = b.LTCNT

            });
        }

    }
    public class TRMBL
    {
        [Key]
        public int RMBLID { get; set; }
        public string RMBLNUM { get; set; }
        public int COMPANYID { get; set; }
        public int PERIODID { get; set; }
        public int RSTYPEID { get; set; }
        public int? OBJECTID { get; set; }
        public int? POPOBJECTID { get; set; }
        public int? EMLOBJECTID { get; set; }
        public string? PAYMENTREF { get; set; }
        public double IAMOUNT { get; set; }
        public double ADMINFEE { get; set; }
        public string? ADMINFEETYPE { get; set; }
        public int? PAYMENTID { get; set; }
        public int? STATUSID { get; set; }
        public DateTime INSDAT { get; set; }
        public string INSBY { get; set; }
        public DateTime MODAT { get; set; }
        public string MODBY { get; set; }
        public int FLOADS { get; set; }
        public int MEMBERS { get; set; }
        public virtual MCOMPANY Company { get; set; }
        public virtual MPERIOD Period { get; set; }

        [ForeignKey(nameof(OBJECTID))]
        public virtual MOBJECT_DOC? RemitDocument { get; set; }


        [ForeignKey(nameof(POPOBJECTID))]
        public virtual MOBJECT_DOC? POPDocument { get; set; }

        public virtual TPAYMENT? Payment { get; set; }
        public virtual MSTATUS_TEXT? Status { get; set; }

        [NotMapped]
        public string? CNAME { get; set; }
        [NotMapped]
        public string? REMIT_DOCUMENTNAME { get; set; }
        [NotMapped]
        public string? POP_DOCUMENTNAME { get; set; }
        [NotMapped]
        public string? PAYMENTNUM { get; set; }
        [NotMapped]
        public string? STATUS_NAME { get; set; }
        [NotMapped]
        public int? STATUS_SEQ { get; set; }

        [NotMapped]
        public string? FYEARMONTH { get; set; }
    }



    public class TRMBLD
    {
        [Key]
        public int RMBLDID { get; set; }
        public int RMBLID { get; set; }
        public int VERSIONID { get; set; }
        public int MEMEXISTS { get; set; }
        public string? MEMBERID { get; set; }
        public string? DTF_MNAME { get; set; }
        public string? DTF_MSURNAME { get; set; }
        public string? DTF_EMPNUMBER { get; set; }
        public string? DTF_IDTYPE { get; set; }
        public string? DTF_IDNUMBER { get; set; }
        public int? DTF_IDSEQ { get; set; }
        public string? DTF_GENDER { get; set; }
        public string? DTF_CITYID { get; set; }
        public string? DTF_COMPANYID { get; set; }
        public string? DTF_BCOMPANYID { get; set; }
        public string? DTF_RCOMPANYID { get; set; }
        public string? DTF_ECONTRIBUTION { get; set; }
        public string? DTF_ICONTRIBUTION { get; set; }
        public string? DTF_SALARY { get; set; }
        public string? DTF_MEMSTATUSID { get; set; }
        public string? DTF_DOB { get; set; }
        public string? DTF_CELLNUMBER { get; set; }
        public string? DTF_O1ID { get; set; }
        public int? DB_MEMBERID { get; set; }
        public int? DB_MEMSTATUSID { get; set; }
        public string? DB_GENDER { get; set; }
        public string? DB_IDNUMBER { get; set; }
        public string? DB_EMPNUMBER { get; set; }
        public string? DB_MNAME { get; set; }
        public string? DB_MSURNAME { get; set; }
        public string? DB_CELLNUMBER { get; set; }
        public int? DB_COMPANYID { get; set; }
        public int? DB_BCOMPANYID { get; set; }
        public int? DB_RCOMPANYID { get; set; }
        public int? DB_UNIONID { get; set; }
        public string? DB_CITYID { get; set; }
        public string? DB_PROVINCEID { get; set; }
        public int? DB_LOCALITYID { get; set; }
        public string? DB_IDTYPEID { get; set; }
        public double? DB_ECONTRIBUTION { get; set; }
        public double? DB_ICONTRIBUTION { get; set; }
        public int? MATCH_BCOMPANYID { get; set; }
        public int? MATCH_COMPANYID { get; set; }
        public int? MATCH_EMPNUMBER { get; set; }
        public int? MATCH_IDNUMBER { get; set; }
        public int? MATCH_GENDER { get; set; }
        public int? MATCH_NAME { get; set; }
        public int? MATCH_SURNAME { get; set; }
        public int? MATCH_MEMSTATUSID { get; set; }
        public int? MATCH_CELLNUMBER { get; set; }
        public int? MATCH_IDTYPEID { get; set; }
        public string? TRL_IDTYPEID { get; set; }
        public int? TRL_MAGE { get; set; }
        public string? TRL_GENDER { get; set; }
        public int? TRL_UNIONID { get; set; }
        public int? TRL_MSTATUSID { get; set; }
        public string? TRL_CITYID { get; set; }
        public int? TRL_BCOMPANYID { get; set; }
        public int? TRL_RCOMPANYID { get; set; }
        public double? TRL_SALARY { get; set; }
        public double? TRL_EAMOUNT { get; set; }
        public double? TRL_IAMOUNT { get; set; }
        public int? ERRORNUM { get; set; }
        public string? ERRORDESCRIPTION { get; set; }
        public string? MEMICP { get; set; }
        public string? DELETEREC { get; set; }
        public string? ANYPROB { get; set; }
        public string? CIT { get; set; }
        public string? INSBY { get; set; }
        public int? LT { get; set; }
        public int? LTCNT { get; set; }
        public DateTime? INSDAT { get; set; }
        public string? MODBY { get; set; }
        public DateTime? MODAT { get; set; }

        [NotMapped]
        public int? Loaded { get; set; }
        [NotMapped]
        public int? WithIssues { get; set; }

    }

    public class TRMBLE
    {
        [Key]
        public int RMBLEID { get; set; }
        public int RMBLID { get; set; }
        public int VERSIONID { get; set; }
        public int OBJECTID { get; set; }
        public int? STATUSID { get; set; }
        public int ERRNUM { get; set; }
        public int ERRCOUNT { get; set; }
        public string? ERRNOTE { get; set; }
        public DateTime? INSDAT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public int LT { get; set; }
    }


    public class TRMBL_POST
    {
        [Key]
        public int RMBLID { get; set; }
        public string RMBLNUM { get; set; }
        public int COMPANYID { get; set; }
        public int PERIODID { get; set; }
        public int? RSTYPEID { get; set; }
        public int? OBJECTID { get; set; }
        public int? POPOBJECTID { get; set; }
        public int? EMLOBJECTID { get; set; }
        public string? PAYMENTREF { get; set; }
        public double IAMOUNT { get; set; }
        public double ADMINFEE { get; set; }
        public string? ADMINFEETYPE { get; set; }
        public int? PAYMENTID { get; set; }
        public int? STATUSID { get; set; }
        public DateTime INSDAT { get; set; }
        public string INSBY { get; set; }
        public DateTime MODAT { get; set; }
        public string MODBY { get; set; }
        public int FLOADS { get; set; }
        public int MEMBERS { get; set; }
    }



    public class Remmittance_POST
    {
        [Key]
        public int PostType { get; set; }
        public TRMBL_POST Remmittance { get; set; }
        public List<TRMBLD>? RemmittanceDetail { get; set; }
    }
        public record Remittance
    (

        int RMBLID,
        int PAGENO,
        int PAGESIZE

    );


    public class RemittanceCreateEvent
    {
        public int COMPANYID { get; set; }
        public int RMBLID { get; set; }
        public int PERIODID { get; set; }
        public int OBJECTID { get; set; }
        public string SYSTEMID { get; set; }
        public string USERNAME { get; set; }
        public string FILENAME { get; set; }
        public string DOCFOLDER { get; set; }

    }

    public class PaymentCreateEvent
    {
        public int COMPANYID { get; set; }
        public int PAYMENTID { get; set; }
        public int PERIODID { get; set; }
        public int SOURCEID { get; set; }
        public int OBJECTID { get; set; }
        public int SYSTEMID { get; set; }
        public string USERNAME { get; set; }

    }

    public class MessageQTest
    {
        public int COMPANYID { get; set; }

    }
}

