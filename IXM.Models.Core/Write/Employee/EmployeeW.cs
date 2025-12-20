using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models.Core
{




    [Table("MEMPLOYEE")]


    public class MEMPLOYEE_WRITE
    {
        [Key]
        public int EMPID { get; set; }
        public int? EMPTYPID { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? IDNUMBER { get; set; }
        public string? GENDER { get; set; }
        public string? CELLNUMBER { get; set; }
        public string? EMAIL { get; set; }
        public string? VE { get; set; }
        public int? IPOSITIONID { get; set; }
        public int? MSTATUSID { get; set; }
        public int? LOCALITYID { get; set; }
        public int? UNIONID { get; set; }
        public int? MEMBERID { get; set; }
        public int? USERID { get; set; }

        public int? COUNTRYID { get; set; }
        public DateTime? VUDAT { get; set; }
        public DateTime? INDAT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }

    }


}
