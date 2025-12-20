using DocumentFormat.OpenXml.Vml;
using IXM.Common.Constant;
using IXM.Constants;
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace IXM.DB
{
    public class Member : IMember
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly IGeneral _general;
        private readonly IMasterData _masterData;
        private readonly ILogger<Member> _logger;

        public GenFunctions genFunctions = new GenFunctions();

        public Member( 
                            IXMDBIdentity idtcontext,
                            IXMDBContext dbcontext,
                            IMemoryCache memoryCache,
                            IConfiguration configuration, 
                            ILogger<Member> logger)
        {
            _dbcontext = dbcontext;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _masterData = new MasterData(null, null, _dbcontext, memoryCache, null);
            _general = new General(_dbcontext, null, null, null);
        }

        
        public MMEMBER GetMemberByGuid(ApplicationUser au, string _Guid)
        {
            try
            {
                MMEMBER mmember = new MMEMBER();

                var _user = _dbcontext.MUSER.Where(a => a.AUTHCODE == _Guid).First();

                if (_user.MEMBERID != null)
                {
                    mmember = _dbcontext.MMEMBER.Where(a => a.MEMBERID == _user.MEMBERID).Single();
                    return mmember;
                } return null;


            }
            catch (Exception)
            {

                return null;
            }

        }


        public async Task<MEMBER_MASTERDATA> GetMember_MASTERDATA()
        {


            var member = new MEMBER_MASTERDATA();

            member.MINTPOSITION = await _masterData.GetIntPosition();
            //member.VMSTATUS = await _masterData.GetStatusText();
            member.MSECTOR = await _masterData.GetSector();
            //member.MEMPLOYEE = await _masterData.GetEmployeeType();

            return member ?? new MEMBER_MASTERDATA();

        }

        public List<MMEMBER> GetMemberByCompanyGuid(ApplicationUser au, string _Guid)
        {
            try
            {
                //ListMMEMBER mmember = new MMEMBER();

                var _user = _dbcontext.MCOMPANY.Where(a => a.HGUID == _Guid).Select(b => new { b.COMPANYID }).First();

                if (_user.COMPANYID != null)
                {
                    var mmember = _dbcontext.MMEMBER.Where(a => a.COMPANYID == _user.COMPANYID).ToList();
                                                //.Where(a => a.MEMSTATUSID).Single();
                    return mmember;
                }
                return null;


            }
            catch (Exception)
            {

                return null;
            }

        }


        public bool ModifyMemberCard(MMCDD mMMCDD)
        {

            var mMEMBERl = _dbcontext.MMCDD.Where(b => b.MEMBERID == mMMCDD.MEMBERID)
                           .ExecuteUpdate(up => up.SetProperty(upd => upd.LT, 0));


            if (mMEMBERl == 0)
            {
                var mmcdd = new MMCDD()
                {

                    CDDID = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQ_TMCDD)),
                    PNAME = mMMCDD.PNAME,
                    PSURNAME = mMMCDD.PSURNAME,
                    INSDAT = DateTime.Now,
                    LT = 1
                };
                _dbcontext.MMCDD.AddAsync(mmcdd);

            }
            else
            {

                var _dset = _dbcontext.MMCDD.Where(a => a.MEMBERID == mMMCDD.MEMBERID)
                                .OrderByDescending(b => b.MEMBERID).FirstOrDefault();
                //SetProperty(upd => upd.LT, 0));
                if (mMMCDD.WI == "Y")
                {
                    _dset.WI = mMMCDD.WI;
                    _dbcontext.MMCDD.Update(_dset);
                }

            }
            ;

            return false;
        }


        public Tuple<int, string> MemberUpsert(MMEMBER mMEMBER)
        {

            Tuple<int, string> _regresult = new Tuple<int, string>(-1, "Init");

            if (mMEMBER.MEMBERID == -1)
            {
                var lVal = InsertMember(mMEMBER);
                _regresult = new Tuple<int, string>(lVal, "Member Inserted Successfully");

            }


            else if (mMEMBER.MEMBERID > 0)
            {
                ModifyMember(mMEMBER);
                _regresult = new Tuple<int, string>(mMEMBER.MEMBERID, "Member Updated Successfully");

            }


            return _regresult;
        }

        public bool ModifyMember(MMEMBER mMEMBER)
        {

            // Get Definitions
            var toUpdate = new[] { "MNAME", "MSURNAME", "INITIALS", "CELLNUMBER", "EMPNUMBER", "DOB", "SALARY", "MEMSTATUSID", "LGLSTATUSID", "RUNSTATUSID", "RECRUITERID", "MAGE", "GENDER", "UNIONID", "BCOMPANYID", "MODIFIED_BY", "MODIFIED_DATE" };
            Type type = typeof(MMEMBER);
            PropertyInfo[] properties = type.GetProperties();

            // Arrange objects to modify
            _dbcontext.Entry(mMEMBER).State = EntityState.Modified;
            var entry = _dbcontext.Entry(mMEMBER);

            //Set Update Properties
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    entry.Property(property.Name).IsModified = false;
                }
                catch (Exception e)
                {

                    _logger.LogInformation("{@1}", e.Message);
                }


            }
            foreach (var pname in toUpdate)
            {
                entry.Property(pname).IsModified = true;
            }

            _dbcontext.SaveChanges();
            //ModifyMemberCard(logger, mMEMBER);


            //var companyId = company.CompanyId;

            return false;
        }


        public int InsertMember(MMEMBER mMEMBER)
        {
            //if (pid != company.MEMBERID) return BadRequest();
            mMEMBER.MEMBERID = _general.GetSEQUENCE("SEQMEMBER");

            // Get Definitions
            var toUpdate = new[] { "MNAME", "MSURNAME", "CELLNUMBER", "EMPNUMBER", "SALARY" };
            //Type type = typeof(MMEMBER_C);
            //PropertyInfo[] properties = type.GetProperties();


            _dbcontext.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_APP_INSERT_MEMBER({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                mMEMBER.MEMBERID,
                mMEMBER.MNAME,
                mMEMBER.MSURNAME,
                mMEMBER.EMPNUMBER,
                mMEMBER.IDNUMBER,
                mMEMBER.COMPANYID,
                mMEMBER.BCOMPANYID,
                mMEMBER.CITYID,
                mMEMBER.INSERTED_BY);
            // Arrange objects to modify
            //_context.Entry(mMEMBER).State = EntityState.Added;
            //var entry = _context.Entry(mMEMBER);
            //var entry = _context.MMEMBER.Add(mMEMBER);


            _dbcontext.SaveChanges();
            //ModifyMemberCard(logger, mMEMBER);


            //var companyId = company.CompanyId;

            return mMEMBER.MEMBERID;
        }



    }
}
