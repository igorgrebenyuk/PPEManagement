using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Context.EntityFrameworkCore;
using PPEManagement.Entities;

namespace PPEManagement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="PPEStatementItem"/> для Entity Framework Core
/// </summary>
public class PPEStatementItemConfiguration : IEntityTypeConfiguration<PPEStatementItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PPEStatementItem> builder)
    {
        builder.ToTable("PPEStatementItems");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.EmployeeFullName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.PersonnelNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PPEName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BatchNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Size)
            .HasMaxLength(50);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.SignatureStatus)
            .IsRequired()
            .HasMaxLength(100);
    }
}