using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.ORM;


public class ApplicationDbContext : DbContext
{
    public DbSet<TaskBoardAggregate> TaskBoards { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskBoardAggregate>(builder => ConfigureTaskBoard(builder));
    }

    private void ConfigureTaskBoard(EntityTypeBuilder<TaskBoardAggregate> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new {
            e.OrganizationId,
            e.Name
        }).IsUnique();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .IsRequired(false);

        builder.HasIndex(e => e.OrganizationId);

        builder.OwnsOne(e => e.Timestamps, t =>
        {
            t.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");

            t.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        });
        builder.OwnsOne(e => e.AuthorInfo, a =>
        {
            a.Property(p => p.CreatedById)
                .IsRequired()
                .HasColumnName("created_by_id");
            a.Property(p => p.UpdatedById)
                .IsRequired()
                .HasColumnName("updated_by_id");
        });

        // builder.HasMany(e => e.Columns)
        //     .WithOne()
        //     .HasForeignKey(c => c.BoardId)
        //     .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(e => e.Columns, ConfigureTaskBoardColumn);
    }

    private void ConfigureTaskBoardColumn(OwnedNavigationBuilder<TaskBoardAggregate, TaskBoardColumnModel> builder)
    {
        builder.ToTable("task_board_columns");

        builder.WithOwner()
            .HasForeignKey("board_id");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new {
            e.Name,
            e.BoardId
        }).IsUnique();

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.OwnsOne(e => e.AuthorInfo, a =>
        {
            a.Property(p => p.CreatedById)
                .IsRequired()
                .HasColumnName("created_by_id");
            a.Property(p => p.UpdatedById)
                .IsRequired()
                .HasColumnName("updated_by_id");
        });

        builder.OwnsOne(e => e.Timestamps, t =>
        {
            t.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");
            t.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at");
        });
    }
}
