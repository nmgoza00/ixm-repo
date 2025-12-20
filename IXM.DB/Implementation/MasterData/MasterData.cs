using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using DocumentFormat.OpenXml.Spreadsheet;
using IXM.Common;
using IXM.Common.Constant;
using IXM.Common.Data;
using IXM.Constants;
using IXM.Models;
using IXM.Models.Core;
using IXM.Models.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;

namespace IXM.DB
{
    public class MasterData : IMasterData
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IXMDBContextFactory _dbfactory;
        private readonly CustomData _customData = new CustomData();
        private readonly IMemoryCache _memCache;
        private readonly IGeneral _general;
        private readonly ILogger<MasterData> _logger;

        public GenFunctions genFunctions = new GenFunctions();

        public MasterData(
                            IXMDBIdentity idtcontext,
                            IXMDBContextFactory dbfactory,
                            IXMDBContext dbcontext,
                            IMemoryCache memoryCache,
                            ILogger<MasterData> logger)
        {
            _dbcontext = dbcontext;
            _dbfactory = dbfactory;
            _memCache = memoryCache;
            _general = new General(_dbcontext, null, null, null);
            _identitycontext = idtcontext;
            _logger = logger;
        }


        public async Task<List<MCASETYPE>> GetCaseType()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("CaseTypeCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MCASETYPE.ToListAsync();

                });

                return result ?? new List<MCASETYPE>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MCITY>> GetCities()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("CityCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MCITY.ToListAsync();

                });

                return result ?? new List<MCITY>();


            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MEMPTYPE>> GetEmployeeType()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("EmpTypeCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MEMPTYPE.ToListAsync();

                });

                return result ?? new List<MEMPTYPE>();


            }
            catch (Exception)
            {

                return null;
            }

        }


        public async Task<List<MLOCALITY>> GetLocality()
        {
            try
            {
                var result = await _memCache.GetOrCreateAsync("MLOCALITYCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MLOCALITY.ToListAsync();

                });

                return result ?? new List<MLOCALITY>();

            }
            catch (Exception)
            {

                return null;
            }

        }


        public async Task<List<CLOCALITY>> GetLocalityCityMap()
        {
                       try
            {
                var result = await _memCache.GetOrCreateAsync("CLOCALITYCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.CLOCALITY.ToListAsync();
                });
                return result ?? new List<CLOCALITY>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> PostLocalityCityMap(List<CLOCALITY> cLOCALITies)
        {

            try
            {
                var aSECTOR = _dbcontext.CLOCALITY.ToList();
                var cSECTOR = _dbcontext.CLOCALITY.Where(a => a.LOCALITYID == cLOCALITies.First().LOCALITYID).ToList();
                List<CLOCALITY> delClocality = new List<CLOCALITY>();
                List<CLOCALITY> addClocality = new List<CLOCALITY>();

                if (cSECTOR != null)
                {


                    // Loop through each DataTable in the DataSet
                    foreach (CLOCALITY dt in cSECTOR)
                    {
                        // Delete
                        if (!cLOCALITies.Any(x => x.LOCALITYID == dt.LOCALITYID && x.CITYID == dt.CITYID))
                        {
                            delClocality.Add(dt);
                        }   

                    }


                    foreach (CLOCALITY dt in cLOCALITies)
                    {
                        // Add
                        if (!aSECTOR.Any(x => x.LOCALITYID == dt.LOCALITYID && x.CITYID == dt.CITYID))
                        {
                            addClocality.Add(dt);
                        }

                    }


                    if (delClocality.Count > 0)
                    {
                        _dbcontext.CLOCALITY.RemoveRange(delClocality);
                        await _dbcontext.SaveChangesAsync();
                    }

                    if (addClocality.Count > 0)
                    {
                        _dbcontext.CLOCALITY.AddRange(addClocality);
                        await _dbcontext.SaveChangesAsync();
                    }


                    return 0;

                }
                else
                {

                    return 0;
                }


            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -1;
            }

        }

        public async Task<List<MPERIOD>> GetPeriod(int? PeriodId)
        {
            try
            {


                var result = await _memCache.GetOrCreateAsync("MPERIODCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    /*string lSql = "SELECT v.* FROM MPERIOD v";
                    if (PeriodId != null)
                    {
                        lSql += " WHERE v.PRID > " + PeriodId.ToString();
                    }*/
                    return await _dbcontext.MPERIOD.ToListAsync();

                });

                return result ?? new List<MPERIOD>();


            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MSTATUS_TEXT>> GetStatusText()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MSTATUS_TEXTCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MSTATUS_TEXT.ToListAsync();

                });

                return result ?? new List<MSTATUS_TEXT>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MPROVINCE>> GetProvince()
        {
            try
            {
                var result = await _memCache.GetOrCreateAsync("MPROVINCECache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MPROVINCE.ToListAsync();

                });

                return result ?? new List<MPROVINCE>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MUSER_GROUP>> GetUserGroup()
        {
            try
            {
                var result = await _memCache.GetOrCreateAsync("MUSERGROUPCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MUSER_GROUP.ToListAsync();

                });

                return result ?? new List<MUSER_GROUP>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MCOMPTYPE>> GetCompanyType()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MCOMPTYPEECache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MCOMPTYPE.ToListAsync();

                });

                return result ?? new List<MCOMPTYPE>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MSECTOR>> GetSector()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MSECTORCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MSECTOR.ToListAsync();

                });

                return result ?? new List<MSECTOR>();

            }
            catch (Exception)
            {

                return null;
            }

        }
        public async Task<List<MUNION>> GetUnion()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MUNIONCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MUNION.ToListAsync();

                });

                return result ?? new List<MUNION>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MINTPOSITION>> GetIntPosition()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MINTPOSITIONCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MINTPOSITION.ToListAsync();

                });

                return result ?? new List<MINTPOSITION>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MEXTPOSITION>> GetExtPosition()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MEXTPOSITIONCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MEXTPOSITION.ToListAsync();

                });

                return result ?? new List<MEXTPOSITION>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<MSYSTEM>> GetSystem()
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("MSYSTEMCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _identitycontext.MSYSTEM.ToListAsync();

                });

                return result ?? new List<MSYSTEM>();

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<CST_MGENDER>> GetGender()
        {

           return _customData.GetGender();
        }
        public async Task<List<CST_CITYTYPE>> GetCityType()
        {

            return _customData.GetCityType();
        }

        public async Task<List<MLANGUAGE>> GetLanguage()
        {

            return _customData.GetLanguage();
        }

        public async Task<List<CST_MARSTAT>> GetMaritalStatus()
        {

            return _customData.GetMaritalStatus();
        }


        public async Task<List<MUNION>> GetUnionSystem()
        {
            try
            {

                var tu = await _memCache.GetOrCreateAsync("MUNIONCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _dbcontext.MUNION.ToListAsync();
                });

                var tus = await _memCache.GetOrCreateAsync("MUNIONSystemCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    return await _identitycontext.MUNION_SYSTEM.ToListAsync();
                });


                var mSYSTEMS = _customData.GetSystems();


                var fresult = (from o in tu
                               join c in tus
                                  on o.UNIONID equals c.UNIONID
                               select new MUNION
                               {
                                   UNIONID = o.UNIONID,
                                   SHORTDESC = o.SHORTDESC,
                                   DESCRIPTION = o.DESCRIPTION,
                                   SYSTEMID = c.SYSTEMID
                               }).ToList();

                return fresult;

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<List<TCAPTIONS>> GetCaption(IxmAppCaptionObject CaptionObject)
        {
            try
            {

                var result = await _memCache.GetOrCreateAsync("TCAPTIONSCache", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                    string lSql = "SELECT * FROM APP_OBJECTSETUP WHERE OBJECTNAME = '" + CaptionObject.ToString() + "' ORDER BY FIELD_POSITION";
                    return await _dbcontext.TCAPTIONS.FromSqlRaw<TCAPTIONS>(lSql).ToListAsync();

                });

                return result ?? new List<TCAPTIONS>();

            }
            catch (Exception)
            {

                return null;
            }

        }


        public async Task<int> PostSector(MSECTORWr mSECTOR)
        {

            try
            {
                var cSECTOR = _dbcontext.MSECTOR.Where(a => a.SECTORID == mSECTOR.SECTORID).FirstOrDefault();


                if (cSECTOR != null)
                {
                    var tPROJECTu = _dbcontext.MSECTOR.Where(b => b.SECTORID == mSECTOR.SECTORID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.INSERT_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.DESCRIPTION, mSECTOR.DESCRIPTION)
                                                          .SetProperty(upd => upd.SHORT_DESCRIPTION, mSECTOR.SHORT_DESCRIPTION)
                                                          .SetProperty(upd => upd.MODIFIED_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.HSECTORID, mSECTOR.HSECTORID)
                                                          .SetProperty(upd => upd.SCODE, mSECTOR.SCODE)
                                                          .SetProperty(upd => upd.COUNCILID, mSECTOR.COUNCILID)
                                                          .SetProperty(upd => upd.CORDINATORID, mSECTOR.CORDINATORID));

                } else
                {



                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mSECTOR.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lSECTORID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMSECTOR), _ctx);
                    var sSECTOR = new MSECTOR()
                    {

                        SECTORID = lSECTORID,
                        DESCRIPTION = mSECTOR.DESCRIPTION,
                        SCODE = mSECTOR.SCODE,
                        SHORT_DESCRIPTION = mSECTOR.SHORT_DESCRIPTION,
                        HSECTORID = mSECTOR.HSECTORID,
                        COUNCILID = mSECTOR.COUNCILID,
                        CORDINATORID = mSECTOR.CORDINATORID,
                        ISACTIVE = mSECTOR.ISACTIVE,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = mSECTOR.INSERTED_BY,
                        MODIFIED_DATE = DateTime.Now,
                        MODIFIED_BY = mSECTOR.MODIFIED_BY

                    };

                    mSECTOR.SECTORID = lSECTORID;
                    _dbcontext.MSECTOR.Add(sSECTOR);
                    var lresult = await _dbcontext.SaveChangesAsync();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Sector Created : {@model}", sSECTOR);

                    }


                }


                return mSECTOR.SECTORID;
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -1;
            }

        }


        public async Task<string> PostCity(MCITY mCITY)
        {

            try
            {
                var cSECTOR = _dbcontext.MCITY.Where(a => a.CITYID == mCITY.CITYID).FirstOrDefault();


                if (cSECTOR != null)
                {
                    var tPROJECTu = _dbcontext.MCITY.Where(b => b.CITYID == mCITY.CITYID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.INSERT_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.DESCRIPTION, mCITY.DESCRIPTION)
                                                          .SetProperty(upd => upd.CITYPE, mCITY.CITYPE)
                                                          .SetProperty(upd => upd.MODIFIED_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.PROVINCEID, mCITY.PROVINCEID)
                                                          .SetProperty(upd => upd.POSTCODE, mCITY.POSTCODE)
                                                          .SetProperty(upd => upd.NOTES, mCITY.NOTES)
                                                          .SetProperty(upd => upd.PCITYID, mCITY.PCITYID));

                }
                else
                {



                    //var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mCITY.SYSTEMID.ToString()).FirstOrDefault();
                    //var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    //var lCITYID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMSECTOR), _ctx);
                    var sCITY = new MCITY()
                    {

                        CITYID = mCITY.CITYID,
                        DESCRIPTION = mCITY.DESCRIPTION,
                        POSTCODE = mCITY.POSTCODE,
                        NOTES = mCITY.NOTES,
                        CITYPE = mCITY.CITYPE,
                        PCITYID = mCITY.PCITYID,
                        PROVINCEID = mCITY.PROVINCEID,
                        ISACTIVE = mCITY.ISACTIVE,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = mCITY.INSERTED_BY,
                        MODIFIED_DATE = DateTime.Now,
                        MODIFIED_BY = mCITY.MODIFIED_BY

                    };

                    //mCITY.CITYID = lCITYID;
                    _dbcontext.MCITY.Add(sCITY);
                    var lresult = await _dbcontext.SaveChangesAsync();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("City Created : {@model}", sCITY);

                    }


                }


                return mCITY.CITYID;
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return "nil";
            }

        }

        public async Task<int> PostUnion(MUNIONWr mUNION)
        {

            try
            {
                var cUNION = _dbcontext.MUNION.Where(a => a.UNIONID == mUNION.UNIONID).FirstOrDefault();


                if (cUNION != null)
                {
                    var tPROJECTu = _dbcontext.MUNION.Where(b => b.UNIONID == mUNION.UNIONID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.INSDT, DateTime.Now)
                                                          .SetProperty(upd => upd.MODBY, mUNION.MODBY)
                                                          .SetProperty(upd => upd.DESCRIPTION, mUNION.DESCRIPTION)
                                                          .SetProperty(upd => upd.MODAT, DateTime.Now)
                                                          .SetProperty(upd => upd.INSBY, mUNION.INSBY)
                                                          .SetProperty(upd => upd.CINDSID, mUNION.CINDSID));

                    return cUNION.UNIONID;

                }
                else
                {

                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mUNION.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lUNIONID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMUNION), _ctx);
                    var sUNION = new MUNION()
                    {
                        UNIONID = lUNIONID,
                        DESCRIPTION = mUNION.DESCRIPTION,
                        CINDSID = mUNION.CINDSID,
                        INSDT = DateTime.Now,
                        INSBY = mUNION.INSBY,
                        MODAT = DateTime.Now,
                        MODBY = mUNION.MODBY

                    };

                    _dbcontext.MUNION.Add(sUNION);
                    var lresult = _dbcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Union Created : {@model}", sUNION);
                        return lUNIONID;

                    } else return -1;


                }


               
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }

        }
        public async Task<int> PostLocality(MLOCALITYWr mLOCALITY)
        {

            try
            {
                var cUNION = _dbcontext.MLOCALITY.Where(a => a.LOCALITYID == mLOCALITY.LOCALITYID).FirstOrDefault();


                if (cUNION != null)
                {
                    var tPROJECTu = _dbcontext.MLOCALITY.Where(b => b.LOCALITYID == mLOCALITY.LOCALITYID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.DESCRIPTION, mLOCALITY.DESCRIPTION)
                                                          .SetProperty(upd => upd.MODIFIED_BY, mLOCALITY.MODIFIED_BY)
                                                          .SetProperty(upd => upd.CITYID, mLOCALITY.CITYID)
                                                          .SetProperty(upd => upd.BUILDINGNAME, mLOCALITY.BUILDINGNAME)
                                                          .SetProperty(upd => upd.BUILDINGNO, mLOCALITY.BUILDINGNO)
                                                          .SetProperty(upd => upd.STREETNAME, mLOCALITY.STREETNAME)
                                                          .SetProperty(upd => upd.STREETNO, mLOCALITY.STREETNO)
                                                          .SetProperty(upd => upd.MODIFIED_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.PROVINCEID, mLOCALITY.PROVINCEID)
                                                          .SetProperty(upd => upd.POSTALCODE, mLOCALITY.POSTALCODE)
                                                          .SetProperty(upd => upd.GEOLATI, mLOCALITY.GEOLATI)
                                                          .SetProperty(upd => upd.GEOLONG, mLOCALITY.GEOLONG)
                                                          .SetProperty(upd => upd.PLOCALITYID, mLOCALITY.PLOCALITYID)
                                                          .SetProperty(upd => upd.POPROVINCEID, mLOCALITY.POPROVINCEID)
                                                          .SetProperty(upd => upd.POPOSTALCODE, mLOCALITY.POPOSTALCODE)
                                                          .SetProperty(upd => upd.POBOXNAME, mLOCALITY.POBOXNAME)
                                                          .SetProperty(upd => upd.POCITYID, mLOCALITY.POCITYID)
                                                          .SetProperty(upd => upd.POBOXNO, mLOCALITY.POBOXNO)
                                                          .SetProperty(upd => upd.ISLOCAL, mLOCALITY.ISLOCAL)
                                                          .SetProperty(upd => upd.LOCTYPE, mLOCALITY.LOCTYPE)
                                                          .SetProperty(upd => upd.TELNUMBER, mLOCALITY.TELNUMBER)
                                                          .SetProperty(upd => upd.ADDRESSNOTE, mLOCALITY.ADDRESSNOTE));

                    return cUNION.LOCALITYID;

                }
                else
                {

                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mLOCALITY.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lLOCALITYID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMLOCALITY), _ctx);
                    var sLOCALITY = new MLOCALITY()
                    {
                        LOCALITYID = lLOCALITYID,
                        DESCRIPTION = mLOCALITY.DESCRIPTION,
                        STREETNAME = mLOCALITY.STREETNAME,
                        STREETNO = mLOCALITY.STREETNO,
                        BUILDINGNAME = mLOCALITY.BUILDINGNAME,
                        BUILDINGNO = mLOCALITY.BUILDINGNO,
                        CITYID = mLOCALITY.CITYID,
                        POSTALCODE = mLOCALITY.POSTALCODE,
                        GEOLATI = mLOCALITY.GEOLATI,
                        GEOLONG = mLOCALITY.GEOLONG,
                        TELNUMBER = mLOCALITY.TELNUMBER,
                        PROVINCEID = mLOCALITY.PROVINCEID,
                        PLOCALITYID = mLOCALITY.PLOCALITYID,
                        POPROVINCEID = mLOCALITY.POPROVINCEID,
                        POPOSTALCODE = mLOCALITY.POPOSTALCODE,
                        POBOXNAME = mLOCALITY.POBOXNAME,
                        POCITYID = mLOCALITY.POCITYID,
                        POBOXNO = mLOCALITY.POBOXNO,
                        ISLOCAL = mLOCALITY.ISLOCAL,
                        LOCTYPE = mLOCALITY.LOCTYPE,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = mLOCALITY.INSERTED_BY,
                        MODIFIED_DATE = DateTime.Now,
                        MODIFIED_BY = mLOCALITY.MODIFIED_BY

                    };

                    _dbcontext.MLOCALITY.Add(sLOCALITY);
                    var lresult = _dbcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Locality Created : {@model}", sLOCALITY);
                        return lLOCALITYID;

                    }
                    else return -1;


                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }

        }

        public async Task<int> PostPeriod(MPERIODWr mPERIOD)
        {

            try
            {
                var cPERIOD = _dbcontext.MPERIOD.Where(a => a.PRID == mPERIOD.PRID).FirstOrDefault();


                if (cPERIOD is not null)
                {
                    var tPROJECTu = _dbcontext.MPERIOD.Where(b => b.PRID == mPERIOD.PRID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.QT, mPERIOD.QT)
                                                          .SetProperty(upd => upd.PSTATUSID, mPERIOD.PSTATUSID)
                                                          .SetProperty(upd => upd.FMONTH, mPERIOD.FMONTH)
                                                          .SetProperty(upd => upd.FYEARMONTH, mPERIOD.FYEARMONTH)
                                                          .SetProperty(upd => upd.FDESCRIPTION, mPERIOD.FDESCRIPTION)
                                                          .SetProperty(upd => upd.MDESCRIPTION, mPERIOD.MDESCRIPTION)
                                                          .SetProperty(upd => upd.MIMONTH, mPERIOD.MIMONTH)
                                                          .SetProperty(upd => upd.MYEAR, mPERIOD.MYEAR)
                                                          .SetProperty(upd => upd.MYEARMONTH, mPERIOD.MYEARMONTH)
                                                          .SetProperty(upd => upd.SDATE, mPERIOD.SDATE)
                                                          .SetProperty(upd => upd.EDATE, mPERIOD.EDATE)
                                                          .SetProperty(upd => upd.FYEAR, mPERIOD.FYEAR));

                    return cPERIOD.PRID;

                }
                else
                {
                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mPERIOD.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lPERIODID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMPERIOD), _ctx);
                    var sPERIOD = new MPERIOD()
                    {
                        PRID = lPERIODID,
                        PSTATUSID = mPERIOD.PSTATUSID,
                        FMONTH = mPERIOD.FMONTH,
                        FYEARMONTH = mPERIOD.FYEARMONTH,
                        MYEARMONTH = mPERIOD.MYEARMONTH,
                        FDESCRIPTION = mPERIOD.FDESCRIPTION,
                        MDESCRIPTION = mPERIOD.MDESCRIPTION,
                        MIMONTH = mPERIOD.MIMONTH,
                        MYEAR = mPERIOD.MYEAR,
                        SDATE = mPERIOD.SDATE,
                        EDATE = mPERIOD.EDATE,
                        FYEAR = mPERIOD.FYEAR,
                        ISACTIVE = mPERIOD.ISACTIVE,
                        MMONTH = mPERIOD.MMONTH

                    };

                    _dbcontext.MPERIOD.Add(sPERIOD);
                    var lresult = _dbcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Locality Created : {@model}", sPERIOD);
                        return lPERIODID;

                    }
                    else return -1;


                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }

        }

        public async Task<int> PostCaseType(MCASETYPEWr mCASETYPE)
        {

            try
            {
                var cCASETYPE = _dbcontext.MCASETYPE.Where(a => a.CASETYPEID == mCASETYPE.CASETYPEID).FirstOrDefault();


                if (cCASETYPE is not null)
                {
                    var tPROJECTu = _dbcontext.MCASETYPE.Where(b => b.CASETYPEID == mCASETYPE.CASETYPEID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.DESCRIPTION, mCASETYPE.DESCRIPTION)
                                                          .SetProperty(upd => upd.INSBY, mCASETYPE.INSBY)
                                                          .SetProperty(upd => upd.MODBY, mCASETYPE.MODBY)
                                                          .SetProperty(upd => upd.INSDT, mCASETYPE.INSDT)
                                                          .SetProperty(upd => upd.MODAT, mCASETYPE.MODAT));

                    return cCASETYPE.CASETYPEID;

                }
                else
                {
                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mCASETYPE.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lCASETYPEID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMCASETYPE), _ctx);
                    var sCASETYPE = new MCASETYPE()
                    {
                        CASETYPEID = lCASETYPEID,
                        DESCRIPTION = mCASETYPE.DESCRIPTION,
                        INSBY = mCASETYPE.INSBY,
                        MODBY = mCASETYPE.MODBY,
                        INSDT = DateTime.Now,
                        MODAT = DateTime.Now

                    };

                    _dbcontext.MCASETYPE.Add(sCASETYPE);
                    var lresult = _dbcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Case Type Created : {@model}", sCASETYPE);
                        return lCASETYPEID;

                    }
                    else return -1;


                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }

        }
        public async Task<int> PostIntPosition(MINTPOSITIONWr mINTPOSITION)
        {

            try
            {
                var cMINTPOSITION = _dbcontext.MINTPOSITION.Where(a => a.IPOSITIONID == mINTPOSITION.IPOSITIONID).FirstOrDefault();


                if (cMINTPOSITION is not null)
                {
                    var tPROJECTu = _dbcontext.MINTPOSITION.Where(b => b.IPOSITIONID == mINTPOSITION.IPOSITIONID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.DESCRIPTION, mINTPOSITION.DESCRIPTION)
                                                          .SetProperty(upd => upd.SHRT_DESCRIPTION, mINTPOSITION.SHRT_DESCRIPTION)
                                                          .SetProperty(upd => upd.IRANKING, mINTPOSITION.IRANKING)
                                                          .SetProperty(upd => upd.INSBY, mINTPOSITION.INSBY)
                                                          .SetProperty(upd => upd.MODBY, mINTPOSITION.MODBY)
                                                          .SetProperty(upd => upd.INSDT, mINTPOSITION.INSDT)
                                                          .SetProperty(upd => upd.MODAT, mINTPOSITION.MODAT));

                    return cMINTPOSITION.IPOSITIONID;

                }
                else
                {
                    var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == mINTPOSITION.SYSTEMID.ToString()).FirstOrDefault();
                    var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                    var lINTPOSITIONID = await _general.GetSEQUENCE(nameof(IxmDBSequence.SEQMINTPOSITION), _ctx);
                    var sMINTPOSITION = new MINTPOSITION()
                    {
                        IPOSITIONID = lINTPOSITIONID,
                        DESCRIPTION = mINTPOSITION.DESCRIPTION,
                        INSBY = mINTPOSITION.INSBY,
                        MODBY = mINTPOSITION.MODBY,
                        INSDT = DateTime.Now,
                        MODAT = DateTime.Now

                    };

                    _dbcontext.MINTPOSITION.Add(sMINTPOSITION);
                    var lresult = _dbcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("Position Created : {@model}", sMINTPOSITION);
                        return lINTPOSITIONID;

                    }
                    else return -1;


                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }

        }
    }
}
