using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
namespace XeniaRentalApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<AccountGroups> AccountGroups { get; set; }

        public DbSet<Accounts> Accounts { get; set; }

        public DbSet<BedSpace> BedSpaces { get; set; }

        public DbSet<BedSpaceAssignemnt> BedSpaceAssignemnt { get; set; }

        public DbSet<BedSpacePlan> BedSpacePlans { get; set; }

        public DbSet<Charges> Charges { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<Documents> Documents { get; set; }

        public DbSet<Ledger> Ledgers { get; set; }

        public DbSet<MessDetails> MessDetails { get; set; }

        public DbSet<MessTypes> MessTypes { get; set; }

        public DbSet<Properties> Properties { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantAssignemnt> TenantAssignemnts { get; set; }

        public DbSet<TenantDocuments> TenantDocuments { get; set; }

        public DbSet<Units> Units { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<UnitChargesMapping> UnitChargesMappings { get; set; }

        public DbSet<TblOTPLog> tblOTPLogs { get; set; }

        public DbSet<TblNotification> tblNotifications { get; set; }

        public DbSet<TblEmailSmsSettings> tblEmailSmsSettings {  get; set; }

        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>()
          .HasOne(u => u.UserRole)
          .WithMany()
          .HasForeignKey(u => u.UserType)
          .HasPrincipalKey(r => r.UserRoleId);



        }



    }

}

