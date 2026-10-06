using AutoMapper;
using FluentValidation;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Contracts.Exceptions;
using PPEManagement.Services.Contracts.Models.PPEStatement;
using PPEManagement.Common;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Services;

/// <summary>
/// Сервис для работы с ведомостями выдачи СИЗ.
/// </summary>
public class PPEStatementService : IPPEStatementService
{
    private readonly IPPEStatementRepository ppeStatementRepository;
    private readonly IPPEStatementItemRepository itemRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IValidator<PPEStatementCreateModel> createValidator;

    public PPEStatementService(
        IPPEStatementRepository ppeStatementRepository,
        IPPEStatementItemRepository itemRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<PPEStatementCreateModel> createValidator)
    {
        this.ppeStatementRepository = ppeStatementRepository;
        this.itemRepository = itemRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.createValidator = createValidator;
    }

    public async Task<IReadOnlyCollection<PPEStatementModel>> GetPPEStatementsAsync(CancellationToken cancellationToken)
    {
        var entities = await ppeStatementRepository.GetPPEStatementsAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<PPEStatementModel>>(entities);
    }

    public async Task<PPEStatementDetailModel> GetPPEStatementByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await ppeStatementRepository.GetPPEStatementByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<PPEStatement>(id);
        }

        return mapper.Map<PPEStatementDetailModel>(entity);
    }

    public async Task<PPEStatementDetailModel> AddPPEStatementAsync(PPEStatementCreateModel ppeStatementModel, CancellationToken cancellationToken)
    {
        var validationResult = await createValidator.ValidateAsync(ppeStatementModel, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage
            }));
        }

        var entity = mapper.Map<PPEStatement>(ppeStatementModel);

        // Дата выдачи у всех позиций совпадает с датой ведомости
        foreach (var item in entity.Items)
        {
            item.IssueDate = entity.IssueDate;
        }

        // 1. Генерация уникального регистрационного номера ведомости
        entity.StatementNumber = $"ВЕД-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

        // 2. Пересчет итоговых сумм по категориям СИЗ (по количеству штук)
        RecalculateTotals(entity, entity.Items);

        ppeStatementRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PPEStatementDetailModel>(entity);
    }

    public async Task DeletePPEStatementAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await ppeStatementRepository.GetPPEStatementByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<PPEStatement>(id);
        }

        ppeStatementRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePPEStatementAsync(Guid id, PPEStatementCreateModel ppeStatementModel, CancellationToken cancellationToken)
    {
        // 1. Валидация входных данных
        var validationResult = await createValidator.ValidateAsync(ppeStatementModel, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage
            }));
        }

        // 2. Получение существующей сущности из БД
        var entity = await ppeStatementRepository.GetPPEStatementByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<PPEStatement>(id);
        }

        // 3. Запоминаем текущие позиции и отвязываем их от шапки (чтобы не было конфликта экземпляров в EF)
        var existingItems = entity.Items.ToList();
        foreach (var existing in existingItems)
        {
            existing.Statement = null!;
        }

        // 4. Обновляем шапку; список Items, который заполнил маппер, не используем
        mapper.Map(ppeStatementModel, entity);
        entity.Items = new List<PPEStatementItem>();

        // 5. Синхронизация позиций: есть Id -> обновляем, нет Id -> добавляем, пропали из запроса -> удаляем
        var finalItems = new List<PPEStatementItem>();
        foreach (var itemModel in ppeStatementModel.Items)
        {
            var existing = itemModel.Id.HasValue
                ? existingItems.FirstOrDefault(x => x.Id == itemModel.Id.Value)
                : null;

            if (existing is not null)
            {
                existing.EmployeeFullName = itemModel.EmployeeFullName;
                existing.PersonnelNumber = itemModel.PersonnelNumber;
                existing.PPEName = itemModel.PPEName;
                existing.BatchNumber = itemModel.BatchNumber;
                existing.Size = itemModel.Size ?? string.Empty;
                existing.Quantity = itemModel.Quantity;
                existing.IssueDate = entity.IssueDate;

                itemRepository.Update(existing);
                finalItems.Add(existing);
            }
            else
            {
                var newItem = mapper.Map<PPEStatementItem>(itemModel);
                newItem.StatementId = id;
                newItem.IssueDate = entity.IssueDate;

                itemRepository.Add(newItem);
                finalItems.Add(newItem);
            }
        }

        foreach (var removed in existingItems.Where(x => !finalItems.Contains(x)))
        {
            itemRepository.Delete(removed);
        }

        // 6. Пересчет итоговых сумм СИЗ (по количеству штук)
        RecalculateTotals(entity, finalItems);

        ppeStatementRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Пересчитывает итоги по категориям СИЗ: суммирует количество штук, а не число строк.
    /// Каждая позиция попадает ровно в одну категорию.
    /// </summary>
    private static void RecalculateTotals(PPEStatement entity, IReadOnlyCollection<PPEStatementItem> items)
    {
        static bool IsGasMask(PPEStatementItem i) =>
            i.PPEName.Contains("Противогаз", StringComparison.OrdinalIgnoreCase);

        static bool IsKimgz(PPEStatementItem i) =>
            i.PPEName.Contains("КИМГЗ", StringComparison.OrdinalIgnoreCase)
            || i.PPEName.Contains("Аптечка", StringComparison.OrdinalIgnoreCase);

        entity.TotalGasMasks = items.Where(IsGasMask).Sum(i => i.Quantity);
        entity.TotalKIMGZ = items.Where(i => !IsGasMask(i) && IsKimgz(i)).Sum(i => i.Quantity);
        entity.TotalOtherPPE = items.Where(i => !IsGasMask(i) && !IsKimgz(i)).Sum(i => i.Quantity);
    }
}
