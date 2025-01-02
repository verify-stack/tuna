using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Roblox.Configuration;
using Roblox.Website.Models.DB;

namespace Roblox.Website.Context;

public partial class RobloxContext : DbContext
{
    public RobloxContext()
    {
    }

    public RobloxContext(DbContextOptions<RobloxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql(Settings.MainConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("AccountStatusEnum", new[] { "Offline", "Online", "Playing", "Studio" })
            .HasPostgresEnum("GenderEnum", new[] { "Male", "Female" });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Name }).HasName("Users_pkey");

            entity.ToTable(tb => tb.HasComment("Contains all of the ROBLOX users."));

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasDefaultValueSql("'Hello! Im new to ROBLOX.'::character varying");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Exploring ROBLOX.'::character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
