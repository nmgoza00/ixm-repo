using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{

    public class SSMS_DATA
    {
        [Key]
        public int SSID { get; set; }
        public string SOURCEOBJECT { get; set; }
        public int SOURCEID { get; set; }
        public string CELLNUMBER { get; set; }
        public string SMESSAGE { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public DateTime SEND_DATE { get; set; }
        public string INSERTED_BY { get; set; }
        public int STATUSID { get; set; }
        public string SENT_BY { get; set; }
        public string SRESULT { get; set; }
    }

    public class SSMS_MESSAGE
    {
        public string to { get; set; }
        public string body { get; set; }

    }

    public class MMSG
    {
        [Key]
        public int MSGID { get; set; }
        public string SMESSAGE { get; set; }
        public string? MSGFIELDS { get; set; }
        public int? LANGUAGEID { get; set; }

    }
}
