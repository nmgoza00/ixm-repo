using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace IXM.Models.Write
{


    public class MSECTORWr
    {
        [Key]
        public int SECTORID { get; set; }
        public int SYSTEMID { get; set; }
        [StringLength(4)]
        public string? SCODE { get; set; }
        public int? HSECTORID { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? SHORT_DESCRIPTION { get; set; }
        public int? COUNCILID { get; set; }
        public int? CORDINATORID { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public string? ISACTIVE { get; set; }

    }


    public class MUNIONWr
    {
        [Key]
        public int UNIONID { get; set; }
        public int? SYSTEMID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public string? SHORTDESC { get; set; }
        public int? CINDSID { get; set; }

    }

    public class MUSERWr
    {
        [Key]
        public int USERID { get; set; }
        public string UNAME { get; set; }
        public string? UPASSWORD { get; set; }
        public string? NAME { get; set; }
        public string? SURNAME { get; set; }
        public string? TELNUMBER { get; set; }
        public string? CELLNUMBER { get; set; }
        public int? LOCALITYID { get; set; }
        public int? IPOSITIONID { get; set; }
        public int? UGID { get; set; }
        public int? MEMBERID { get; set; }
        public int? EMPID { get; set; }
        public int? MSTATUSID { get; set; }
        [EmailAddress]
        public string? EMAILADDRESS { get; set; }
        public DateOnly? EXPDAT { get; set; }
        public string? AUTHCODE { get; set; }
        public string? APPSERVICE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MCNAME { get; set; }
        [StringLength(1)]
        public string? ISACTIVE { get; set; }

    }

    public class MLOCALITYWr
    {
        [Key]
        public int LOCALITYID { get; set; }
        public int? SYSTEMID { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? STREETNO { get; set; }
        public string? STREETNAME { get; set; }
        public string? BUILDINGNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? POSTALCODE { get; set; }
        [StringLength(10)]
        public string? CITYID { get; set; }
        [StringLength(1)]
        public string? ISLOCAL { get; set; }
        public string? PROVINCEID { get; set; }
        public string? GEOLATI { get; set; }
        public string? GEOLONG { get; set; }
        public string? TELNUMBER { get; set; }
        public string? FAXNUMBER { get; set; }
        public int? POBOXNO { get; set; }
        public string? POBOXNAME { get; set; }
        [StringLength(10)]
        public string? POCITYID { get; set; }
        public string? POPROVINCEID { get; set; }
        public string? POPOSTALCODE { get; set; }
        public string? ADDRESSNOTE { get; set; }
        [StringLength(2)]
        public string? LOCTYPE { get; set; }
        public string? PLOCALITYID { get; set; }
        [StringLength(4)]
        public string? LSC { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }

    }

    public class MPERIODWr
    {
        [Key]
        public int PRID { get; set; }
        public int? SYSTEMID { get; set; }
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

    public class MCASETYPEWr
    {
        [Key]
        public int CASETYPEID { get; set; }
        public int? SYSTEMID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }

    }


    public class MINTPOSITIONWr
    {
        [Key]
        public int IPOSITIONID { get; set; }
        public int? SYSTEMID { get; set; }
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
}
