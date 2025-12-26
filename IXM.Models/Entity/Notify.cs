using System.ComponentModel.DataAnnotations;

namespace IXM.Models.Notify

{

    public class TMB_NOTICE
    {
        [Key]
        public int NOTIFICATIONID { get; set; }
        public string? NOTIFYTYPE { get; set; }
        public string? HEADING { get; set; }
        public string? NOTICE_TEXT { get; set; }
        public string? NSRC_IMAGE  { get; set; }
        public string? NTGT_LINK { get; set; }
        public DateTime? EXPIRATIONDATE { get; set; }
        public string? ISACTIVE { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }

    }

    public class TMB_NOTICE_AUDIANCE
    {
        [Key]
        public int NOTIFICATIONID { get; set; }
        public string? AUDIANCETYPE { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEFLD { get; set; }
        public string? SOURECID { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }

    }
    public class API_RESPONSE
    {
        [Key]
        public int RESPONSEID { get; set; }
        public int StatusCode { get; set; }
        public string GeneralMessage { get; set; }
        public string TechnicalMessage { get; set; }
        public DateTime InsertDate { get; set; }

    }

}