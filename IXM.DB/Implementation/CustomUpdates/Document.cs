using IXM.Models;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IXM.GeneralSQL;
using IXM.Constants;

namespace IXM.DB
{
    public class Document : IDocument
    {

        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IGeneralSQL _generalSQL;
        private readonly IIXMDBRepo _dbrepo;
        private readonly IGeneral _general;
        private readonly ILogger<Document> _logger;
        private readonly IConfiguration _configuration;
        private readonly IXMDBContextFactory _dbfactory;
        private readonly UserManager<ApplicationUser> _usermanager;

        public Document(IXMDBContext context, 
                             IXMDBIdentity idtcontext,
                             IXMDBContextFactory dbfactory,
                             UserManager<ApplicationUser> usermanager,
                             IIXMDBRepo dbrepo,
                             IGeneralSQL generalSQL,
                             IConfiguration configuration,
                             ILogger<Document> logger)
        {
            _context = context;
            _identitycontext = idtcontext;
            _usermanager = usermanager;
            _configuration = configuration;
            _dbfactory = dbfactory;
            _generalSQL = generalSQL;
            _dbrepo = dbrepo;
            _logger = logger;
            _general = new General(_context, null, null, null);
        }

        public async Task<Tuple<int, string>> DocumentTransfer(ILogger logger, MOBJECT_DOC model, int FromID, string pSystemName)
        {
            try
            {
                var _ctx = _dbfactory.Create("Firebird" + pSystemName);

                var mOBJECT_DOC = _ctx.MOBJECT_DOC.Where(b => b.SOURCEOBJ == model.SOURCEOBJ)
                                                      .Where(b => b.SOURCEFLD == model.SOURCEFLD)
                                                      .Where(b => b.SOURCEID == model.SOURCEID)
                                                      .Where(b => b.DOCTYPE == model.DOCTYPE).ToList();


                mOBJECT_DOC.ForEach(b =>
                {
                    b.LT = 0;
                    b.EFFDATE = DateTime.Now;
                    _ctx.Update(b);
                });


                _logger.LogInformation("Data extracted from Context {@model}", model);

                model.OBJECTID = await _dbrepo.General.GetSEQUENCE(nameof(IxmDBSequence.SEQMOBJECT_DOC), _ctx);
                model.EFFDATE = DateTime.Now;
                model.DFE = 2;

                _logger.LogInformation("Data extracted from Context {@model}", model);

                model.INSERT_DATE = DateTime.Now;
                model.LT = 1;

                _ctx.MOBJECT_DOC.Add(model);
                _ctx.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Tuple<int, string>(-1, ex.Message);

            }

            return new Tuple<int, string>(model.OBJECTID, "Loaded");

        }

        public Tuple<int, string> AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model)
        {

            try
            {

                model.INSERT_DATE = DateTime.Now;
                model.LT = 1;
                var mOBJECT_DOC = _context.MOBJECT_DOC.Where(b => b.SOURCEOBJ == model.SOURCEOBJ)
                                                      .Where(b => b.SOURCEFLD == model.SOURCEFLD)
                                                      .Where(b => b.SOURCEID == model.SOURCEID)
                                                      .Where(b => b.DOCTYPE == model.DOCTYPE).ToList();



                _logger.LogInformation("Data extracted from Context {@model}", mOBJECT_DOC);

                mOBJECT_DOC.ForEach(b =>
                {
                    b.LT = 0;
                    b.EFFDATE = DateTime.Now;
                    _context.Update(b);
                });


                model.OBJECTID = _dbrepo.General.GetSEQUENCE(nameof(IxmDBSequence.SEQMOBJECT_DOC));

                _logger.LogInformation("Data to Load {@model}", model);
                _context.MOBJECT_DOC.Add(model);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Tuple<int,string>(-1, ex.Message);

            }

            return new Tuple<int, string>(model.OBJECTID,"Loaded");
        }


        public int EditOrganizerImage(MOBJECT_DOC model)
        {

            try
            {
                _context.MOBJECT_DOC.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }

            return model.OBJECTID;
        }




    }


}
