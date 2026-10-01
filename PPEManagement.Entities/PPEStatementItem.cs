using PPEManagement.Dal.Contracts;

namespace PPEManagement.Entities;

/// <summary>
/// Сущность строки табличной части ведомости выдачи СИЗ.
/// </summary>
public class PPEStatementItem : BaseAuditEntity
{
    /// <summary>
    /// Внешний ключ связи с родительской ведомостью.
    /// </summary>
    public Guid StatementId { get; set; }

    /// <summary>
    /// Навигационное свойство родительской ведомости.
    /// </summary>
    public PPEStatement? Statement { get; set; }

    /// <summary>
    /// ФИО сотрудника-получателя.
    /// </summary>
    public string EmployeeFullName { get; set; } = string.Empty;

    /// <summary>
    /// Табельный номер сотрудника-получателя.
    /// </summary>
    public string PersonnelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Наименование выданного СИЗ (из карточки склада).
    /// </summary>
    public string PPEName { get; set; } = string.Empty;

    /// <summary>
    /// Номер партии или заводской номер выданного СИЗ.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Размер выданного СИЗ (или "—", если размер не предусмотрен).
    /// </summary>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Выданное количество (шт./компл.).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата и время фактической выдачи.
    /// </summary>
    public DateTime IssueDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Статус подписи получателя (например, "Подписано УКЭП", "Ожидает подписи").
    /// </summary>
    public string SignatureStatus { get; set; } = "Ожидает подписи";
}