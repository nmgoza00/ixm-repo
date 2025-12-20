using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using IXM.Constants;

namespace IXM.Models.Core
{

    public class MOBJECT_DOC_API
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

        //Try and create this model in WebApp rather than the generic Model
        [NotMapped]
        public IFormFile? ORGIMAGEFILE { get; set; }
        [NotMapped]
        public string? DOCFILEURI { get; set; }
        [NotMapped]
        public string? IMAGEURI { get; set; }
        [NotMapped]
        public string? CODE_TEXT { get; set; }
        [NotMapped]
        public string? CODE_FPATH { get; set; }

    }



    public class MOBJECT_DOC
    {

        [Key]
        public int OBJECTID { get; set; }
        public int HIERID { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEFLD { get; set; }
        public int SOURCEID { get; set; }
        public string? DOCTYPE { get; set; }
        public string? DOCUMENTNAME { get; set; }
        public string? LDOCUMENTNAME { get; set; }
        public string? LFOLDERNAME { get; set; }
        public string? LMACHINENAME { get; set; }
        public string? SFOLDERNAME { get; set; }
        public double? FILESIZE { get; set; }
        public string? FILEXTENSION { get; set; }
        public string? INSERTED_BY { get; set; }
        public int? DFE { get; set; }
        public int? LT { get; set; }
        public DateTime? EFFDATE { get; set; }
        public DateTime? INSERT_DATE { get; set; }

        [NotMapped]
        public string? FYEARMONTH { get; set; }

        [NotMapped]
        public string? IMAGEURI { get; set; }

        [NotMapped]
        public string? DOCFILEURI { get; set; }

        [NotMapped]
        public string? CODE_TEXT { get; set; }

        [NotMapped]
        public string? SYSTEMID { get; set; }
        [NotMapped]
        public string? UNAME { get; set; }

        [NotMapped]
        public IFormFile? ORGIMAGEFILE { get; set; }

    }

    public class DOCUMENT_UPLOAD
    {

        [Key]
        public int OBJECTID { get; set; }
        public int SOURCEID { get; set; }
        public IxmAppDocumentType? DOCTYPE { get; set; }
        public string? DOCUMENTNAME { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? SYSTEMID { get; set; }
        public string? UNAME { get; set; }
        public IFormFile? ORGIMAGEFILE { get; set; }

    }

    public class CST_IDTYPE
    {

        [Key]
        public string IDTYPEID { get; set; }
        public string DESCRIPTION { get; set; }

    }
    public class CST_CITYTYPE
    {

        [Key]
        public string CITYTYPEID { get; set; }
        public string DESCRIPTION { get; set; }

    }

    public class CST_MARSTAT
    {

        [Key]
        public string MARSTATID { get; set; }
        public string DESCRIPTION { get; set; }

    }
    public class CST_MGENDER
    {

        [Key]
        public string GENDER { get; set; }
        public string DESCRIPTION { get; set; }

    }
    public class CST_MEMEXISTS
    {

        [Key]
        public int MEMEXISTS { get; set; }
        public string DESCRIPTION { get; set; }

    }
    public class CST_MEMSTATUSID
    {

        [Key]
        public int STATUSID { get; set; }
        public string DESCRIPTION { get; set; }

    }
    public class CST_SIMPLEMASTER
    {

        [Key]
        public int ID { get; set; }
        public string DESCRIPTION { get; set; }

    }

}
