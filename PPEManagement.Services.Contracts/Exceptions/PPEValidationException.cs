using PPEManagement.Common;

namespace PPEManagement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение при попытке валидации
/// </summary>
public class PPEValidationException : PPEManagementException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="errors">Ошибки</param>
    public PPEValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}