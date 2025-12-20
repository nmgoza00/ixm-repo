using IXM.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using IXM.Models.Core;
using IXM.Models;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LegalController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IIXMDBRepo _dataservice;
        private readonly ILogger <LegalController>_logger;

        public LegalController( IXMDBContext context,
                                    ILogger<LegalController> logger,
                                    IIXMDBRepo dataservice)
        { 
            
            _context = context;
            _logger = logger;
            _dataservice = dataservice;

        }

        [AllowAnonymous]
        [HttpGet("Cases")]
        public async Task<IActionResult> GetCases()
        {

            var lg = await _dataservice.Legal.GetCases();

            return Ok(lg);

        }



        [HttpPost("Cases")]
        public async Task<IActionResult> PostLegalCase(TCASE_WRITE LegalCase)
        {

            try
            {
                int lresult = await _dataservice.Legal.PostCase(LegalCase, LegalCase.INSBY);

           
                if (lresult == 0)
                {
                    return Ok();

                }
                else
                {

                    return NotFound();

                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }




    }
}
