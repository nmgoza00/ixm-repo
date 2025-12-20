using IXM.DB;
using IXM.API.Services;
using IXM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {

        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;


        public TransfersController(IXMDBContext context,
                                    IDataService dataservice)
        {
            _context = context;
            _dataservice = dataservice;
        }



        [HttpGet]
        public IActionResult Get(string pTrId)
        {

            var prData = _context.MMTR.FromSqlRaw<MMTR>($"SELECT FIRST 250 m.*" +
                $" FROM MMTR m WHERE m.MMTRID = " +pTrId);                //.Take(1000).ToList();
            return prData == null ? NotFound() : Ok(prData);

            //return await _context.Company.ToListAsync();
        }

    }
}
