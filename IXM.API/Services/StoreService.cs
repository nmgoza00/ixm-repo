using IXM.API.Controllers;
using IXM.DB;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using static IXM.DB.QueryRepository;
using IXM.API.Resources;
using System.Data.Entity.Core.Metadata.Edm;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity;
using System.Collections.Specialized;
using System.Text.Json;
using System.Text;
using System;
using System.Buffers;


namespace IXM.API.Services
{
    public class StoreService : IStoreService
    {
        private readonly IXMDBContext _context;
        private readonly IXMStoreDBContext _storecontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        //private readonly IQueryRepository _queryRepository;

        public StoreService(IXMDBContext context, IXMStoreDBContext storecontext, IXMDBIdentity idtcontext, IConfiguration configuration)
        {
            _context = context;
            _storecontext = storecontext;
            _identitycontext = idtcontext;
            _configuration = configuration;
            //_queryRepository = queryRepository;
        }



        public string GetProducts(string pProductId)
        {

            var lSql = $" SELECT a.\"ProductId\", a.\"Description\", a.\"ProductCategoryId\", a.\"InsertDate\", a.\"InsertedBy\", a.\"ModifiedDate\", a.\"ModifiedBy\", a.\"IsActive\",case when c.\"Price\" is null then 1 else c.\"Price\" end \"ProductPrice\" from \"mProduct\" a " +
                        " LEFT JOIN \"mProductPrice\" c " +
                        " ON a.\"ProductId\" = c.\"ProductId\"";
            return lSql;

        }


        public string GetProductCategory(string pProductCategoryId)
        {

            var lSql = " SELECT * from \"mProductCategory\"";
            return lSql;

        }


    }
}
