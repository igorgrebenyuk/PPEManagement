namespace PPEManagement.Services.Contracts.Models.PPECard;

/// <summary>
/// Модель для обновления карточки средств индивидуальной защиты (СИЗ).
/// </summary>
public class PPECardUpdateModel
{
    /// <summary>
    /// Уникальный идентификатор карточки СИЗ.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование средства индивидуальной защиты.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Номер партии или сертификата.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Единица измерения (например, шт., пар).
    /// </summary>
    public string MeasureUnit { get; set; } = "шт.";

    /// <summary>
    /// Размер средства защиты.
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Количество единиц СИЗ.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата окончания срока годности или эксплуатации.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Дата следующей обязательной проверки или испытания.
    /// </summary>
    public DateTime NextCheckDate { get; set; }

    /// <summary>
    /// Текущий статус СИЗ (например, "На хранении", "Выдано").
    /// </summary>
    public string Status { get; set; } = "На хранении";
}