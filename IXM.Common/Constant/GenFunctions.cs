using IXM.Models;
using IXM.Constants;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;



namespace IXM.Common.Constant
{

    public class GenFunctions
    {
       //public GenFunctions(ProtectedLocalStorage protectedLclStorage)
       // {
       //     _protectedLclStorage = protectedLclStorage;
       // }
        

        public Tuple<int, string> GetDocumentPath(string pSystemName, IxmAppDocumentType pDoctype, string lSystemType, string BaseFolder)
        {

            try
            {
               

                if (BaseFolder is null)
                {
                    BaseFolder = "C:\\APPS\\IXM\\";
                    //return new Tuple<int, string>(-2, "Base folder not specified in Configuration.");

                }

                var Path = "";
                if (pDoctype == IxmAppDocumentType.XLS_TEMPLATE)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Xls_Templates\\";
                }
                else if (pDoctype == IxmAppDocumentType.XLS_REMITTANCE)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Xls_Remittance\\";
                }
                else if (pDoctype == IxmAppDocumentType.EVENT)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Event\\";
                }
                else if (pDoctype == IxmAppDocumentType.XLS_REMITTANCEERR)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Xls_RemittanceErr\\";
                }
                else if (pDoctype == IxmAppDocumentType.MEMAPPLICATION)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Member\\";
                }

                return new Tuple<int, string>(0, Path);

            }
            catch (Exception e)
            {
                return new Tuple<int, string>(-1, e.Message);

            }


        }


        public Tuple<int, string> GetTemplateDocument(string pSystemName, IxmAppDocumentType pDoctype, IxmAppTemplateFileType pFiletype, string lSystemType, string BaseFolder)
        {

            try
            {


                if (BaseFolder is null)
                {
                    BaseFolder = "C:\\APPS\\IXM\\";
                    //return new Tuple<int, string>(-2, "Base folder not specified in Configuration.");

                }

                var Path = "";
                if (pDoctype == IxmAppDocumentType.XLS_TEMPLATE)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Xls_Templates\\";
                }
                else if (pDoctype == IxmAppDocumentType.MEMAPPLICATION)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Member\\";
                }
                else if (pDoctype == IxmAppDocumentType.XLS_REPORT)
                {
                    Path = BaseFolder + pSystemName + "\\" + lSystemType + "\\Aplication\\Xls_Reports\\";
                }

                if (pFiletype == IxmAppTemplateFileType.Schedule)
                {
                    Path = Path + "Schedule\\TemplateSchedule.xlsx";
                }

                return new Tuple<int, string>(0, Path);

            }
            catch (Exception e)
            {
                return new Tuple<int, string>(-1, e.Message);

            }


        }

        public string GetBlockScriptName(IxmBlockScriptType ScriptType)
        {
            if (ScriptType == IxmBlockScriptType.BS_UpdatePostPayment)
            {
                return "\\UPD_PAYMENT_LT.block";
            }
            else return "";
        }


        public async Task<DEVICE_INFO> GetWebDeviceInfo()
        {

            DEVICE_INFO ldeviceinfo = new DEVICE_INFO();


            ldeviceinfo.DeviceType = "WebApplication";
            ldeviceinfo.Model = "";
            ldeviceinfo.Name = "";
            ldeviceinfo.Manufacturer = "";
            ldeviceinfo.OSVersion = "";
            ldeviceinfo.Idiom = "";
            ldeviceinfo.Platform = "";

            return ldeviceinfo;

        }

        public string GenerateRandomizedCode(int companyId)
        {

            string Base36Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int CodeLength = 6;
            long Offset = 60466176;
            long MaxCompanyId = 2116316159; 

            if (companyId < 0 || companyId > MaxCompanyId)
                throw new ArgumentOutOfRangeException(nameof(companyId),
                    $"CompanyID must be between 0 and {MaxCompanyId}.");

            long value = companyId + Offset;
            char[] code = new char[CodeLength];

            for (int i = CodeLength - 1; i >= 0; i--)
            {
                code[i] = Base36Characters[(int)(value % 36)];
                value /= 36;
            }

            return new string(code);

        }


        /*
        public void LocallyStore(string stRoute, string stValue)
        {

            _protectedLclStorage.SetAsync(stRoute, stValue);

        }
        public async Task<string> Locallyfetch(string stRoute)
        {


            ProtectedBrowserStorageResult<string> result_uid = await _protectedLclStorage.GetAsync<string>(stRoute);
            if (result_uid.Success)
            {
                return result_uid.Value;
            }
            else return null;

        }
        */

    }
}
