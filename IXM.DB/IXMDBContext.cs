
using IXM.Models;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace IXM.DB
{
    public class IXMDBContext : DbContext
    {
        private string _connectionString;
        //public readonly string _basedocfolder;
        //public IConfiguration _configuration;


        public IXMDBContext(DbContextOptions<IXMDBContext> options)
        : base(options)
        //public IXMDBContext(IConfiguration configuration)
        {
            //_connectionString = connectionString;
            var relationalOptions = options.Extensions
                .OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>()
                .FirstOrDefault();

            if (relationalOptions != null)
            {
                //Debug.WriteLine($"🔍 ConnectionString: {relationalOptions.ConnectionString}");
            }

            //_connectionString = connectionString;
        }


        public DbSet<MCOMPTYPE> MCOMPTYPE { get; set; }
        public DbSet<MCOMPANY> MCOMPANY { get; set; }
        public DbSet<MMTR> MMTR { get; set; }
        public DbSet<MCOMPANY_CCUR> MCOMPANY_CCUR { get; set; }
        public DbSet<ORGANISER_COMPANY> ORGANISER_COMPANY { get; set; }
        public DbSet<USER_COMPANY> USER_COMPANY { get; set; }
        public DbSet<MUSER> MUSER { get; set; }
        public DbSet<TPAYMENT> TPAYMENT { get; set; }
        public DbSet<TPAYMENT_READ> TPAYMENT_READ { get; set; }
        public DbSet<TPAYMENT_DET> TPAYMENT_DET { get; set; }
        public DbSet<TINVOICE> TINVOICE { get; set; }
        public DbSet<TINVOICE_DET> TINVOICE_DET { get; set; }
        public DbSet<TMEMPAYMENT> TMEMPAYMENT { get; set; }
        public DbSet<MEMPTYPE> MEMPTYPE { get; set; }
        public DbSet<TRMBL> TRMBL { get; set; }
        public DbSet<TRMBLD> TRMBLD { get; set; }
        public DbSet<TRMBLE> TRMBLE { get; set; }
        public DbSet<TTJRN> TTJRN { get; set; }
        public DbSet<TTRAN> TTRAN { get; set; }
        public DbSet<MMEMBER> MMEMBER { get; set; }
        public DbSet<MINTPOSITION> MINTPOSITION { get; set; }
        public DbSet<MEXTPOSITION> MEXTPOSITION { get; set; }
        public DbSet<MEMPLOYEE> MEMPLOYEE { get; set; }
        public DbSet<MEMPLOYEE_DISPLAY> MEMPLOYEE_DISPLAY { get; set; }
        public DbSet<TMB_NOTICE> TMB_NOTICE { get; set; }
        public DbSet<TMB_NOTICE_AUDIANCE> TMB_NOTICE_AUDIANCE { get; set; }
        public DbSet<MMEMBER_NOT> MMEMBER_NOT { get; set; }
        public DbSet<MMEMBER_ADD> MMEMBER_ADD { get; set; }
        public DbSet<MMEMBER_BANK> MMEMBER_BANK { get; set; }
        public DbSet<MMCDD> MMCDD { get; set; }
        public DbSet<MCODE> MCODE { get; set; }
        public DbSet<MREPORT> MREPORT { get; set; }
        public DbSet<TCAPTIONS> TCAPTIONS { get; set; }
        public DbSet<CLOCALITY> CLOCALITY { get; set; }
        public DbSet<MMEMBER_NONPAYMENT> MMEMBER_NONPAYMENT { get; set; }
        public DbSet<MMEMBER_READ> MMEMBER_READ { get; set; }
        public DbSet<IXMChartData> IXMChartData { get; set; }
        public DbSet<TSURVEYMEMBERDATA> TSURVEYMEMBERDATA { get; set; }
        public DbSet<TDSURVEY_C> TDSURVEY_C { get; set; }
        public DbSet<MSECTOR> MSECTOR { get; set; }
        public DbSet<MUNION> MUNION { get; set; }
        public DbSet<MLOCALITY> MLOCALITY { get; set; }
        public DbSet<MPERIOD> MPERIOD { get; set; }
        public DbSet<MSTATUS> MSTATUS { get; set; }
        public DbSet<VMSTATUS> VMSTATUS { get; set; }
        public DbSet<MPROCESSOR> MPROCESSOR { get; set; }
        public DbSet<MSTATUS_TEXT> MSTATUS_TEXT { get; set; }
        public DbSet<MCITY> MCITY { get; set; }
        public DbSet<MBANKNAME> MBANKNAME { get; set; }
        public DbSet<MPROVINCE> MPROVINCE { get; set; }
        public DbSet<STATSREGION> STATSREGION { get; set; }
        public DbSet<STATSBRANCH> STATSBRANCH { get; set; }
        public DbSet<MOBJECT_NOT> MOBJECT_NOT { get; set; }
        public DbSet<MOBJECT_DOC> MOBJECT_DOC { get; set; }
        public DbSet<MOBJECT_DOC_API> MOBJECT_DOC_API { get; set; }
        public DbSet<MOBJECT_DOCFILE> MOBJECT_DOCFILE { get; set; }
        public DbSet<MUSER_GROUP> MUSER_GROUP { get; set; }
        public DbSet<CUSER_GROUP> CUSER_GROUP { get; set; }
        public DbSet<APP_HARDCODES> APP_HARDCODES { get; set; }
        public DbSet<SEQUENCE> SEQUENCE { get; set; }
        public DbSet<MUSER_ROLE> MUSER_ROLE { get; set; }
        public DbSet<DATA_VALUEMAPPING> DATA_VALUEMAPPING { get; set; }
        public DbSet<SSMS_DATA> SSMS_DATA { get; set; }
        public DbSet<MMSG> MMSG { get; set; }
        public DbSet<CCUR> CCUR { get; set; }


        public DbSet<REP_PROCESSORWORK> REP_PROCESSORWORK { get; set; }
        public DbSet<REP_PROCESSOR_LOADEDSCHEDULES> REP_PROCESSOR_LOADEDSCHEDULES { get; set; }
        public DbSet<REP_DEPT_LOADEDSCHEDULES> REP_DEPT_LOADEDSCHEDULES { get; set; }
        public DbSet<LIST_MONTHLYREPORTS> LIST_MONTHLYREPORTS { get; set; }


        public DbSet<MUSER_MENU> MUSER_MENU { get; set; }
        public DbSet<TCOUNT> TCOUNT { get; set; }
        public DbSet<TCASE> TCASE { get; set; }
        public DbSet<MCASETYPE> MCASETYPE { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                Debug.WriteLine("🚫 DbContext is not configured.");

                optionsBuilder.UseFirebird(_connectionString);
            }
            else
            {
                Debug.WriteLine("✅ DbContext is configured.");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            /*var connectionString = this.Database.GetDbConnection().ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("⚠️ Connection string is not initialized!");
            }*/
            var provider = this.Database.ProviderName;
            Debug.WriteLine($"🔧 Model being created for provider: {provider}");

            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity<TCOUNT>().HasNoKey();
            modelBuilder.Entity<MUSER_ROLE>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<SSMS_DATA>().HasKey(r => new { r.SSID});
            modelBuilder.Entity<CCUR>().HasKey(r => new { r.COMPANYID, r.SRCOBJ, r.SRCID });
            modelBuilder.Entity<IXMChartData>().HasNoKey();
            modelBuilder.Entity<MPROCESSOR>().HasNoKey();
            modelBuilder.Entity<TSURVEYMEMBERDATA>().HasNoKey();
            modelBuilder.Entity<STATSREGION>().HasNoKey();
            modelBuilder.Entity<DATA_VALUEMAPPING>().HasKey(o => new { o.DHCODE, o.SVALUE, o.TVALUE });


            modelBuilder.Entity<MMEMBER>().Ignore(p => p.DBCONTRIBUTION).Ignore(p => p.MemCardReceived);

    //var connection = this.Database.GetDbConnection();
    //Debug.WriteLine($"💡 Using DB: {connection.Database} on {connection.DataSource}");



        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException != null &&
                   ex.InnerException.Message.Contains("violation of UNIQUE KEY", StringComparison.OrdinalIgnoreCase);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new InvalidOperationException("Duplicate record detected. The entity already exists.", ex);
            }
            catch
            {
                throw;
            }
        }


        public void ChangeConnection(string connectionString)
        {
            _connectionString = connectionString;

            // Reset internal state if needed
            this.Database.GetDbConnection().ConnectionString = connectionString;
        }


    }
}
