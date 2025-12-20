using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{

    public static class TCASESNWrite
    {
        public static IQueryable<TCASE> LegalConfirm(this IQueryable<TCASE> source)
        {
            return source.Select(b => new TCASE
            {
                CASEID = b.CASEID,
                CASESTATUSID = b.CASESTATUSID,
                CASEPRIORITY = b.CASEPRIORITY,
                CASETYPEID = b.CASETYPEID,
                DESCRIPTION = b.DESCRIPTION,
                INSBY = b.INSBY,
                INSDT = b.INSDT,
                HGUID = b.HGUID,

            });
        }
    }

    public class TCASE
    {
        [Key]
        public int CASEID { get; set; }
        public string? CASENUMBER { get; set; }
        public int CASESTATUSID { get; set; }
        public int CASETYPEID { get; set; }
        public int CASEPRIORITY { get; set; }
        public int CASEAFFECTING { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public string? HGUID { get; set; }


        public virtual MSTATUS_TEXT? CaseStatus { get; set; }
        public virtual MCASETYPE? CaseType { get; set; }

    }

    public class MCASETYPE
    {
        [Key]
        public int CASETYPEID { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }

    }






}
