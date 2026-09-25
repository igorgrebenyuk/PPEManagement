using PPEManagement.Dal.Contracts;

namespace PPEManagement.Entities;

/// <summary>
/// Сущность шапки ведомости выдачи средств индивидуальной защиты.
/// </summary>
public class PPEStatement : BaseAuditEntity
{
    /// <summary>
    /// Уникальный регистрационный номер ведомости (например, "ВЕД-2026/001").
    /// </summary>
    public string StatementNumber { get; set; } = string.Empty;

    /// <summary>
    /// Наименование организации.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Наименование подразделения или формирования НФГО, которому выдаются СИЗ.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи (например, "Плановая замена", "Тренировка", "Введение Плана ГО").
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время формирования ведомости.
    /// </summary>
    public DateTime IssueDate { get; set; } = DateTime.Now;

    /// <summary>
    /// ФИО и должность ответственного за выдачу лица.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Итоговое количество противогазов, выданных по ведомости (шт.).
    /// </summary>
    public int TotalGasMasks { get; set; }

    /// <summary>
    /// Итоговое количество аптечек КИМГЗ, выданных по ведомости (шт.).
    /// </summary>
    public int TotalKIMGZ { get; set; }

    /// <summary>
    /// Итоговое количество прочих СИЗ, выданных по ведомости (шт./компл.).
    /// </summary>
    public int TotalOtherPPE { get; set; }

    /// <summary>
    /// Список строк табличной части ведомости.
    /// </summary>
    public List<PPEStatementItem> Items { get; set; } = new();
}