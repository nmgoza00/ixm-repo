
using Microsoft.EntityFrameworkCore;
using FirebirdSql.EntityFrameworkCore.Firebird;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IXM.Models;
using DevExpress.Xpo.Logger;

namespace IXM.DB
{
    public class IXMAppLogContext : DbContext
    {
        readonly string _connectionString;
        //public readonly string _basedocfolder;
        public IConfiguration _configuration;



        public IXMAppLogContext(IConfiguration configuration, DbContextOptions<IXMAppLogContext> options)
        : base(options)
        {
            //_connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\IXM.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
            //_basedocfolder = configuration.GetConnectionString("BaseDocFolder");
            _configuration = configuration;
        }

        public DbSet<APP_LOGTABLE> APP_LOGTABLE { get; set; }
        public DbSet<APP_LOGTABLEI> APP_LOGTABLEI { get; set; }
        public DbSet<APP_LOGTABLEE> APP_LOGTABLEE { get; set; }
        public DbSet<APP_LOGTABLEF> APP_LOGTABLEF { get; set; }
        public DbSet<APP_LOGTABLER> APP_LOGTABLER { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APP_LOGTABLE>().HasNoKey();
            modelBuilder.Entity<APP_LOGTABLEE>().HasNoKey();
            modelBuilder.Entity<APP_LOGTABLEI>().HasNoKey();
            modelBuilder.Entity<APP_LOGTABLEF>().HasNoKey();
            modelBuilder.Entity<APP_LOGTABLER>().HasNoKey();
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

    }
}
