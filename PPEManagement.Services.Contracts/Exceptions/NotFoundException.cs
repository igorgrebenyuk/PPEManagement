namespace PPEManagement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение объект не найден
/// </summary>
/// <param name="message">Сообщение</param>
public class NotFoundException(string message) : PPEManagementException(message);