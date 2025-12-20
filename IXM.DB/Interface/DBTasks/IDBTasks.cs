using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using IXM.Models;
using Microsoft.AspNetCore.Mvc;

namespace IXM.DB
{
    public interface IDBTasks
    {
        Task StatusTriggerCreate(string TBNAME, string CHKFIELD, string KEYFIELD);
        Task StatusTriggerDrop(string TBNAME, string FLDNAME);
        Task DatabaseBackup(string TBNAME);
        Task DatabaseSweep(string TBNAME);
    }
}
