using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Data.ApplicationUser
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<OtpVerification> OtpVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<OtpVerification>()
                .HasIndex(o => o.Token)
                .IsUnique();

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed permissions
            modelBuilder.Entity<Permission>().HasData(
                // Cartes
                new Permission { Id = 1, Code = "carte.create",  Label = "Créer une carte",       Category = "Cartes" },
                new Permission { Id = 2, Code = "carte.list",    Label = "Voir la liste",           Category = "Cartes" },
                new Permission { Id = 3, Code = "carte.confirm", Label = "Confirmer OTP",           Category = "Cartes" },
                new Permission { Id = 4,  Code = "carte.edit",       Label = "Modifier une carte",         Category = "Cartes" },
                new Permission { Id = 10, Code = "carte.activer",   Label = "Activer une carte",          Category = "Cartes" },
                new Permission { Id = 11, Code = "carte.desactiver", Label = "Désactiver une carte",       Category = "Cartes" },
                // Paramétrage
                new Permission { Id = 6, Code = "parametrage.view", Label = "Voir paramétrage",    Category = "Paramétrage" },
                new Permission { Id = 7, Code = "parametrage.edit", Label = "Modifier paramétrage", Category = "Paramétrage" },
                // Administration
                new Permission { Id = 8, Code = "admin.users",  Label = "Gérer utilisateurs",      Category = "Administration" },
                new Permission { Id = 9, Code = "admin.roles",  Label = "Gérer rôles et permissions", Category = "Administration" }
            );
        }
    }
}
