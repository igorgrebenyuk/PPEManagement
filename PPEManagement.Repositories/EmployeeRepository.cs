using PPEManagement.Context.Repositories;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;
using PPEManagement.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PPEManagement.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="Employee"/>
/// </summary>
public class EmployeeRepository : BaseWriteRepository<Employee>, IEmployeeRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public EmployeeRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех сотрудников
    /// </summary>
    Task<IReadOnlyCollection<Employee>> IEmployeeRepository.GetEmployeesAsync(CancellationToken cancellationToken)
        => reader.Read<Employee>()
            .NotDeletedAt()
            .OrderBy(x => x.FullName)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение сотрудника по ID
    /// </summary>
    Task<Employee?> IEmployeeRepository.GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Employee>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Поиск сотрудника по табельному номеру
    /// </summary>
    Task<Employee?> IEmployeeRepository.GetEmployeeByPersonnelNumberAsync(string personnelNumber, CancellationToken cancellationToken)
        => reader.Read<Employee>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.PersonnelNumber.ToLower() == personnelNumber.ToLower(), cancellationToken);
}