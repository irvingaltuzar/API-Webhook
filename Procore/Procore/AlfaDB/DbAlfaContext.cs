using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Procore.AlfaDB;

public partial class DbAlfaContext : DbContext
{
    public DbAlfaContext()
    {
    }

    public DbAlfaContext(DbContextOptions<DbAlfaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatSupplierNotification> CatSupplierNotifications { get; set; }

    public virtual DbSet<DmiabaSupplierRegistration> DmiabaSupplierRegistrations { get; set; }

    public virtual DbSet<OauthRefreshTokenProcore> OauthRefreshTokenProcores { get; set; }

    public virtual DbSet<OauthTokensProcore> OauthTokensProcores { get; set; }

    public virtual DbSet<ProcoreConfiguration> ProcoreConfigurations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=192.168.3.171;Database=db_alfa;User Id=alfaUsr;Password=4lf4Usr.;Trusted_Connection=False;TrustServerCertificate=True;Encrypt = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatSupplierNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cat_supp__3213E83F5EBA4FD3");

            entity.ToTable("cat_supplier_notification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .HasColumnName("mail");
            entity.Property(e => e.ResponsableUser)
                .HasMaxLength(50)
                .HasColumnName("responsable_user");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<DmiabaSupplierRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dmiaba_s__3213E83F6149FDB9");

            entity.ToTable("dmiaba_supplier_registration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BanEmail)
                .HasMaxLength(255)
                .HasColumnName("ban_email");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(255)
                .HasColumnName("bank_account");
            entity.Property(e => e.BankClabe)
                .HasMaxLength(255)
                .HasColumnName("bank_clabe");
            entity.Property(e => e.BankSwift)
                .HasMaxLength(255)
                .HasColumnName("bank_swift");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(255)
                .HasColumnName("business_name");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Classification)
                .HasMaxLength(3034)
                .HasColumnName("classification");
            entity.Property(e => e.ComercialName)
                .HasMaxLength(255)
                .HasColumnName("comercial_name");
            entity.Property(e => e.Contact)
                .HasMaxLength(255)
                .HasColumnName("contact");
            entity.Property(e => e.ContactReference)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contact_reference");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Cp)
                .HasMaxLength(255)
                .HasColumnName("cp");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreditDays)
                .HasMaxLength(255)
                .HasColumnName("credit_days");
            entity.Property(e => e.Currency)
                .HasMaxLength(255)
                .HasColumnName("currency");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Efo)
                .HasMaxLength(255)
                .HasColumnName("efo");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Import)
                .HasMaxLength(255)
                .HasColumnName("import");
            entity.Property(e => e.ManualDown).HasColumnName("manual_down");
            entity.Property(e => e.MotiveDown)
                .HasMaxLength(318)
                .HasColumnName("motive_down");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.ProcoreId).HasColumnName("procore_id");
            entity.Property(e => e.ReferenciaIntelisis)
                .HasMaxLength(255)
                .HasColumnName("referencia_intelisis");
            entity.Property(e => e.Rfc)
                .HasMaxLength(255)
                .HasColumnName("rfc");
            entity.Property(e => e.SpecialityMain)
                .HasMaxLength(255)
                .HasColumnName("speciality_main");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("('0')")
                .HasColumnName("status");
            entity.Property(e => e.StatusFiles)
                .HasMaxLength(255)
                .HasColumnName("status_files");
            entity.Property(e => e.Suburb)
                .HasMaxLength(255)
                .HasColumnName("suburb");
            entity.Property(e => e.TypePerson)
                .HasMaxLength(255)
                .HasColumnName("type_person");
            entity.Property(e => e.TypeSupplier)
                .HasMaxLength(255)
                .HasColumnName("type_supplier");
            entity.Property(e => e.UpdateUser).HasColumnName("update_user");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.User)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user");
            entity.Property(e => e.UserApproved)
                .HasMaxLength(100)
                .HasColumnName("user_approved");
            entity.Property(e => e.WebPage)
                .HasMaxLength(255)
                .HasColumnName("web_page");
            entity.Property(e => e.Zip)
                .HasMaxLength(255)
                .HasColumnName("zip");
        });

        modelBuilder.Entity<OauthRefreshTokenProcore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__oauth_re__3213E83F93BC6CBD");

            entity.ToTable("oauth_refresh_token_procore");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessTokenId)
                .HasMaxLength(1000)
                .HasColumnName("access_token_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.RefreshTokenId)
                .HasMaxLength(100)
                .HasColumnName("refresh_token_id");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.TokenType)
                .HasMaxLength(255)
                .HasColumnName("token_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<OauthTokensProcore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_tokens_procore_id_primary");

            entity.ToTable("oauth_tokens_procore");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ResourceOwnerId).HasColumnName("resource_owner_id");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<ProcoreConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__procore___3213E83F85134077");

            entity.ToTable("procore_configuration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(1000)
                .HasColumnName("client_id");
            entity.Property(e => e.ClientSecret)
                .HasMaxLength(1000)
                .HasColumnName("client_secret");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(1000)
                .HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FolderCompanyId).HasColumnName("folder_company_id");
            entity.Property(e => e.ServiceUrl)
                .HasMaxLength(1000)
                .HasColumnName("service_url");
            entity.Property(e => e.ServiceUrlLogin)
                .HasMaxLength(1000)
                .HasColumnName("service_url_login");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
