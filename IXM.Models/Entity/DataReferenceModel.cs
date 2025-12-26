using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{
    public class DataReferenceModel
    {
    }


    public class MCOMPTYP
    {
        [Key]
        public int COMPTYPID { get; set; }
        public string? DESCRIPTION { get; set; }

    }

    public class MSYSTEMS
    {
        [Key] 
        public string SYSTEMID { get; set;}
        public string? SYSTEMNAME { get; set;}
    }


}
