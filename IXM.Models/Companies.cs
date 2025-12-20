using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IXM.Models
{



    public static class MSTATUSExtension
    {


        public static IQueryable<MSTATUS> StatusId(this IQueryable<MSTATUS> source)
        {
            return source.Select(b => new MSTATUS
            {
                STATUSID = b.STATUSID,
                ISACTIVE = b.ISACTIVE


            });
        }
    }

    public class SEQUENCE
    {
        [Key]
        public int SEQVALUE { get; set; }

    }
    public class TCAPTIONS
    {
        [Key]
        public int OSID { get; set; }
        public string? OBJECTNAME { get; set; }
        public string? FIELD_NAME { get; set; }
        public string? ALT_FIELD_NAME { get; set; }
        public string? FIELD_CAPTION { get; set; }
        public string? DISPLAY_V { get; set; }
        public int? FIELD_WIDTH { get; set; }

    }

    public class MMTR
    {
        [Key]
        public string MMTRID { get; set; }
        public string MMTRNM { get; set; }
        public string? STATUSID { get; set; }
        public string? TREA { get; set; }
        public DateTime TRDAT { get; set; }
    }



    public class CCUR
    {
        public int COMPANYID { get; set; }
        public int USERID { get; set; }
        public string? SRCOBJ { get; set; }
        public int SRCID { get; set; }
        public int MIDX { get; set; }
        public DateTime? INSDAT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public string? ISACTIVE { get; set; }
    }


    public class MCOMPANY_CCUR
    {
        [Key]
        public int COMPANYID { get; set; }
        public string HGUID { get; set; }
        public string CNAME { get; set; }
        //[Required]
        public string? COMPANYNUM { get; set; }
        public int RCOMPANYID { get; set; }
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
        public string? BCITYID { get; set; }
        public string? BPROVINCEID { get; set; }
        public string? COMPANYINFO { get; set; }
        public string? TO_INVOICE { get; set; }
        public int? COMDTYID { get; set; }
        public string? SECTORNAME { get; set; }
        public int? MEMCOUNT { get; set; }
        public string? FYEARMONTH { get; set; }
        public string? PROCESSOR_NAME { get; set; }
        public string? PROCESSOR_CELLNUMBER { get; set; }
        public string? PROCESSOR_EMAIL { get; set; }

        public int COMPTYPID { get; set; }

    }



    public class MOBJECT_NOT
    {
        [Key]
        public int NOTEID { get; set; }
        public int? PNOTEID { get; set; }
        public int? NIDX { get; set; }
        public string? NAME { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEFLD { get; set; }
        public int? SOURCEID { get; set; }
        public int? PERIODID { get; set; }
        public string? NOTES { get; set; }
        public string? RSCODE { get; set; }
        public string? NOTETYPE { get; set; }
        public string? CELLNUMBER { get; set; }
        public string? INSBY { get; set; }
        public DateTime? INSDAT { get; set; }

    }


    public class MEMPLOYEE_DISPLAY
    {
        [Key]
        public int EMPID { get; set; }
        public int? EMPTYPID { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? IDNUMBER { get; set; }
        public string? FULLNAME { get; set; }
        public string? CELLNUMBER { get; set; }
        public string? EMAIL { get; set; }
        public int? MEMBERID { get; set; }
        public string? ID_USERID { get; set; }
        public int? USERID { get; set; }
        public string? HGUID { get; set; }

    }


    public class MCODE
    {

        [Key]
        public int CODEID { get; set; }
        public int? CODETYPEID { get; set; }
        public string? CODE_TEXT { get; set; }
        public int? CODE_LEN { get; set; }
        public string? CODE_PREFIX { get; set; }
        public string? CODE_VALUE { get; set; }
        public string? CODE_FPATH { get; set; }
        public string? CODE_CA { get; set; }

    }




    public class MSTATUS
    {
        [Key]
        public int MSTATUSID { get; set; }
        public required int STATUSID { get; set; }
        public int? STATUS_SEQ { get; set; }
        public string? STATUS_TYPE { get; set; }
        public string? SHORT_CODE { get; set; }
        public int? XP { get; set; }
        public int? WFID { get; set; }
        public string? CA { get; set; }
        public int? DAM { get; set; }
        public int? IDAM { get; set; }
        public int? KSH { get; set; }
        public string? SHF { get; set; }
        public int? SCQ { get; set; }
        public string? SCM { get; set; }
        public int? SGRP { get; set; }
        public int? TABLEID { get; set; }
        public required string? ISACTIVE { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

    }
    public class VMSTATUS
    {
        [Key]
        public int MSTATUSID { get; set; }
        public required int STATUSID { get; set; }
        public int? STATUS_SEQ { get; set; }
        public string? STATUS_TYPE { get; set; }
        public string? SHORT_CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? XP { get; set; }
        public int? WFID { get; set; }
        public string? CA { get; set; }
        public int? DAM { get; set; }
        public int? IDAM { get; set; }
        public int? KSH { get; set; }
        public string? SHF { get; set; }
        public int? SCQ { get; set; }
        public string? SCM { get; set; }
        public int? SGRP { get; set; }
        public int? TABLEID { get; set; }
        public required string? ISACTIVE { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

    }

    public class MSTATUS_TEXT
    {
        [Key]
        public int STATUSID { get; set; }
        public required string DESCRIPTION { get; set; }
        public string? ISACTIVE { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

    }
    public class MEMPTYPE
    {
        [Key]
        public int EMPTYPID { get; set; }
        public required string DESCRIPTION { get; set; }
        public string? INSDAT { get; set; }
        public string? INSBY { get; set; }
        public string? ISACTIVE { get; set; }
        public int? EGROUP { get; set; }

    }

    public class MCITY
    {
        [Key]
        public required string CITYID { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string PROVINCEID { get; set; }
        public string? PCITYID { get; set; }
        public string? CITYPE { get; set; }
        public string? POSTCODE { get; set; }
        public string? NOTES { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public string? ISACTIVE { get; set; }
    }

    public class MPROVINCE
    {
        [Key]
        public required string PROVINCEID { get; set; }
        public required string DESCRIPTION { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public string? SVG_MAP { get; set; }
        public string? COUNTRYID { get; set; }
    }

    public class MBANKNAME
    {
        [Key]
        public int BANKNAMEID { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string SHORT_CODE { get; set; }
        public required string UNIVERSALCODE { get; set; }
        public required string INSERT_DATE { get; set; }
        public required string INSERTED_BY { get; set; }
        public required string MODIFIED_DATE { get; set; }
        public required string MODIFIED_BY { get; set; }
    }

    public class DATA_VALUEMAPPING
    {
        public required string DHCODE { get; set; }
        public required string SVALUE { get; set; }
        public required string TVALUE { get; set; }
        public required string TKEY { get; set; }
        public required string MODIFY_DATE { get; set; }
        public required string MODIFIED_BY { get; set; }
    }

    public class MUNION
    {
        [Key]
        public int UNIONID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public string? SHORTDESC { get; set; }
        public int? CINDSID { get; set; }
        [NotMapped]
        public int? SYSTEMID { get; set; }

    }
    public class MINTPOSITION
    {
        [Key]
        public int IPOSITIONID { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? SHRT_DESCRIPTION { get; set; }
        public int? IRANKING { get; set; }
        public int? PAIDC { get; set; }
        public int? LINTPOSITION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }

    }
    public class MEXTPOSITION
    {
        [Key]
        public int EPOSITIONID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public string? ISACTIVE { get; set; }

    }
    public class MLANGUAGE
    {
        [Key]
        public int LANGUAGEID { get; set; }
        public string? DESCRIPTION { get; set; }

    }
    public class MUNION_SYSTEM
    {
        [Key]
        public int UNIONID { get; set; }
        public int SYSTEMID { get; set; }
    }

        public class MLOCALITY
    {
        [Key]
        public int LOCALITYID { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? STREETNO { get; set; }
        public string? STREETNAME { get; set; }
        public string? BUILDINGNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? POSTALCODE { get; set; }
        public string? CITYID { get; set; }
        public string? ISLOCAL { get; set; }
        public string? PROVINCEID { get; set; }
        public string? GEOLATI { get; set; }
        public string? GEOLONG { get; set; }
        public string? TELNUMBER { get; set; }
        public string? FAXNUMBER { get; set; }
        public int? POBOXNO { get; set; }
        public string? POBOXNAME { get; set; }
        public string? POCITYID { get; set; }
        public string? POPROVINCEID { get; set; }
        public string? POPOSTALCODE { get; set; }
        public string? ADDRESSNOTE { get; set; }
        public string? LOCTYPE { get; set; }
        public string? PLOCALITYID { get; set; }
        public string? LSC { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }

    }
    public class CLOCALITY
    {
        [Key]
        public string CITYID { get; set; }
        public int? LOCALITYID { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? ISACTIVE { get; set; }
    }


        public class MOBJECT_DOCFILE
    {
        [Key]
        public int OBJECTID { get; set; }
        public string? DOCUMENTNAME { get; set; }
        public string? SFOLDERNAME { get; set; }
        public string? IMAGEURI { get; set; }
        public string DOCTYPE { get; set; }
        public string CODE_FPATH { get; set; }
        public int? LT { get; set; }
    }



    public class APP_HARDCODES
    {
        [Key]
        public string HARDC_TYPE { get; set; }
        public string HARDCODEID { get; set; }
        public string HVALUE { get; set; }
    }



    public class TMEMPAYMENT
    {
        [Key]
        public int PAYMENTID { get; set; }
        public int PERIODID { get; set; }
        public int COMPANYID { get; set; }
        public int MEMBERID { get; set; }
        public string? PAYMENTNUM { get; set; }
        public string MNAME { get; set; }
        public string MSURNAME { get; set; }
        public string? COMPANYNUM { get; set; }
        public double? PAMOUNT { get; set; }
        public string? PYEARMONTH { get; set; }

    }

    public class MMEMBER_READ
    {
        [Key]
        public int MEMBERID { get; set; }
        public string? MEMBERNUM { get; set; }
        public int? MEMBERTYPE { get; set; }
        public string? KNOWNAS { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? IDTYPEID { get; set; }
        public string? IDNUMBER { get; set; }
        public string? CITYID { get; set; }
        public string? EMPNUMBER { get; set; }
        public string? EXTNUMBER { get; set; }
        public string? INITIALS { get; set; }
        public int? ETHNICID { get; set; }
        public int? COMPANYID { get; set; }
        public string? GENDER { get; set; }
        public string? DOB { get; set; }
        public int? MAGE { get; set; }
        public string? HTEL_NO { get; set; }
        public double? SALARY { get; set; }
        public int? YOS { get; set; }
        public string? CELLNUMBER { get; set; }
        public int? LOCALITYID { get; set; }
        public int? SECTORID { get; set; }
        public DateTime? EMPLOYDATE { get; set; }
        public DateTime? JOINDATE { get; set; }
        public double? ANNUAL_SALARY { get; set; }
        public int? POSITIONID { get; set; }
        public int? UNIONID { get; set; }
        public int? LANGUAGEID { get; set; }
        public int? QFCTID { get; set; }
        public string? TITLE { get; set; }
        public string? MARSTAT { get; set; }
        public string? EMAIL { get; set; }
        public string? MMTRID { get; set; }
        public int? BCOMPANYID { get; set; }
        public int? MEMSTATUSID { get; set; }   // Member Status ID
        public int? PROSTATUSID { get; set; }   // Process Status ID
        public int? LGLSTATUSID { get; set; }   // Legal Status ID
        public int? RECRUITERID { get; set; }   // Legal Status ID     
        public int? RMBLDID { get; set; }   // Remittance ID  
        public double? ECONTRIBUTION { get; set; }
        public double? ICONTRIBUTION { get; set; }
        public double? DBCONTRIBUTION { get; set; }
        public string? FILENAME { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MemCardReceived { get; set; }

    }

    //Used for Create Operations
    public class MMEMBER_C
    {
        [Key]
        public int MEMBERID { get; set; }
        public string? MEMBERNUM { get; set; }
        public string? KNOWNAS { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? IDTYPEID { get; set; }
        public string? IDNUMBER { get; set; }
        public string? CITYID { get; set; }
        public string? EMPNUMBER { get; set; }
        public string? EXTNUMBER { get; set; }
        public string? INITIALS { get; set; }
        public int? ETHNICID { get; set; }
        public int? COMPANYID { get; set; }
        public string? GENDER { get; set; }
        public string? DOB { get; set; }
        public string? HTEL_NO { get; set; }
        public double? SALARY { get; set; }
        public int? YOS { get; set; }
        public string? CELLNUMBER { get; set; }
        public int? LOCALITYID { get; set; }
        public int? SECTORID { get; set; }
        public string? EMPLOYDATE { get; set; }
        public string? JOINDATE { get; set; }
        public string? ANNUAL_SALARY { get; set; }
        public int? POSITIONID { get; set; }
        public int? UNIONID { get; set; }
        public string? TITLE { get; set; }
        public string? MARSTAT { get; set; }
        public string? EMAIL { get; set; }
        public int? BCOMPANYID { get; set; }
        public double? DBCONTRIBUTION { get; set; }

    }

    public class MMEMBER_NONPAYMENT
    {
        [Key]
        public int MEMBERID { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? PYEARMONTH { get; set; }
        public string? MemCardReceived { get; set; }

    }


    public class MOBJECT_DOC_READ
    {
        [Key]
        public int OBJECTID { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEFLD { get; set; }
        public int SOURCEID { get; set; }
        public required string DOCTYPE { get; set; }
        public string? DOCUMENTNAME { get; set; }
        public string? LDOCUMENTNAME { get; set; }
        public string? LFOLDERNAME { get; set; }
        public string? LMACHINENAME { get; set; }
        public string? SFOLDERNAME { get; set; }
        public double? FILESIZE { get; set; }
        public string? FILEXTENSION { get; set; }
        public int? LT { get; set; }
        public DateTime? EFFDATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        [NotMapped]
        public string? DOCFILEURI { get; set; }
        [NotMapped]
        public string? IMAGEURI { get; set; }
        [NotMapped]
        public string? CODE_TEXT { get; set; }
        [NotMapped]
        public string? CODE_FPATH { get; set; }

    }

    public class Companies
    {
    }
}
