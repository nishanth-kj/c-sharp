using Microsoft.EntityFrameworkCore;
using SharpIB.Domain.Entities;

namespace SharpIB.Infrastructure.Data;

public class SharpIBDbContext(DbContextOptions<SharpIBDbContext> options) : DbContext(options)
{
    public DbSet<ActivityRecord> Activities => Set<ActivityRecord>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<AppCategoryMapping> AppCategoryMappings => Set<AppCategoryMapping>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<DailySnapshot> DailySnapshots => Set<DailySnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ActivityRecord
        modelBuilder.Entity<ActivityRecord>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ProcessName).HasMaxLength(256).IsRequired();
            e.Property(a => a.WindowTitle).HasMaxLength(1024);
            e.Property(a => a.ExecutablePath).HasMaxLength(1024);
            e.HasIndex(a => a.StartTime);
            e.HasIndex(a => a.ProcessName);
            e.Ignore(a => a.Duration); // Computed property
            e.HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.SetNull);
        });

        // Category
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).HasMaxLength(128).IsRequired();
            e.Property(c => c.ColorHex).HasMaxLength(9);
            e.HasMany(c => c.Mappings).WithOne(m => m.Category).HasForeignKey(m => m.CategoryId);
        });

        // AppCategoryMapping
        modelBuilder.Entity<AppCategoryMapping>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.ProcessName).HasMaxLength(256).IsRequired();
            e.HasIndex(m => m.ProcessName).IsUnique();
        });

        // Goal
        modelBuilder.Entity<Goal>(e =>
        {
            e.HasKey(g => g.Id);
            e.Property(g => g.Title).HasMaxLength(256).IsRequired();
            e.Property(g => g.Description).HasMaxLength(1024);
            e.Property(g => g.TrackedProcesses).HasMaxLength(1024);
            e.HasOne(g => g.TrackedCategory).WithMany().HasForeignKey(g => g.TrackedCategoryId).OnDelete(DeleteBehavior.SetNull);
        });

        // DailySnapshot
        modelBuilder.Entity<DailySnapshot>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasIndex(s => s.Date).IsUnique();
            e.Property(s => s.TopAppsJson).HasMaxLength(4096);
        });

        // Seed default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Productive", Level = Domain.Enums.ProductivityLevel.Productive, ColorHex = "#4CAF50" },
            new Category { Id = 2, Name = "Neutral", Level = Domain.Enums.ProductivityLevel.Neutral, ColorHex = "#607D8B" },
            new Category { Id = 3, Name = "Distracting", Level = Domain.Enums.ProductivityLevel.Distracting, ColorHex = "#F44336" }
        );
    }
}

