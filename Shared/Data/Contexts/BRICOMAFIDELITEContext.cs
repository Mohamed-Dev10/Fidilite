using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BRICOMA.ECOMMERCE.Data.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Contexts
{
    public partial class BRICOMAFIDELITEContext : DbContext
    {
        public BRICOMAFIDELITEContext()
        {
        }

        public BRICOMAFIDELITEContext(DbContextOptions<BRICOMAFIDELITEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnniversaireSuivi> AnniversaireSuivi { get; set; }
        public virtual DbSet<AppFile> AppFile { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClienteAudit> ClienteAudit { get; set; }
        public virtual DbSet<ClienteEvent> ClienteEvent { get; set; }
        public virtual DbSet<MapClienteAppFile> MapClienteAppFile { get; set; }
        public virtual DbSet<Profil> Profil { get; set; }
        public virtual DbSet<RefAnniversaireCarte> RefAnniversaireCarte { get; set; }
        public virtual DbSet<RefCarteType> RefCarteType { get; set; }
        public virtual DbSet<RefClienteStatut> RefClienteStatut { get; set; }
        public virtual DbSet<RefClienteType> RefClienteType { get; set; }
        public virtual DbSet<RefGenre> RefGenre { get; set; }
        public virtual DbSet<RefMagasin> RefMagasin { get; set; }
        public virtual DbSet<RefMetier> RefMetier { get; set; }
        public virtual DbSet<RefVille> RefVille { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=SRV-REPORTING;Initial Catalog=BRICOMA.FIDELITE;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnniversaireSuivi>(entity =>
            {
                entity.Property(e => e.CaNetHt)
                    .HasColumnName("CA_NET_HT")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CaNetTtc)
                    .HasColumnName("CA_NET_TTC")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Carte).HasColumnName("CARTE");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.DateConsultation).HasColumnType("datetime");

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.DateCreationSuivi).HasColumnType("datetime");

                entity.Property(e => e.DateEnvoi).HasColumnType("datetime");

                entity.Property(e => e.DateNaissance).HasColumnType("datetime");
            });

            modelBuilder.Entity<AppFile>(entity =>
            {
                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.DateModification).HasColumnType("datetime");

                entity.Property(e => e.NomPrenom).HasMaxLength(255);

                entity.Property(e => e.ProfilId)
                    .HasColumnName("Profil_Id")
                    .HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Profil)
                    .WithMany(p => p.AppFileProfil)
                    .HasForeignKey(d => d.ProfilId)
                    .HasConstraintName("FK_AppFile_ToProfil");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.AppFileUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_AppFile_ToUpdatedProfil");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ_Cliente_Email")
                    .IsUnique();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.DateConfirmation).HasColumnType("datetime");

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.DateDeactivation).HasColumnType("datetime");

                entity.Property(e => e.DateNaissance).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gsm).IsRequired();

                entity.Property(e => e.Nia)
                    .HasColumnName("NIA")
                    .HasColumnType("decimal(20, 0)");

                entity.Property(e => e.Nom).IsRequired();

                entity.Property(e => e.Prenom).IsRequired();

                entity.Property(e => e.ProfilDeactivation).HasMaxLength(450);

                entity.Property(e => e.RefCarteTypeId).HasColumnName("REF_CarteType_Id");

                entity.Property(e => e.RefClienteStatutId).HasColumnName("REF_ClienteStatut_Id");

                entity.Property(e => e.RefGenreId).HasColumnName("REF_Genre_Id");

                entity.Property(e => e.RefMagasinId).HasColumnName("REF_Magasin_Id");

                entity.Property(e => e.RefMetierId).HasColumnName("REF_Metier_Id");

                entity.Property(e => e.RefVilleId).HasColumnName("REF_Ville_Id");

                entity.HasOne(d => d.ProfilDeactivationNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.ProfilDeactivation)
                    .HasConstraintName("FK_Cliente_ToProfil");

                entity.HasOne(d => d.RefCarteType)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefCarteTypeId)
                    .HasConstraintName("FK_Cliente_ToREF_CarteType");

                entity.HasOne(d => d.RefClienteStatut)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefClienteStatutId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_ToREF_ClienteStatut");

                entity.HasOne(d => d.RefGenre)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefGenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_ToREF_Genre");

                entity.HasOne(d => d.RefMagasin)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefMagasinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_ToREF_Magasin");

                entity.HasOne(d => d.RefMetier)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefMetierId)
                    .HasConstraintName("FK_Cliente_ToREF_Metier");

                entity.HasOne(d => d.RefVille)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.RefVilleId)
                    .HasConstraintName("FK_Cliente_ToREF_Ville");
            });

            modelBuilder.Entity<ClienteAudit>(entity =>
            {
                entity.Property(e => e.AspNetUsersId).HasMaxLength(450);

                entity.Property(e => e.ClienteCode).HasMaxLength(255);

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.HasOne(d => d.AspNetUsers)
                    .WithMany(p => p.ClienteAudit)
                    .HasForeignKey(d => d.AspNetUsersId)
                    .HasConstraintName("FK_ClienteAudit_ToAspNetUsers");
            });

            modelBuilder.Entity<ClienteEvent>(entity =>
            {
                entity.Property(e => e.AspNetUsersId).HasMaxLength(450);

                entity.Property(e => e.ClienteCode).HasMaxLength(255);

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.HasOne(d => d.AspNetUsers)
                    .WithMany(p => p.ClienteEvent)
                    .HasForeignKey(d => d.AspNetUsersId)
                    .HasConstraintName("FK_ClienteEvent_ToAspNetUsers");

                entity.HasOne(d => d.NewStatus)
                    .WithMany(p => p.ClienteEventNewStatus)
                    .HasForeignKey(d => d.NewStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteEvent_ToREF_ClienteStatutNewStatus");

                entity.HasOne(d => d.PreviousStatus)
                    .WithMany(p => p.ClienteEventPreviousStatus)
                    .HasForeignKey(d => d.PreviousStatusId)
                    .HasConstraintName("FK_ClienteEvent_ToREF_ClienteStatutPreviousStatus");
            });

            modelBuilder.Entity<MapClienteAppFile>(entity =>
            {
                entity.HasKey(e => new { e.ClienteId, e.AppFileId });

                entity.ToTable("MAP_Cliente_AppFile");

                entity.HasOne(d => d.AppFile)
                    .WithMany(p => p.MapClienteAppFile)
                    .HasForeignKey(d => d.AppFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAP_Cliente_AppFile_AppFile");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.MapClienteAppFile)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAP_Cliente_AppFile_Cliente");
            });

            modelBuilder.Entity<Profil>(entity =>
            {
                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.Nom).IsRequired();

                entity.Property(e => e.Prenom).IsRequired();

                entity.Property(e => e.RefMagasinId).HasColumnName("REF_Magasin_Id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Profil)
                    .HasForeignKey<Profil>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profil_ToAspNetUsers");

                entity.HasOne(d => d.RefMagasin)
                    .WithMany(p => p.Profil)
                    .HasForeignKey(d => d.RefMagasinId)
                    .HasConstraintName("FK_Profil_ToREF_Magasin");
            });

            modelBuilder.Entity<RefAnniversaireCarte>(entity =>
            {
                entity.ToTable("REF_AnniversaireCarte");

                entity.Property(e => e.Montant).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefCarteType>(entity =>
            {
                entity.ToTable("REF_CarteType");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefClienteStatut>(entity =>
            {
                entity.ToTable("REF_ClienteStatut");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefClienteType>(entity =>
            {
                entity.ToTable("REF_ClienteType");

                entity.Property(e => e.Montant).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefGenre>(entity =>
            {
                entity.ToTable("REF_Genre");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefMagasin>(entity =>
            {
                entity.ToTable("REF_Magasin");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefMetier>(entity =>
            {
                entity.ToTable("REF_Metier");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefVille>(entity =>
            {
                entity.ToTable("REF_Ville");

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

