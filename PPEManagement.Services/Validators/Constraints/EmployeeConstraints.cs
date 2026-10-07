namespace PPEManagement.Services.Validators.Constraints;

/// <summary>
/// Ограничения для полей сотрудника
/// </summary>
public static class EmployeeConstraints
{
    /// <summary>
    /// Максимальная длина табельного номера.
    /// </summary>
    public const int PersonnelNumberMaxLength = 20;

    /// <summary>
    /// Максимальная длина имени.
    /// </summary>
    public const int FirstNameMaxLength = 100;

    /// <summary>
    /// Максимальная длина фамилии.
    /// </summary>
    public const int LastNameMaxLength = 100;

    /// <summary>
    /// Максимальная длина отчества.
    /// </summary>
    public const int MiddleNameMaxLength = 100;

    /// <summary>
    /// Максимальная длина названия подразделения.
    /// </summary>
    public const int DepartmentMaxLength = 150;

    /// <summary>
    /// Максимальная длина названия должности.
    /// </summary>
    public const int PositionMaxLength = 150;
}