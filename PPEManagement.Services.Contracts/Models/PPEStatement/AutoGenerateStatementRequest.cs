namespace PPEManagement.Services.Contracts.Models.PPEStatement;

/// <summary>
/// Запрос на автоматическое формирование ведомости выдачи СИЗ.
/// </summary>
public class AutoGenerateStatementRequest
{
    /// <summary>
    /// Идентификатор подразделения (для фильтрации сотрудников).
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// Наименование организации (автозаполнение или из настроек).
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Наименование подразделения или формирования НФГО.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи (Плановая замена / Тренировка / Введение Плана ГО).
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// ФИО и должность ответственного лица.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;
}

/// <summary>
/// Модель представления для отображения ведомости на фронтенде (Bootstrap).
/// </summary>
public class PPEStatementViewModel
{
    /// <summary>
    /// Наименование организации.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Подразделение или формирование НФГО.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Основание выдачи.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Дата формирования документа.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Список позиций ведомости.
    /// </summary>
    public List<PPEStatementItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Итоговое количество выданных противогазов.
    /// </summary>
    public int TotalGasMasks { get; set; }

    /// <summary>
    /// Итоговое количество выданных аптечек КИМГЗ.
    /// </summary>
    public int TotalKIMGZ { get; set; }

    /// <summary>
    /// Итоговое количество прочих СИЗ.
    /// </summary>
    public int TotalOtherPPE { get; set; }

    /// <summary>
    /// Ответственное лицо.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Срок следующей проверки (освежения) партии (берется из карточки СИЗ).
    /// </summary>
    public string NextCheckDate { get; set; } = string.Empty;
}

/// <summary>
/// Модель строки позиции ведомости для отображения.
/// </summary>
public class PPEStatementItemViewModel
{
    /// <summary>
    /// Порядковый номер в таблице.
    /// </summary>
    public int RowNumber { get; set; }

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
    /// Номер партии или заводской номер.
    /// </summary>
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Размер СИЗ.
    /// </summary>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Количество (шт./компл.).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата выдачи.
    /// </summary>
    public DateTime IssueDate { get; set; }
}