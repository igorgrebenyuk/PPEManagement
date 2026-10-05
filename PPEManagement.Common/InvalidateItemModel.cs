namespace PPEManagement.Common;

/// <summary>
/// Модель ошибки валидации
/// </summary>
public class InvalidateItemModel
{
    /// <summary>
    /// Имя поля
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
}