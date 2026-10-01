using PPEManagement.Dal.Contracts;

namespace PPEManagement.Entities;

/// <summary>
/// Сущность номенклатурной карточки СИЗ на складе.
/// </summary>
public class PPECard : BaseAuditEntity
{
    /// <summary>
    /// Наименование средства индивидуальной защиты (например, "Противогаз ГП-7").
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// ФИО сотрудника, которому выдано СИЗ (заполняется при списании из резерва).
    /// </summary>
    public string IssuedToEmployeeFullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Номер партии или заводской номер изделия.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Размер или рост изделия (0 - если размер не предусмотрен).
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Доступный остаток на складе (в шт. или компл.).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата окончания срока годности изделия.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Срок следующей лабораторной проверки или освежения СИЗ.
    /// </summary>
    public DateTime NextCheckDate { get; set; }

    /// <summary>
    /// Текущий статус карточки (например, "На хранении", "Выдано", "Списано").
    /// </summary>
    public string Status { get; set; } = "На хранении";
}