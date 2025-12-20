
using IXM.Models;
using IXM.Models.Events;
using IXM.Models.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IXM.DB
{
    public class IXMSystemDBContext : DbContext
    {
        readonly string _connectionString;
        public IConfiguration _configuration;


        public IXMSystemDBContext(IConfiguration configuration, DbContextOptions<IXMSystemDBContext> options)
        : base(options)
        {
            _connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.Store.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            _configuration = configuration;
        }

        public DbSet<MSTATUS> MSTATUS { get; set; }
        public DbSet<MSTATUS_TEXT> MSTATUS_TEXT { get; set; }
        public DbSet<TPROJECT> TPROJECT { get; set; }
        public DbSet<TEVENT> TEVENT { get; set; }
        public DbSet<TPRJEVT> TPRJEVT { get; set; }
        public DbSet<SEQUENCE> SEQUENCE { get; set; }
        public DbSet<TPRJEVTD> TPRJEVTD { get; set; }
        public DbSet<TPRJEVTDD> TPRJEVTDD { get; set; }
        public DbSet<TPRJEVTATTACHMENT> TPRJEVTATTACHMENT { get; set; }
        public DbSet<TPRJEVT_DISPLAY> TPRJEVT_DISPLAY { get; set; }
        public DbSet<TPRJEVTD_DISPLAY> TPRJEVTD_DISPLAY { get; set; }
        public DbSet<TPRJEVTS> TPRJEVTS { get; set; }
        public DbSet<MEVENT_COMPONENT> MEVENT_COMPONENT { get; set; }
        public DbSet<CEVT_COMPONENT> CEVT_COMPONENT { get; set; }
        public DbSet<VCEVT_COMPONENT> VCEVT_COMPONENT { get; set; }
        public DbSet<REP_EVTSTATS_S> REP_EVTSTATS_S { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<MUSER_ROLE>().HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<TPROJECT>(b =>
            {

                b.HasKey(x => new { x.PRJID, x.SYSTEMID });

            });

            modelBuilder.Entity<TPRJEVT>(b =>
            {
                b.HasKey(x => x.PEVTID);
                b.HasIndex(x => x.PSTDAT);
                b.Property(x => x.PAGEHTML).HasColumnType("blob sub_type text"); // Firebird preference; adapt if using SQL Server
            });

            modelBuilder.Entity<TPRJEVTATTACHMENT>(b =>
            {
                b.HasKey(x => x.EVTATTACHMENTID);
            });
        }


    }
}
