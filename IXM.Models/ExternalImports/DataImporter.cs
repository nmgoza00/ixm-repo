using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;

namespace IXM.Models
{


     public class XLS_REMITTANCE
     {
        public string? DTF_MNAME { get; set; }
        public string? DTF_MSURNAME { get; set; }
	    public string? DTF_EMPNUMBER { get; set; }
        public string? DTF_IDTYPE { get; set; }
        public string? DTF_IDNUMBER { get; set; }
        public string? DTF_CITYID { get; set; }
        public string? DTF_BCOMPANYID { get; set; }
        public string? DTF_CCOMPANYID { get; set; }
        public string? DTF_RCOMPANYID { get; set; }
        public string? DTF_ECONTRIBUTION { get; set; }
        public string? DTF_ICONTRIBUTION { get; set; }
	    public string? DTF_SALARY { get; set; }
	    public string? DTF_MEMSTATUSID { get; set; }
        public string? DTF_GENDER { get; set; }
        public string? DTF_DOB { get; set; }
        public string? DTF_CELLNUMBER { get; set; }
        public string? DTF_O1ID {get; set;}
        public string? DTF_ADMINFEE { get; set; }
        public string? DTF_DBCOMPANYID { get; set; }
        public double? TRL_EAMOUNT { get; set; }
        public double? TRL_IAMOUNT { get; set; }
        public double? TRL_SALARY { get; set; }
        public string? TRL_GENDER { get; set; }
        public string? TRL_IDTYPEID { get; set; }
        public string? TRL_CITYID { get; set; }
        public int? TRL_BCOMPANYID { get; set; }
        public DateTime? INSDAT { get; set; }
        public string? INSBY { get; set; }
	    public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public int? ERRORNUM { get; set; }
        public string? ERRORDESCRIPTION { get; set; }

        [NotMapped]
        public int? RMBLID { get; set; }
        [NotMapped]
        public int? COMPANYID { get; set; }
        [NotMapped]
        public int ERROR_OBJECTID { get; set; }

    }


    public class FileImport
    {

        public string DOCTYPE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? FILEPATH { get; set; }
        public string? PERIOD { get; set; }
        public string? FILESIZE { get; set; }
        public string? LDOCUMENTNAME { get; set; }
        public string? SDOCUMENTNAME { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }

    }



}
