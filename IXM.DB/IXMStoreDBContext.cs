
using Microsoft.EntityFrameworkCore;
using FirebirdSql.EntityFrameworkCore.Firebird;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IXM.Models.Store;
using Microsoft.Extensions.Configuration;
using IXM.Models;

namespace IXM.DB
{
    public class IXMStoreDBContext : DbContext
    {
        readonly string _connectionString;
        public IConfiguration _configuration;


        public IXMStoreDBContext(IConfiguration configuration, DbContextOptions<IXMStoreDBContext> options)
        : base(options)
        {
            _connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.Store.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            _configuration = configuration;
        }

        public DbSet<mProduct> mProduct { get; set; }
        public DbSet<mProductDisplay> mProductDisplay { get; set; }
        public DbSet<mProductCategory> mProductCategory { get; set; }
        public DbSet<cSystemProduct> cSystemProduct { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
