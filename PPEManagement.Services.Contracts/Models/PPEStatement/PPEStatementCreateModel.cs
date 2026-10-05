namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Модель создания ведомости выдачи СИЗ.
/// </summary>
public class PPEStatementCreateModel
{
    /// <summary>
    /// Наименование организации.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Структурное подразделение / НФГО.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи (плановая замена, введение плана ГО и т.д.).
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления ведомости.
    /// </summary>
    public DateTime StatementDate { get; set; } = DateTime.Today;

    /// <summary>
    /// ФИО и должность ответственного лица.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Список позиций (выданных СИЗ) в ведомости.
    /// </summary>
    public List<PPEStatementItemCreateModel> Items { get; set; } = new();
}