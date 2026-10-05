using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="Employee"/>
/// </summary>
public interface IEmployeeRepository: IBaseWriteRepository<Employee>
{
    /// <summary>
    /// Получение всех сотрудников
    /// </summary>
    Task<IReadOnlyCollection<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сотрудника по ID
    /// </summary>
    Task<Employee?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Поиск сотрудника по табельному номеру
    /// </summary>
    Task<Employee?> GetEmployeeByPersonnelNumberAsync(string personnelNumber, CancellationToken cancellationToken);
}