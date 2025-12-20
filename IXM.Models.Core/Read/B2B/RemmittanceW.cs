using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IXM.Models.Core
{



    [Table("TRMBL")]
    public class TRMBL_WRITE
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
    }



    [Table("TRMBLD")]
    public class TRMBLD_WRITE
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

    }


}

