namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Модель позиции выдачи СИЗ в ведомости
/// </summary>
public class PPEStatementItemModel
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор СИЗ
    /// </summary>
    public Guid PPECardId { get; set; }

    /// <summary>
    /// Наименование СИЗ
    /// </summary>
    public string PPEName { get; set; } = string.Empty;

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string MeasureUnit { get; set; } = string.Empty;

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; set; }
}