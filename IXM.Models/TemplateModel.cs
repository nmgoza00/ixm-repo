using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{




    public class EMAIL_TEMPLATE
    { 

        public string? Name { get; set; }
        public string? Surname { get; set; } 
        public string? userName { get; set; }
        public string? passWord { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
        public string? ToEmail { get; set; }
        public string? Organization { get; set; }
        public string? VerifyLink { get; set; }
        public string? CellNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyNumber { get; set; }
        public string? CompanyContactPerson { get; set; }
        public string? CompanyContactNumber { get; set; }
        public string? ProcessorName { get; set; }
        public string? FreeText1 { get; set; }
        public string? FreeText2 { get; set; }

    }

}
