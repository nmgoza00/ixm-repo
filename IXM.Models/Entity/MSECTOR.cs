using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{
       public class MSECTOR
        {
            [Key]
            public int SECTORID { get; set; }
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

}
