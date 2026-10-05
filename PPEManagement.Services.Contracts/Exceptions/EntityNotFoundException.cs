namespace PPEManagement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение сущность не найдена
/// </summary>
/// <typeparam name="T">Сущность</typeparam>
/// <param name="id">Идентификатор</param>
public class EntityNotFoundException<T>(Guid id) : NotFoundException($"Сущность {typeof(T).Name} с идентификатором {id} не найдена");