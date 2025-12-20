
using IXM.Models;
using IXM.Models.Core;
using IXM.Models.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IXM.DB
{
    public class IXMWriteDBContext : DbContext
    {
        readonly string _connectionString;
        //public readonly string _basedocfolder;
        public IConfiguration _configuration;



        public IXMWriteDBContext(IConfiguration configuration, DbContextOptions<IXMWriteDBContext> options)
        : base(options)
        //public IXMDBContext(IConfiguration configuration)
        {
            //_connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            //_basedocfolder = configuration.GetConnectionString("BaseDocFolder");
            _configuration = configuration;
        }

        public DbSet<APPDOMAINLICENSE> APPDOMAINLICENSE { get; set; }
        public DbSet<TRMBLD_WRITE> TRMBLD { get; set; }
        public DbSet<TRMBL_WRITE> TRMBL { get; set; }
        public DbSet<TPROJECTWr> TPROJECT { get; set; }
        public DbSet<MCOMPANY_WRITE> MCOMPANY { get; set; }
        public DbSet<MEMPLOYEE_WRITE> MEMPLOYEE { get; set; }
        public DbSet<CUSER_GROUP_WRITE> CUSER_GROUP { get; set; }
        public DbSet<TTJRN> TTJRNL { get; set; }
        public DbSet<TTRAN> TTRAN { get; set; }
        public DbSet<MUSER> MUSER { get; set; }
        public DbSet<MUSER_GROUP> MUSER_GROUP { get; set; }
        public DbSet<CCUR> CCUR { get; set; }
        public DbSet<SEQUENCE> SEQUENCE { get; set; }
        public DbSet<VMSTATUS> VMSTATUS { get; set; }
        public DbSet<TCASE_WRITE> TCASE { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CCUR>().HasKey(r => new { r.COMPANYID, r.SRCOBJ, r.SRCID });
        }

    }
}
