using PPEManagement.Services.Contracts.Models.PPEStatement;

namespace PPEManagement.Services.Contracts;

/// <summary>
/// Интерфейс сервиса для работы с ведомостями выдачи СИЗ.
/// </summary>
public interface IPPEStatementService
{
    /// <summary>
    /// Получает список всех ведомостей выдачи СИЗ.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Коллекция кратких моделей ведомостей.</returns>
    Task<IReadOnlyCollection<PPEStatementModel>> GetPPEStatementsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получает ведомость выдачи СИЗ с позициями по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор ведомости.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Детальная модель ведомости СИЗ.</returns>
    Task<PPEStatementDetailModel> GetPPEStatementByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создает новую ведомость выдачи СИЗ.
    /// </summary>
    /// <param name="ppeStatementModel">Модель создания ведомости.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Созданная детальная модель ведомости СИЗ.</returns>
    Task<PPEStatementDetailModel> AddPPEStatementAsync(PPEStatementCreateModel ppeStatementModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет ведомость выдачи СИЗ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор ведомости.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task DeletePPEStatementAsync(Guid id, CancellationToken cancellationToken);
}