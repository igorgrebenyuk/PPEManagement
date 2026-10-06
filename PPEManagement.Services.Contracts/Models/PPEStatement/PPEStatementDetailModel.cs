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
    public string StatementNumber { get; set; } = string.Empty;

    /// <summary>
    /// Дата выдачи
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Наименование организации
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Структурное подразделение
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// ФИО и должность ответственного лица
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Информация о сотруднике
    /// </summary>
    public EmployeeModel Employee { get; set; } = null!;

    /// <summary>
    /// Список выданных СИЗ
    /// </summary>
    public IReadOnlyCollection<PPEStatementItemModel> Items { get; set; } = new List<PPEStatementItemModel>();

    /// <summary>
    /// Итоговое количество выданных противогазов
    /// </summary>
    public int TotalGasMasks { get; set; }

    /// <summary>
    /// Итоговое количество выданных аптечек КИМГЗ
    /// </summary>
    public int TotalKIMGZ { get; set; }

    /// <summary>
    /// Итоговое количество прочих СИЗ
    /// </summary>
    public int TotalOtherPPE { get; set; }
}