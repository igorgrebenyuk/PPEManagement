namespace PPEManagement.Services.Contracts.Models.PPECard;

/// <summary>
/// Модель создания карточки СИЗ
/// </summary>
public class PPECardCreateModel
{
    /// <summary>
    /// Наименование СИЗ
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Номер партии / Сертификат
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string MeasureUnit { get; set; } = string.Empty;

    /// <summary>
    /// Срок носки (в месяцах)
    /// </summary>
    public int WearPeriodMonths { get; set; }
}