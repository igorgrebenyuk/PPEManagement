namespace PPEManagement.Services.Contracts.Models.PPEStatement;

public class PPEStatementModel
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Регистрационный номер ведомости.
    /// </summary>
    public string StatementNumber { get; set; } = string.Empty;

    /// <summary>
    /// Наименование организации.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Подразделение.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления.
    /// </summary>
    public DateTime IssueDate { get; set; }
}