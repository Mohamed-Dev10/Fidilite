using System;
using System.Collections.Generic;
using BRICOMA.ECOMMERCE.Data.Models.Market;
using Microsoft.EntityFrameworkCore;

namespace BRICOMA.ECOMMERCE.Data.Contexts;

public partial class BRICOMAMARKETContext : DbContext
{
    public BRICOMAMARKETContext()
    {
    }

    public BRICOMAMARKETContext(DbContextOptions<BRICOMAMARKETContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BonsachatsFid> BonsachatsFids { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Entretourcaisse> Entretourcaisses { get; set; }

    public virtual DbSet<GlovoCatalogue> GlovoCatalogues { get; set; }

    public virtual DbSet<ItemsEcommerce> ItemsEcommerces { get; set; }

    public virtual DbSet<Lesticketscaiss> Lesticketscaisses { get; set; }

    public virtual DbSet<Magasin> Magasins { get; set; }

    public virtual DbSet<Metier> Metiers { get; set; }

    public virtual DbSet<RefStatut> RefStatuts { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Typeclient> Typeclients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Name=BRICOMAMARKETContext");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BonsachatsFid>(entity =>
        {
            entity.HasKey(e => e.RefBon);

            entity.ToTable("bonsachats_fid");

            entity.Property(e => e.RefBon)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ref_bon");
            entity.Property(e => e.Agent)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("agent");
            entity.Property(e => e.Annee)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("annee");
            entity.Property(e => e.CodeClt)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("code_clt");
            entity.Property(e => e.CodeMag)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_mag");
            entity.Property(e => e.CodeUti)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("code_uti");
            entity.Property(e => e.DateBon).HasColumnName("date_bon");
            entity.Property(e => e.DateTkt).HasColumnName("date_tkt");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Heure)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("heure");
            entity.Property(e => e.Montant)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montant");
            entity.Property(e => e.Ticket)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ticket");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83FBF768EAF");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Categorie__paren__03F0984C");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.CodeClt);

            entity.ToTable("clients");

            entity.HasIndex(e => new { e.CodePri, e.Bloquer }, "NonClusteredIndex-20230203-161553");

            entity.Property(e => e.CodeClt)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("code_clt");
            entity.Property(e => e.Actif).HasColumnName("actif");
            entity.Property(e => e.Adresse)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("adresse");
            entity.Property(e => e.BloqEdba)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("bloq_edba");
            entity.Property(e => e.Bloquer)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("bloquer");
            entity.Property(e => e.Cartevalid).HasColumnName("cartevalid");
            entity.Property(e => e.Cin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cin");
            entity.Property(e => e.Client1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("client");
            entity.Property(e => e.CodeCar)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("code_car");
            entity.Property(e => e.CodeCat)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_cat");
            entity.Property(e => e.CodeMag)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_mag");
            entity.Property(e => e.CodeMet)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_met");
            entity.Property(e => e.CodePri)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_pri");
            entity.Property(e => e.CodeVen)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_ven");
            entity.Property(e => e.CodeVil)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_vil");
            entity.Property(e => e.Compte)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("compte");
            entity.Property(e => e.Datecreat).HasColumnName("datecreat");
            entity.Property(e => e.Datemodif).HasColumnName("datemodif");
            entity.Property(e => e.Datenaiss).HasColumnName("datenaiss");
            entity.Property(e => e.Echeance)
                .HasColumnType("numeric(3, 0)")
                .HasColumnName("echeance");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FlagFid)
                .HasDefaultValue(false)
                .HasColumnName("flag_fid");
            entity.Property(e => e.Idfiscal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idfiscal");
            entity.Property(e => e.Interne)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("interne");
            entity.Property(e => e.Modepaie)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("modepaie");
            entity.Property(e => e.Nia)
                .HasColumnType("decimal(20, 0)")
                .HasColumnName("NIA");
            entity.Property(e => e.Patente)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("patente");
            entity.Property(e => e.Plafond)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("plafond");
            entity.Property(e => e.Rc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rc");
            entity.Property(e => e.Siteweb)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("siteweb");
            entity.Property(e => e.Solvable)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("solvable");
            entity.Property(e => e.Statut)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("statut");
            entity.Property(e => e.Tel)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("tel");
            entity.Property(e => e.Telmob)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telmob");
            entity.Property(e => e.Ville)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ville");
        });

        modelBuilder.Entity<Entretourcaisse>(entity =>
        {
            entity.HasKey(e => e.RefRet);

            entity.ToTable("entretourcaisse");

            entity.HasIndex(e => e.CodeMag, "NonClusteredIndex-20190514-120840");

            entity.HasIndex(e => e.DateRet, "NonClusteredIndex-20190514-122203");

            entity.Property(e => e.RefRet)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ref_ret");
            entity.Property(e => e.Agent)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("agent");
            entity.Property(e => e.Annee)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("annee");
            entity.Property(e => e.CodeMag)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_mag");
            entity.Property(e => e.DateRet).HasColumnName("date_ret");
            entity.Property(e => e.DateTkt1).HasColumnName("date_tkt1");
            entity.Property(e => e.DateTkt2).HasColumnName("date_tkt2");
            entity.Property(e => e.MntHt)
                .HasColumnType("decimal(14, 2)")
                .HasColumnName("mnt_ht");
            entity.Property(e => e.MntTtc)
                .HasColumnType("decimal(14, 2)")
                .HasColumnName("mnt_ttc");
            entity.Property(e => e.Remarque)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("remarque");
            entity.Property(e => e.Ticket1)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ticket1");
            entity.Property(e => e.Ticket2)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ticket2");
            entity.Property(e => e.Vendeur)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("vendeur");
        });

        modelBuilder.Entity<GlovoCatalogue>(entity =>
        {
            entity.HasKey(e => e.ExternalId).HasName("PK__GLOVO_CA__E0D2834337C00354");

            entity.ToTable("GLOVO_CATALOGUE");

            entity.Property(e => e.ExternalId)
                .HasMaxLength(255)
                .HasColumnName("External_ID");
            entity.Property(e => e.AttributeGroups)
                .HasMaxLength(255)
                .HasColumnName("Attribute_Groups");
            entity.Property(e => e.Collection).HasMaxLength(255);
            entity.Property(e => e.CollectionImage)
                .HasMaxLength(255)
                .HasColumnName("Collection_Image");
            entity.Property(e => e.CollectionOrder).HasColumnName("Collection_Order");
            entity.Property(e => e.DateCreation).HasColumnType("datetime");
            entity.Property(e => e.DateExportaion).HasColumnType("datetime");
            entity.Property(e => e.DateModification).HasColumnType("datetime");
            entity.Property(e => e.Dietary).HasMaxLength(255);
            entity.Property(e => e.Image1)
                .HasMaxLength(255)
                .HasColumnName("Image_1");
            entity.Property(e => e.Image10)
                .HasMaxLength(255)
                .HasColumnName("Image_10");
            entity.Property(e => e.Image2)
                .HasMaxLength(255)
                .HasColumnName("Image_2");
            entity.Property(e => e.Image3)
                .HasMaxLength(255)
                .HasColumnName("Image_3");
            entity.Property(e => e.Image4)
                .HasMaxLength(255)
                .HasColumnName("Image_4");
            entity.Property(e => e.Image5)
                .HasMaxLength(255)
                .HasColumnName("Image_5");
            entity.Property(e => e.Image6)
                .HasMaxLength(255)
                .HasColumnName("Image_6");
            entity.Property(e => e.Image7)
                .HasMaxLength(255)
                .HasColumnName("Image_7");
            entity.Property(e => e.Image8)
                .HasMaxLength(255)
                .HasColumnName("Image_8");
            entity.Property(e => e.Image9)
                .HasMaxLength(255)
                .HasColumnName("Image_9");
            entity.Property(e => e.ImageSource1)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_1");
            entity.Property(e => e.ImageSource10)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_10");
            entity.Property(e => e.ImageSource2)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_2");
            entity.Property(e => e.ImageSource3)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_3");
            entity.Property(e => e.ImageSource4)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_4");
            entity.Property(e => e.ImageSource5)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_5");
            entity.Property(e => e.ImageSource6)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_6");
            entity.Property(e => e.ImageSource7)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_7");
            entity.Property(e => e.ImageSource8)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_8");
            entity.Property(e => e.ImageSource9)
                .HasMaxLength(255)
                .HasColumnName("Image_Source_9");
            entity.Property(e => e.IsAlcoholic)
                .HasMaxLength(255)
                .HasColumnName("Is_Alcoholic");
            entity.Property(e => e.IsTobacco)
                .HasMaxLength(255)
                .HasColumnName("Is_Tobacco");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("Product_Name");
            entity.Property(e => e.Section).HasMaxLength(255);
            entity.Property(e => e.SectionOrder).HasColumnName("Section_Order");
            entity.Property(e => e.SuperCollection).HasMaxLength(255);
            entity.Property(e => e.SuperCollectionImage)
                .HasMaxLength(255)
                .HasColumnName("SuperCollection_Image");
            entity.Property(e => e.SuperCollectionOrder).HasColumnName("SuperCollection_Order");
        });

        modelBuilder.Entity<ItemsEcommerce>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ITEMS_ECOMMERCE");

            entity.Property(e => e.AchatBloq).HasColumnName("achat_bloq");
            entity.Property(e => e.Article)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("article");
            entity.Property(e => e.CodeArt)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("code_art");
            entity.Property(e => e.Datecreat).HasColumnName("datecreat");
            entity.Property(e => e.Datemodif).HasColumnName("datemodif");
            entity.Property(e => e.Debut).HasColumnName("debut");
            entity.Property(e => e.Fin).HasColumnName("fin");
            entity.Property(e => e.Prixpromo)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("prixpromo");
            entity.Property(e => e.PvTtc)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("pv_ttc");
            entity.Property(e => e.Rayon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rayon");
        });

        modelBuilder.Entity<Lesticketscaiss>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("lesticketscaisses");

            entity.Property(e => e.Annee)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("annee");
            entity.Property(e => e.CodeClt)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("code_clt");
            entity.Property(e => e.CodeMaa)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("code_maa");
            entity.Property(e => e.CodeMag)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_mag");
            entity.Property(e => e.CodeOpr)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("code_opr");
            entity.Property(e => e.CodeRes)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("code_res");
            entity.Property(e => e.DateTkt).HasColumnName("date_tkt");
            entity.Property(e => e.Heure)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("heure");
            entity.Property(e => e.MntHt)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("mnt_ht");
            entity.Property(e => e.MntTtc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("mnt_ttc");
            entity.Property(e => e.Modepaie1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modepaie1");
            entity.Property(e => e.Modepaie2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modepaie2");
            entity.Property(e => e.Montant1)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montant1");
            entity.Property(e => e.Montant2)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montant2");
            entity.Property(e => e.Points)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("points");
            entity.Property(e => e.PointsM)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("points_m");
            entity.Property(e => e.Taxe)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("taxe");
            entity.Property(e => e.Ticket)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ticket");
            entity.Property(e => e.Valremise)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("valremise");
        });

        modelBuilder.Entity<Magasin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("magasins");

            entity.Property(e => e.AnneeOuv).HasColumnName("ANNEE_OUV");
            entity.Property(e => e.CodeMag)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_mag");
            entity.Property(e => e.CodeReg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_reg");
            entity.Property(e => e.DateInt).HasColumnName("date_int");
            entity.Property(e => e.Datecalstock).HasColumnName("datecalstock");
            entity.Property(e => e.Dateremdata).HasColumnName("dateremdata");
            entity.Property(e => e.Directeur)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("directeur");
            entity.Property(e => e.IntCmd).HasColumnName("int_cmd");
            entity.Property(e => e.Magasin1)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("magasin");
            entity.Property(e => e.NbrSal).HasColumnName("nbr_sal");
            entity.Property(e => e.OrdrOuv).HasColumnName("ORDR_OUV");
            entity.Property(e => e.Rang)
                .HasColumnType("numeric(3, 0)")
                .HasColumnName("rang");
            entity.Property(e => e.SDrog).HasColumnName("S_DROG");
            entity.Property(e => e.SElec).HasColumnName("S_ELEC");
            entity.Property(e => e.SJard).HasColumnName("S_JARD");
            entity.Property(e => e.SOut).HasColumnName("S_OUT");
            entity.Property(e => e.SQuic).HasColumnName("S_QUIC");
            entity.Property(e => e.SSani).HasColumnName("S_SANI");
            entity.Property(e => e.StoreId)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("store_id");
        });

        modelBuilder.Entity<Metier>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("metiers");

            entity.Property(e => e.CodeMet)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_met");
            entity.Property(e => e.Metier1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metier");
        });

        modelBuilder.Entity<RefStatut>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0701611B4A");

            entity.ToTable("REF_Statut");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("regions");

            entity.Property(e => e.CodeReg)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_reg");
            entity.Property(e => e.Region1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Typeclient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("typeclients");

            entity.Property(e => e.CodePri)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code_pri");
            entity.Property(e => e.Statut)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("statut");
            entity.Property(e => e.TypeClient1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type_client");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
