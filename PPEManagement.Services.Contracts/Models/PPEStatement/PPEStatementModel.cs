namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Модель ведомости выдачи СИЗ
/// </summary>
public class PPEStatementModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Номер ведомости
    /// </summary>
    public int StatementNumber { get; set; }

    /// <summary>
    /// Дата выдачи
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Идентификатор сотрудника
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string EmployeeFullName { get; set; } = string.Empty;
}