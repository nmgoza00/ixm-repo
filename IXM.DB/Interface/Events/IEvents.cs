using IXM.Common.Notification;
using IXM.Models;
using IXM.Models.Events;
using Microsoft.AspNetCore.Http;

namespace IXM.DB
{
    public interface IEvents
    {

        Task<int> EventAttend(IIXMNotification notification, int pECOMPID, TPRJEVTD tPRJEVTD, int SystemId);
        Task<TEVENTWr> Modify_EvtEvent(IIXMNotification notification, TEVENTWr tEVENT, int SystemId);
        Task<TPROJECTWr> Modify_EvtProject(IIXMNotification notification, TPROJECTWr tPROJECT, int SystemId);
        Task<TPRJEVT> Event(IIXMNotification notification, TPRJEVT tPRJEVT, int SystemId);
        string GetConfigPrefix(string OBJECTNAME, string FIELDNAME, int lVal);
        int GetConfigStatus(string OBJECTNAME, int SEQUENCE);
        int GetConfigStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext);
        int GetConfigMStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext);
        int GetUserGroupId(string GROUPOBJECT, IXMWriteDBContext dbcontext);
        Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext);

        Task<List<TPROJECT>> GetEvtProject();
        Task<List<TEVENT>> GetEvtEvent();

    }
}
