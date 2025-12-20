
using IXM.Models;
using IXM.Models.Core;
using IXM.Models.Write;
using static IXM.DB.QueryRepository;

namespace IXM.API.Services
{
    public interface IDataService
    {

        //int AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model);
        Tuple<int, string> RegisterBusinessUploadToRemmittance(ILogger logger, MOBJECT_DOC model, int pPeriodId, int pRSTYPEID);

        Tuple<int, string> ModifyEmployee(ILogger logger, MEMPLOYEE_WRITE mEMPLOYEE);
        bool ModifyUser(ILogger logger, MUSER mMUSER);
        bool ModifyCompany(ILogger logger, MCOMPANY_WRITE mCOMPANY);
        Task<bool> InsertNote(ILogger logger, MOBJECT_NOT mMOBJECT_NOT);
        bool InsertEmployee(ILogger logger, MEMPLOYEE mEMPLOYEE);
        Task<int> InsertUser(ILogger logger, MUSERWr mMUSER);
        //bool ModifyMemberCard(ILogger logger, MMEMBER mMEMBER);
        //bool ModifyMemberCard(ILogger logger, MMCDD mMMCDD);
        public Tuple<int, string> GetObjectFileName(IXMFileType ixmFileType, int pObjectId);

        string GetLinkedUserToEmployee(string pUserId);
        string GetDistribution_CardStats(string pBCompanyId, string pUsrName);
        string GetDistribution_SurveyStats(string pBCompanyId, string pUsrName);
        string GetSurvey_Member(string pMemberId);
        public string GetSurvey_CurrentList(string pMemberId);
        Task <bool> UpdateSurveyData(ILogger logger, TSURVEYMEMBERDATA surveyData);
        Task<bool> UpdateDeviceInfo(ILogger logger, MUSER_DEVICE pDevice);
        string GetMember_Details(string pId);
        string GetPureDocuments(string pCompanyId);
        string GetEmployeePhotos(string pEmployeeId);
        string GetOrganiser_BranchStats(string pUserId);
        string GetBranch_NonPaymentMembers(string pPeriod, string pBCompany);
        public string GetIdentity_UserList(string pUsrName);

        public string GetDepartmentLoadedSchedules(string pYear);
        public IEnumerable<MUSER_MENU> GetSystemMenuRights(string pIdentityUserID, int pSystemUserId);



    }
}
