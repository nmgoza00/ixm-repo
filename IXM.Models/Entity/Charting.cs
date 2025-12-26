using IXM.Constants;
using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{
    public class Charting
    {
    }


    public class IXMChartData
    {
        public string LabelGroupId { get; set; }
        public string LabelGroupName { get; set; }
        public string LabelGroupDescription { get; set; }
        public string Label { get; set; }
        public int LabelValue { get; set; }
        public string Color { get; set; }
    }

    public class IXMChartDataS
    {
        public string Label { get; set; }
        public int LabelValue { get; set; }
    }
    public class IXMDDType
    {
        public string ID { get; set; }
        public string Description { get; set; }
    }
    public class IXMCaptions
    {
        public string FIELDNAME { get; set; }
        public string Description { get; set; }
        public int? Length { get; set; }
    }


    public class TCOUNT
    {
        public int? COUNTER { get; set; }
    }


    public class IXMDocTypeToObject
    {
       public IxmAppDocumentType docType { get; set; }
       public string SOURCEOBJ { get; set; }
       public string SOURCEFLD { get; set; }

    }





}
