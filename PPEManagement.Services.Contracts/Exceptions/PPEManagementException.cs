namespace PPEManagement.Services.Contracts.Exceptions;

/// <summary>
/// Общая ошибка
/// </summary>
public class PPEManagementException : Exception
{
    /// <summary>
    /// ctor без параметров
    /// </summary>
    public PPEManagementException()
    {
    }

    /// <summary>
    /// ctor с передачей сообщения
    /// </summary>
    /// <param name="message">Сообщение</param>
    public PPEManagementException(string message) : base(message)
    {
    }
}