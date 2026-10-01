using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Context.EntityFrameworkCore;
using PPEManagement.Entities;

namespace PPEManagement.Entities.Configurations;
/// <summary>
/// Конфигурация сущности <see cref="PPEStatement"/> для Entity Framework Core
/// </summary>
public class PPEStatementConfiguration : IEntityTypeConfiguration<PPEStatement>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PPEStatement> builder)
    {
        builder.ToTable("PPEStatements");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.StatementNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.OrganizationName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.DepartmentName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Reason)
            .HasMaxLength(500);

        builder.Property(x => x.ResponsiblePerson)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.SignedScanPath)
            .HasMaxLength(500);

        // Настройка связи One-to-Many
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Statement)
            .HasForeignKey(x => x.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.StatementNumber)
            .HasDatabaseName("IX_PPEStatements_StatementNumber")
            .IsUnique()
            .HasFilter($"[{nameof(IEntityAuditDeletedAt.DeletedAt)}] IS NULL");
    }
}