using IXM.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using IXM.Models.Store;
using IXM.Models;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IXMStoreDBContext _storecontext;
        private readonly IDataService _dataservice;
        private readonly IStoreService _storeservice;
        private readonly ILogger <OrganizerController>_logger;

        public StoreController( IXMDBContext context,
                                IXMStoreDBContext storecontext,
                                    ILogger<OrganizerController> logger,
                                    IDataService dataservice,
                                    IStoreService storeservice)
        { 
            
            _context = context;
            _storecontext = storecontext;
            _logger = logger;
            _storeservice = storeservice;

        }

        [AllowAnonymous]
        [HttpGet("Product")]
        public IActionResult GetProduct(string pProductId)
        {


            var prData = _storecontext.mProductDisplay.FromSqlRaw<mProductDisplay>(_storeservice.GetProducts(pProductId));

            return prData == null ? NotFound() : Ok(prData);
        }

        [AllowAnonymous]
        [HttpPost("Product")]
        public IActionResult PostProduct(mProduct pProduct)
        {
            if (pProduct != null)
            {
                _storecontext.mProduct.Add(pProduct);
                _storecontext.SaveChanges();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("Product")]
        public IActionResult PutProduct(mProduct pProduct)
        {
            if (pProduct != null)
            {
                _storecontext.mProduct.Entry(pProduct);
                _storecontext.SaveChanges();
            }

            return Ok();
    }

        [AllowAnonymous]
        [HttpGet("ProductCategory")]
        public IActionResult GetProductCategory(string pProductCatgoryId)
        {
            var prData = _storecontext.mProductCategory.FromSqlRaw<mProductCategory>(_storeservice.GetProductCategory(pProductCatgoryId));
            return prData == null ? NotFound() : Ok(prData);
        }

        [AllowAnonymous]
        [HttpPost("ProductCategory")]
        public IActionResult PostProductCategory(mProductCategory pProductCategory)
        {
            if (pProductCategory != null)
            {
                _storecontext.mProductCategory.Add(pProductCategory);
                _storecontext.SaveChanges();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("ProductCategory")]
        public IActionResult PutProductCategory(mProductCategory pProductCategory)
        {
            if (pProductCategory != null)
            {
                _storecontext.mProductCategory.Entry(pProductCategory);
                _storecontext.SaveChanges();
            }

            return Ok();
        }


    }
}
