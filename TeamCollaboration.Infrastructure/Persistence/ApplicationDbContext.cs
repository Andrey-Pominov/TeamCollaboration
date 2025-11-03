using Microsoft.EntityFrameworkCore;
using TeamCollaboration.Domain.Entities;

namespace TeamCollaboration.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<BoardColumn> Columns => Set<BoardColumn>();

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(builder =>
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(2000);
            builder.Property(t => t.Assignee).HasMaxLength(200);
            builder.Property(t => t.BoardId).IsRequired();
            builder.Property(t => t.SortOrder).HasDefaultValue(0);
            builder.HasIndex(t => new { t.BoardId, t.BoardColumnId, t.SortOrder });
        });

        modelBuilder.Entity<BoardColumn>(builder =>
        {
            builder.ToTable("Columns");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.BoardId).IsRequired();
            builder.Property(c => c.SortOrder).HasDefaultValue(0);
            builder.HasIndex(c => new { c.BoardId, c.SortOrder }).IsUnique();
        });

        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(200);
        });
    }
}

