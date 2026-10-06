using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPEManagement.Dal.Contracts.Interfaces;
using PPEManagement.Context.EntityFrameworkCore;
using PPEManagement.Entities;

namespace PPEManagement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Employee"/> для Entity Framework Core
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.PersonnelNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Department)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Position)
            .IsRequired()
            .HasMaxLength(200);

        // Уникальный индекс табельного номера для не удаленных записей (MS SQL Server / LocalDB)
        builder.HasIndex(x => x.PersonnelNumber)
            .HasDatabaseName("IX_Employees_PersonnelNumber")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
}