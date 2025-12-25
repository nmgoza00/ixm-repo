using IXM.Constants;
using IXM.Models;
using IXM.Models.Write;
using IXM.Models.Core;
using IXM.Models.Notify;

namespace IXM.DB
{
    public interface IMasterData
    {
        Task<List<MCASETYPE>> GetCaseType();
        Task<List<MCITY>> GetCities();
        Task<List<MLOCALITY>> GetLocality();
        Task<List<MPERIOD>> GetPeriod(int? PeriodId);
        Task<List<MSTATUS_TEXT>> GetStatusText();
        Task<List<MPROVINCE>> GetProvince();
        Task<List<MUSER_GROUP>> GetUserGroup();
        Task<List<MSECTOR>> GetSector();
        Task<List<MUNION>> GetUnion();
        Task<List<MINTPOSITION>> GetIntPosition();
        Task<List<MEXTPOSITION>> GetExtPosition();
        Task<List<MEMPTYPE>> GetEmployeeType();
        Task<List<MSYSTEM>> GetSystem();
        Task<List<MUNION>> GetUnionSystem();
        Task<List<MCOMPTYPE>> GetCompanyType();
        Task<List<TCAPTIONS>> GetCaption(IxmAppCaptionObject CaptionObject);
        Task<List<CLOCALITY>> GetLocalityCityMap();
        Task<List<CST_MGENDER>> GetGender();
        Task<List<CST_CITYTYPE>> GetCityType();
        Task<List<MLANGUAGE>> GetLanguage();
        Task<List<CST_MARSTAT>> GetMaritalStatus();



        Task<int> PostSector(MSECTORWr mSECTOR);
        Task<API_RESPONSE> PostCity(MCITY mCITY);
        Task<int> PostUnion(MUNIONWr mUNION);
        Task<int> PostLocality(MLOCALITYWr mLOCALITY);
        Task<int> PostPeriod(MPERIODWr mPERIOD);
        Task<int> PostCaseType(MCASETYPEWr mCASETYPE);
        Task<int> PostIntPosition(MINTPOSITIONWr mINTPOSITION);
        Task<int> PostLocalityCityMap(List<CLOCALITY> cLOCALITies);
    }
}
