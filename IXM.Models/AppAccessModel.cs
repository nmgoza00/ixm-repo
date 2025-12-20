

using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{

    public class APPDOMAINLICENSE
    {
        [Key]
        public int SYSTEMID { get; set;}
        //The id will actually be the User Guiid
        public string? id { get; set; }
        public int SYSTEMUSERID { get; set; }        
        public string DOMAINNAME { get; set; }
        public string DOMAINREQUEST { get; set; }
        public string? APPSERVERNAME { get; set; }
        public string? APPSERVERPORTNO { get; set; }
        public string? DBNAME { get; set; }
        public string? DBSERVERNAME { get; set; }
        public string? APPSERVERDEVPORTNO { get; set; }
    }



}
