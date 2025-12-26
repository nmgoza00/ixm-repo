using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{
    
    public class PROCESS_RUN
    {
        public int SYSTEMID { get; set; }
        [Key]
        public int PROCESSID { get; set; }
        public string TASKID { get; set; }
        public DateTime FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public int TASKNAMEID { get; set; }
    }

    public class MPROCESSTASK
    {
        [Key]
        public int TASKNAMEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string ISACTIVE { get; set; }
        public string TASKCODE { get; set; }
    }
    public class TPROCESSTASK
    {
        public int PROCESSID { get; set; }
        public string PARAMOBJ { get; set; }
        public string PARAMFLD { get; set; }
        public string PARAMVAL { get; set; }
    }

    public class TJOBLOG
    {
        [Key]
        public DateTime RUNDATE { get; set; }
        public string DESCRIPTION { get; set; }
        public int INSTANCES { get; set; }
    }
    public class LTASKPARAMVALUES
    {
        [Key]
        public int PROCESSID { get; set; }
        public int COMPANYID { get; set; }
        public int SYSTEMID { get; set; }
        public int OBJECTID { get; set; }
        public int PERIODID { get; set; }
        public int RMBLID { get; set; }
        public string TASKNAMEID { get; set; }
        public string TASKID { get; set; }
    }




}
