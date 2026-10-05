namespace PPEManagement.Services.Contracts.Models.Employee;

/// <summary>
/// Модель создания сотрудника
/// </summary>
public class EmployeeCreateModel
{
    /// <summary>
    /// Табельный номер
    /// </summary>
    public string PersonnelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Подразделение
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Должность
    /// </summary>
    public string Position { get; set; } = string.Empty;
}