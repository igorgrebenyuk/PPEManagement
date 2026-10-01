namespace PPEManagement.Entities;

/// <summary>
/// Сущность строки табличной части ведомости выдачи СИЗ.
/// </summary>
public class PPEStatementItem : BaseAuditEntity
{
    /// <summary>
    /// Внешний ключ шапки ведомости.
    /// </summary>
    public Guid StatementId { get; set; }

    /// <summary>
    /// Навигационное свойство шапки ведомости.
    /// </summary>
    public PPEStatement Statement { get; set; } = null!;

    /// <summary>
    /// ФИО сотрудника.
    /// </summary>
    public string EmployeeFullName { get; set; } = string.Empty;

    /// <summary>
    /// Табельный номер сотрудника.
    /// </summary>
    public string PersonnelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Наименование СИЗ (из карточки).
    /// </summary>
    public string PPEName { get; set; } = string.Empty;

    /// <summary>
    /// Номер партии / заводской №.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Размер (если применимо).
    /// </summary>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Количество (шт./компл.).
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Дата выдачи СИЗ сотруднику.
    /// </summary>
    public DateTime IssueDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Статус подписи (УКЭП или личная подпись).
    /// </summary>
    public string SignatureStatus { get; set; } = string.Empty;
}