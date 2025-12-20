
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using FirebirdSql.EntityFrameworkCore.Firebird;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace IXM.DB
{
    //public class IXMDBIdentity : IdentityDbContext<MUSER_AUTH>
    public class IXMDBIdentity : IdentityDbContext<ApplicationUser>
    {
        readonly string _connectionString;


        public IXMDBIdentity(DbContextOptions<IXMDBIdentity> options)
        : base(options)
        {
            _connectionString = "data source=localhost; initial catalog=C:\\myData\\IXM\\NUMW.FDB; port number=3050; user id=sysdba; dialect=3;isolationlevel=Snapshot; pooling=True;password=Inch@123;providerName=FirebirdSql.Data.FirebirdClient";
        }


        public DbSet<MUSER_DEVICE> MUSER_DEVICE { get; set; }
        public DbSet<CstmUserRole> CstmUserRoles { get; set; }
        public DbSet<MUSER_ROLE> MUSER_ROLE { get; set; }
        public DbSet<MUSER_SYSTEM> MUSER_SYSTEM { get; set; }
        public DbSet<MUNION_SYSTEM> MUNION_SYSTEM { get; set; }
        public DbSet<MSYSTEM> MSYSTEM { get; set; }
        public DbSet<UserPasswordResets> UserPasswordResets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity<MUSER_ROLE>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<UserPasswordResets>().HasKey(table => new
            {
                table.UserId,
                table.PasswordResetExpiryDate
            } );

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

    }
}
