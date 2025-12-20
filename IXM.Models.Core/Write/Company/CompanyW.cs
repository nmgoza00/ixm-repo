using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models.Core
{




    [Table("MCOMPANY")]
    public class MCOMPANY_WRITE
    {
        [Key]
        public int COMPANYID { get; set; }
        public string HGUID { get; set; }
        public string? CNAME { get; set; }
        public string? COMPANYNUM { get; set; }
        public int? RCOMPANYID { get; set; }
        public int? COMPTYPID { get; set; }
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
        public string? POBOXNAME { get; set; }
        public string? POBOXPREFIX { get; set; }
        public int? POBOXNO { get; set; }
        public string? POPOSTALCODE { get; set; }
        public string? BCITYID { get; set; }
        public string? BPROVINCEID { get; set; }
        public string? COMPANYINFO { get; set; }
        public string? TO_INVOICE { get; set; }
        public int? COMDTYID { get; set; }
        public int? BANK_REFNR { get; set; }
        public string? SECTORNAME { get; set; }
        public int? MEMCOUNT { get; set; }
        public int? HMEMCNT { get; set; }
        public int? HMEMAVG { get; set; }
        public int? DBFACCTID { get; set; }
        public int? CRFACCTID { get; set; }
        public int? MSTATUSID { get; set; }
        public string? WREGCODE { get; set; }
        public string? STIC { get; set; }
    }

}
