using PPEManagement.Services.Contracts.Models.Employee;

namespace PPEManagement.Services.Contracts;

/// <summary>
/// Сервис для работы с сотрудниками
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Получение списка сотрудников
    /// </summary>
    Task<IReadOnlyCollection<EmployeeModel>> GetEmployeesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сотрудника по идентификатору
    /// </summary>
    Task<EmployeeModel> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление сотрудника
    /// </summary>
    Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateModel employeeModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление сотрудника
    /// </summary>
    Task UpdateEmployeeAsync(EmployeeUpdateModel employeeModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сотрудника
    /// </summary>
    Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken);
}