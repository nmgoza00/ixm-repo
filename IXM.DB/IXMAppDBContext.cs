
using Microsoft.EntityFrameworkCore;
using FirebirdSql.EntityFrameworkCore.Firebird;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IXM.Models;

namespace IXM.DB
{
    public class IXMAppDBContext : DbContext
    {
        readonly string _connectionString;
        //public readonly string _basedocfolder;
        public IConfiguration _configuration;



        public IXMAppDBContext(IConfiguration configuration, DbContextOptions<IXMAppDBContext> options)
        : base(options)
        //public IXMDBContext(IConfiguration configuration)
        {
            //_connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            //_basedocfolder = configuration.GetConnectionString("BaseDocFolder");
            _configuration = configuration;
        }

        public DbSet<APPDOMAINLICENSE> APPDOMAINLICENSE { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
