
using IXM.Models;
using IXM.Models.Events;
using IXM.Models.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IXM.DB
{
    public class IXMSystemWrDBContext : DbContext
    {
        readonly string _connectionString;
        public IConfiguration _configuration;


        public IXMSystemWrDBContext(IConfiguration configuration, DbContextOptions<IXMSystemWrDBContext> options)
        : base(options)
        {
            _connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.Store.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            _configuration = configuration;
        }

        public DbSet<TPROJECTWr> TPROJECT { get; set; }
        public DbSet<TEVENTWr> TEVENT { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<TPROJECTWr>(b =>
            {

                b.HasKey(x => new { x.PRJID, x.SYSTEMID });

            });

            modelBuilder.Entity<TEVENTWr>(b =>
            {

                b.HasKey(x => new { x.EVTID, x.SYSTEMID });

            });

        }


    }
}
