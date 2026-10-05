using PPEManagement.Services.Contracts.Models.Employee;

namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Детальная модель ведомости выдачи СИЗ
/// </summary>
public class PPEStatementDetailModel
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
    /// Информация о сотруднике
    /// </summary>
    public EmployeeModel Employee { get; set; } = null!;

    /// <summary>
    /// Список выданных СИЗ
    /// </summary>
    public IReadOnlyCollection<PPEStatementItemModel> Items { get; set; } = new List<PPEStatementItemModel>();
}