

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{

    public class APP_LOGTABLE
    {
        public string TableName { get; set; }
        public string SYSTEMID { get; set; }
        public DateTime LOGTIME { get; set; }
        public string LOGTYPE { get; set; }
        public string SOURCEOBJ { get; set; }
        public string SOURCEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string USERNAME { get; set; }
        public string? IPADDRESS { get; set; }
    }
    [Keyless]
    public class APP_LOGTABLEI
    {
        public int SYSTEMID { get; set; }
        public DateTime LOGTIME { get; set; }
        public string LOGTYPE { get; set; }
        public string SOURCEOBJ { get; set; }
        public string SOURCEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string USERNAME { get; set; }
        public string? IPADDRESS { get; set; }
    }

    [Keyless]
    public class APP_LOGTABLEE
    {
        public int SYSTEMID { get; set; }
        public DateTime LOGTIME { get; set; }
        public string LOGTYPE { get; set; }
        public string SOURCEOBJ { get; set; }
        public string SOURCEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string USERNAME { get; set; }
        public string? IPADDRESS { get; set; }
    }

    [Keyless]
    public class APP_LOGTABLEF
    {
        public int SYSTEMID { get; set; }
        public DateTime LOGTIME { get; set; }
        public string LOGTYPE { get; set; }
        public string SOURCEOBJ { get; set; }
        public string SOURCEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string USERNAME { get; set; }
        public string? IPADDRESS { get; set; }
    }

    [Keyless]
    public class APP_LOGTABLER
    {
        public int SYSTEMID { get; set; }
        public DateTime LOGTIME { get; set; }
        public string LOGTYPE { get; set; }
        public string SOURCEOBJ { get; set; }
        public string SOURCEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string USERNAME { get; set; }
        public string? IPADDRESS { get; set; }
    }


}
