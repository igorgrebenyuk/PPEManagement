namespace PPEManagement.Services.Validators.Constraints;

/// <summary>
/// Ограничения для карточек СИЗ
/// </summary>
public static class PPECardConstraints
{
    /// <summary>
    /// Максимальная длина наименования СИЗ.
    /// </summary>
    public const int NameMaxLength = 200;

    /// <summary>
    /// Максимальная длина номера партии.
    /// </summary>
    public const int BatchNumberMaxLength = 50;

    /// <summary>
    /// Максимальная длина единицы измерения.
    /// </summary>
    public const int MeasureUnitMaxLength = 20;
}