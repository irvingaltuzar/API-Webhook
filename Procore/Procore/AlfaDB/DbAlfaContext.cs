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

    public virtual DbSet<OauthRefreshTokenProcore> OauthRefreshTokenProcores { get; set; }

    public virtual DbSet<OauthTokensProcore> OauthTokensProcores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=192.168.3.161;Database=db_alfa;User Id=alfaUsr;Password=4lf4Usr.;Trusted_Connection=False;TrustServerCertificate=True;Encrypt = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
