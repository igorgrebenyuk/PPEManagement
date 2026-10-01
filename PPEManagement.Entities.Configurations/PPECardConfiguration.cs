using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Context.EntityFrameworkCore;
using PPEManagement.Entities;

namespace PPEManagement.Entities.Configurations;
/// <summary>
/// Конфигурация сущности <see cref="PPECard"/> для Entity Framework Core
/// </summary>
public class PPECardConfiguration : IEntityTypeConfiguration<PPECard>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PPECard> builder)
    {
        builder.ToTable("PPECards");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BatchNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Size)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IssuedToEmployeeFullName)
            .HasMaxLength(250);

        builder.HasIndex(x => new { x.Name, x.BatchNumber })
            .HasDatabaseName("IX_PPECards_Name_BatchNumber")
            .HasFilter($"[{nameof(IEntityAuditDeletedAt.DeletedAt)}] IS NULL");
    }
}