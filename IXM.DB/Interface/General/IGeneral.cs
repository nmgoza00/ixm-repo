using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IXM.DB
{
    public interface IGeneral
    {
        int GetSEQUENCE(string SEQNAME);
        int GetSEQUENCE(string SEQNAME, IXMWriteDBContext dbcontext);
        Task<int> GetSEQUENCE(string SEQNAME, IXMDBContext dbcontext);
        int GetSEQUENCE(string SEQNAME, IXMSystemDBContext systemdbcontext);
        string GetConfigPrefix(string OBJECTNAME, string FIELDNAME, int lVal);
        int GetConfigStatus(string OBJECTNAME, int SEQUENCE);
        int GetConfigStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext);
        int GetConfigMStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext);
        int GetSEQUENCE(string SEQNAME, IXMJobDBContext jobcontext);
        int GetUserGroupId(string GROUPOBJECT, IXMWriteDBContext dbcontext);
        Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext);
        MOBJECT_DOC GetObjectDoc(DOCUMENT_UPLOAD docUpload);

    }
}
