using IXM.Models;
using IXM.Models.Core;
using IXM.Constants;
using Microsoft.IdentityModel.Tokens;
using IXM.Common.Constant;

namespace IXM.Common
{
    public class GenValues : IGenValues
    {
        GenFunctions genFunctions = new();

        public string? GetToken()
        {
            return CommonConstants.JWToken;
        }


        public Tuple<bool, string, double> CheckValueForDecimal(string value)
        {
            try
            {

                Double convertedNum = Convert.ToDouble(value);
                return new Tuple<bool, string, double>(true, value, convertedNum);
            }
            catch (Exception)
            {

                return new Tuple<bool, string, double>(false, value, 0);
            }
        }
        public Tuple<bool, string, DateTime> CheckValueForDate(string value)
        {
            return new Tuple<bool, string, DateTime>(true, "", DateTime.Now);
        }
        public Tuple<bool, int, string> CheckValueForCharacters(string value)
        {
            if (value.Any(ch => !char.IsLetterOrDigit(ch)))
            {

                return new Tuple<bool, int, string>(false, 5, "Unexpected value found");

            }
            return new Tuple<bool, int, string>(true, 0, "");
        }


        public Tuple<int, string> FillCustomIDNumber(string value, string idtype)
        {
            if (value.IsNullOrEmpty())
            {

                return new Tuple<int, string>(2, "Identity of Employee not populated");

            }

            return new Tuple<int, string>(2, "");

        }

        public Tuple<bool, int, string> CheckForValidIDNumber(string value, string idtype)
        {

            try
            {
                if (value.IsNullOrEmpty())
                {

                   return new Tuple<bool, int, string>(false, 2, "Identity of Employee not populated");

                } 
                /*else if (value.Any(ch => !char.IsLetterOrDigit(ch)))
                {

                   return new Tuple<bool, int, string>(false, 3, "Identity of Employee has unexpected values");

                }*/
                else if ((value.Length != 13) && (idtype == "ID Number"))
                {

                   return new Tuple<bool, int, string>(false,4, "ID Number is not valid");

                } 
                else
                {

                   return new Tuple<bool, int, string>(true, 0, "");

                }
         
            }
            catch (Exception)
            {

                return new Tuple<bool, int, string>(false, -1, "");
            }
        }

        public Tuple<bool, string, string> GetGenderFromIDNumber(string value)
        {
            try
            {
                string ls = value.Substring(6, 4);
                int li = Convert.ToInt16(ls);
                string gender = li < 5000 ? "Female" : "Male";
                return new Tuple<bool, string, string>(true, "", gender);

            }
            catch (Exception e)
            {
                return new Tuple<bool, string, string>(false, "", e.Message);
            }
        }

        public Tuple<bool, string, string> GetAgeFromIDNumber(string value)
        {//Still to develop
            try
            {
                string ls = value.Substring(6, 4);
                int li = Convert.ToInt16(ls);
                string gender = li < 5000 ? "Female" : "Male";
                return new Tuple<bool, string, string>(true, "", gender);

            }
            catch (Exception e)
            {
                return new Tuple<bool, string, string>(false, "", e.Message);
            }
        }


        public MOBJECT_DOC GenDocumentValues(int SourceId, IxmAppSourceObjects SourceObj, IxmAppDocumentType DocumentType, String SourceFld, string PathAndFileName, string BaseFolder)
        {

            var lt = genFunctions.GetDocumentPath("NUM", IxmAppDocumentType.XLS_REMITTANCEERR, "Live", BaseFolder);            
            string FileName = Path.GetFileName(PathAndFileName);
            var Filesize = GetDocumentSize(lt + PathAndFileName);

            var mDoc = new MOBJECT_DOC()
            {
                OBJECTID = -1,
                DOCUMENTNAME = FileName,
                DOCTYPE = DocumentType.ToString(),
                SOURCEID = SourceId,
                SOURCEOBJ = SourceObj.ToString(),
                SOURCEFLD = SourceFld,
                FILESIZE = Filesize,
                FILEXTENSION = "xlsx",
                SFOLDERNAME = lt.Item2,                
                INSERT_DATE = DateTime.UtcNow,
                LT = 1

            };
              
            return mDoc;
                
        }

        public long GetDocumentSize(string PathAndFileName)
        {
            try
            {
                long length = new System.IO.FileInfo(PathAndFileName).Length;
                return length;
            }
            catch (Exception)
            {

                return -1;
            }


        }




    }
}
