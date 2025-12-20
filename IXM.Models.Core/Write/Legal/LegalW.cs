using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IXM.Models.Core
{



    public class TCASE_WRITE
    {
        [Key]
        public int CASEID { get; set; }
        public string? CASENUMBER { get; set; }
        public int CASETYPEID { get; set; }
        public int CASESTATUSID { get; set; }
        public int CASEPRIORITY { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime INSDT { get; set; }
        public string INSBY { get; set; }
        public DateTime MODAT { get; set; }
        public string MODBY { get; set; }
    }
}

