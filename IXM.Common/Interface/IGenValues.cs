using IXM.Constants;
using IXM.Models.Core;

namespace IXM.Common {
    public interface IGenValues
    {
        string? GetToken();


        Tuple<bool,string,double> CheckValueForDecimal(string value);
        Tuple<bool,string, DateTime> CheckValueForDate(string value);
        Tuple<bool,int, string> CheckValueForCharacters(string value);
        Tuple<bool, int, string> CheckForValidIDNumber(string value, string idtype);
        Tuple<bool, string, string> GetGenderFromIDNumber(string value);
        Tuple<bool, string, string> GetAgeFromIDNumber(string value);
        Tuple<int, string> FillCustomIDNumber(string value, string idtype);
        MOBJECT_DOC GenDocumentValues(int SourceId, IxmAppSourceObjects SourceObj, IxmAppDocumentType DocumentType, string SourceFld, string PathAndFileName, string BaseFolder);
        long GetDocumentSize(string PathAndFileName);

        //Tuple<bool, string, string> ValueToValidate(string columname, string columnvalue);
    }
}
