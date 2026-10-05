namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Модель создания позиции (строки) в ведомости выдачи СИЗ.
/// </summary>
public class PPEStatementItemCreateModel
{
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// ФИО сотрудника (для отображения).
    /// </summary>
    public string EmployeeFullName { get; set; } = string.Empty;

    /// <summary>
    /// Табельный номер сотрудника.
    /// </summary>
    public string PersonnelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор карточки СИЗ.
    /// </summary>
    public Guid PPECardId { get; set; }

    /// <summary>
    /// Наименование СИЗ.
    /// </summary>
    public string PPEName { get; set; } = string.Empty;

    /// <summary>
    /// Номер партии.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Размер.
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Количество.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата выдачи.
    /// </summary>
    public DateTime IssueDate { get; set; } = DateTime.Today;
}