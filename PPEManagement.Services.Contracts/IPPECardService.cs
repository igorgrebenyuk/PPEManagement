using PPEManagement.Services.Contracts.Models.PPECard;

namespace PPEManagement.Services.Contracts;

/// <summary>
/// Сервис для работы с номенклатурой СИЗ
/// </summary>
public interface IPPECardService
{
    /// <summary>
    /// Получение списка карточек СИЗ
    /// </summary>
    Task<IReadOnlyCollection<PPECardModel>> GetPPECardsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение карточки СИЗ по идентификатору
    /// </summary>
    Task<PPECardModel> GetPPECardByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление карточки СИЗ
    /// </summary>
    Task<PPECardModel> AddPPECardAsync(PPECardCreateModel ppeCardModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление карточки СИЗ
    /// </summary>
    Task UpdatePPECardAsync(PPECardUpdateModel ppeCardModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление карточки СИЗ
    /// </summary>
    Task DeletePPECardAsync(Guid id, CancellationToken cancellationToken);
}