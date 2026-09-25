using PPEManagement.Dal.Contracts;

namespace PPEManagement.Entities;

/// <summary>
/// Сущность сотрудника из штатного расписания.
/// </summary>
public class Employee : BaseAuditEntity
{
    /// <summary>
    /// Табельный номер сотрудника.
    /// </summary>
    public string PersonnelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Полное имя сотрудника (ФИО).
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Наименование подразделения или цеха.
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Должность сотрудника.
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Размер лицевой части противогаза (1, 2, 3, 4).
    /// </summary>
    public int GasMaskSize { get; set; }

    /// <summary>
    /// Размер защитного костюма (например, Л-1).
    /// </summary>
    public int SuitSize { get; set; }

    /// <summary>
    /// Флаг принадлежности сотрудника к нештатным формированиям гражданской обороны (НФГО).
    /// </summary>
    public bool IsNFGO { get; set;}
}