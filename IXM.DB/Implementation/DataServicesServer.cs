using IXM.Models;
using Microsoft.Extensions.Configuration;   
using Microsoft.Extensions.Caching.Memory;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using IXM.Constants;
using Microsoft.IdentityModel.Tokens;

namespace IXM.DB.Server
{
    public class DataServicesServer : IDataServicesServer
    {

        public enum IXMModType
        {
            Insert,
            Modify,
            Delete
        }

        //private string _baseurl = App.aAppInfo.BaseAPIURL;
        private string _baseurl = "";
        //private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private readonly IXMDBContext _dbcontext;
        private readonly IXMJobDBContext _jobcontext;
        private readonly IGeneral _general;
        //private readonly IFileIngest _fileinjest;
        private readonly IMemoryCache _cache;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();
        private IGeneral _genral;

        public DataServicesServer(IConfiguration configuration,            
            IMemoryCache cache, IXMJobDBContext jobcontext, IXMDBContext dbcontext)
        {
                //this._environment = environment;
            _configuration = configuration;
            _dbcontext = dbcontext ;
            _jobcontext = jobcontext;
            //_fileinjest = fileinject;
            _cache = cache;
            _general = new General(dbcontext, null, null, null);
            //GetClient();
            //mSYSTEMS = customData.GetSystems();

        }


        public async Task<List<PROCESS_RUN>> GetProcessQueueAsync()
        {
            try
            {
                //var process = await _jobcontext.PROCESS_RUN.Where(a => a.STATUS == "Queue").FirstAsync();
                var ptask = await _jobcontext.TPROCESSTASK.Where(a => a.PARAMFLD == "RMBLID")
                                                      .Where(b => b.PARAMVAL == "").SingleOrDefaultAsync();
                var process = await _jobcontext.PROCESS_RUN.Where(a => a.PROCESSID == ptask.PROCESSID).ToListAsync();

                return process;

            } catch
            {
                return null;
            }



        }
        public async Task<List<PROCESS_RUN>> GetProcessId(List<TPROCESSTASK> tPROCESSTASK)
        {
            try
            {

                var paramValues = GetProcessTaskValues(tPROCESSTASK);
                var process = _jobcontext.PROCESS_RUN.Where(a => a.STATUS == "Queued").ToList();
                return process;

            }
            catch
            {
                return null;
            }



        }
        public async Task<PROCESS_RUN> ExecuteQueueAsync(int PROCESSID)
        {
            try
            {

                var process = _jobcontext.PROCESS_RUN.Where(a => a.PROCESSID == PROCESSID).SingleOrDefault();

                if (process != null)
                {
                    if (process.TASKNAMEID == 1)  // schedule loading
                    {
                        ExecuteRemittanceQueueAsync(PROCESSID);



                    }

                }

                return process;

            } catch
            {
                throw;
            }


        }

        private LTASKPARAMVALUES GetProcessTaskValues(List<TPROCESSTASK> taskrun)
        {

            LTASKPARAMVALUES lv = new LTASKPARAMVALUES();
            foreach (var lrow in taskrun)
            {
                if  (lrow.PARAMVAL != null)
                {
                    if (lrow.PARAMFLD.ToString() == "FILEID")
                    {
                        lv.OBJECTID = Convert.ToInt32(lrow.PARAMVAL);
                    }
                    if (lrow.PARAMFLD.ToString() == "SYSTEMID")
                    {
                        lv.SYSTEMID = Convert.ToInt32(lrow.PARAMVAL);
                    }
                    if (lrow.PARAMFLD.ToString() == "RMBLID")
                    {
                        lv.RMBLID = Convert.ToInt32(lrow.PARAMVAL);
                    }

                }

            }

            return lv;

        }
        public async Task<List<TPROCESSTASK>> GetProcessTaskParams(int PROCESSID)
        {
            try
            {

                var taskrun = _jobcontext.TPROCESSTASK.Where(a => a.PROCESSID == PROCESSID).ToList();
                return taskrun;
            }
            catch (Exception)
            {

                throw;
                return null;
            }


        }
        public async Task<PROCESS_RUN> ExecuteRemittanceQueueAsync(int PROCESSID)
        {
            string lFileId = "0";
            string lSystemId = "0";
            string lRMBLId = "0";

            var process = _jobcontext.PROCESS_RUN.Where(a => a.PROCESSID == PROCESSID).SingleOrDefault();
            var taskrun = _jobcontext.TPROCESSTASK.Where(a => a.PROCESSID == process.PROCESSID).ToList();

            if (taskrun != null)
            {
                foreach (var lrow in taskrun)
                {
                    if (lrow.PARAMFLD.ToString() == "FILEID")
                    {
                        lFileId = lrow.PARAMVAL.ToString();
                    }
                    if (lrow.PARAMFLD.ToString() == "SYSTEMID")
                    {
                        lSystemId = lrow.PARAMVAL.ToString();
                    }
                    if (lrow.PARAMFLD.ToString() == "RMBLID")
                    {
                        lRMBLId = lrow.PARAMVAL.ToString();
                    }
                }

            }

            if (lFileId == "0")
            {

                ProcessFailAsync("Initiated Incorrectly-Closing", process.PROCESSID.ToString());

            }
            else
            {
                /*
                var _mobject_doc = await _fileinjest.GetFileDocumentInfo(Convert.ToInt32(lFileId));
                _mobject_doc.SYSTEMID = lSystemId;

                ITransaction ldss = new Transaction(_dbcontext, null,null, _configuration, null,null,null,null,null,null, null);

                string lFld = _configuration.GetConnectionString("BaseDocFolder");

                ldss.GenerateRemittanceToDataBase(_mobject_doc, lRMBLId, _mobject_doc.INSERTED_BY, lFld);
                

                //var lresult = await _fileinjest.XlsRemittanceInjest(_mobject_doc.INSERTED_BY, _mobject_doc.OBJECTID.ToString(), _mobject_doc.SYSTEMID.ToString(), Convert.ToChar('Y'));
                if (_mobject_doc != null)
                {

                }
                */
            }




                return process;

        }

        public async Task<List<DATA_VALUEMAPPING>> GetDataValueMapping()
        {
            // Define a cache key
            var cacheKey = "ixm-dvm-data";

            // Try to get data from cache
            if (!_cache.TryGetValue(cacheKey, out List<DATA_VALUEMAPPING>? data))
            {
                // If not in cache, fetch from DB
                data = _dbcontext.DATA_VALUEMAPPING.ToList();

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // refresh expiry if accessed again

                // Save in cache
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }

            return data!;
        }


        public async Task<List<MCOMPANY>>GetBranches()
        {
            var data = _dbcontext.MCOMPANY.Where(a => a.COMPTYPID == 4).ForRemitInfo().ToList();
            return data;

        }
        public async Task<List<MCITY>> GetCitites()
        {
            var data = _dbcontext.MCITY.ToList();
            return data;
        }

        public async Task<string> ProcessQueueAsync(List<TPROCESSTASK> tPROCESSTASK)
        {
            var processId = Guid.NewGuid().ToString();
            int seq = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQPROCESSRUN), _jobcontext);


            var tracker = new PROCESS_RUN
            {
                PROCESSID = seq,
                TASKID = processId,
                //--PROCESS_NAME = "DataImport",
                FROM_DATE = DateTime.Now,
                TO_DATE = null,
                STATUS = "Queued"
            };


            _jobcontext.PROCESS_RUN.Add(tracker);

            if (tPROCESSTASK != null)
            {
                tPROCESSTASK.ForEach(s => s.PROCESSID = tracker.PROCESSID);
                _jobcontext.TPROCESSTASK.AddRange(tPROCESSTASK);

            }

            await _jobcontext.SaveChangesAsync();

            return processId;

        }

        public async Task<string> ProcessStartAsync(string pProcessId)
        {
            if (!pProcessId.IsNullOrEmpty())
            {
                // Update status to completed
                var process = _jobcontext.PROCESS_RUN.Where(a => a.TASKID == pProcessId).SingleOrDefault();
                if (process != null)
                {
                    process.STATUS = "Started";
                    process.TO_DATE = DateTime.Now;
                    _jobcontext.SaveChanges();
                }
            }

            return pProcessId;
        }


        public async Task<string> ProcessCompleteAsync(string pProcessId)
        {
            if (!pProcessId.IsNullOrEmpty())
            {

                // Update status to completed
                var process = _jobcontext.PROCESS_RUN.Where(a => a.TASKID == pProcessId).SingleOrDefault();
                if (process != null)
                {
                    process.STATUS = "Completed";
                    process.TO_DATE = DateTime.Now;
                    await _jobcontext.SaveChangesAsync();
                }
            }
            return pProcessId;

        }
        public async Task<string> ProcessDeleteAsync(string pProcessId)
        {
            if (!pProcessId.IsNullOrEmpty())
            {

                // Update status to completed
                var process = _jobcontext.PROCESS_RUN.Where(a => a.TASKID == pProcessId).SingleOrDefault();
                var tprocess = _jobcontext.TPROCESSTASK.Where(a => a.PROCESSID == process.PROCESSID).SingleOrDefault();

                _jobcontext.PROCESS_RUN.Remove(process);
                _jobcontext.TPROCESSTASK.Remove(tprocess);
                await _jobcontext.SaveChangesAsync();

            }
            return pProcessId;

        }

        public async Task<string> ProcessFailAsync(string pMessage, string pProcessId)
        {
            try
            {
                var process = _jobcontext.PROCESS_RUN.Where(a => a.PROCESSID == (Convert.ToInt16(pProcessId))).First();
                if (process != null)
                {
                    process.STATUS = "Failed";
                    process.MESSAGE = pMessage;
                    process.TO_DATE = DateTime.Now;
                    _jobcontext.SaveChanges();
                }
                return pProcessId;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<PROCESS_RUN> ProcessCheckStatus(string pProcessId)
        {
            var process = _jobcontext.PROCESS_RUN.Where(a => a.TASKID == pProcessId).SingleOrDefault();
            if (process == null)
                return null;
            else return process;
        }


    }
    }
