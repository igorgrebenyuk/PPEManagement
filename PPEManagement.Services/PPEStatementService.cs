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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<PPEStatementCreateModel> createValidator;

        public PPEStatementService(
            IPPEStatementRepository ppeStatementRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<PPEStatementCreateModel> createValidator)
        {
            this.ppeStatementRepository = ppeStatementRepository;
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

            // 1. Генерация уникального регистрационного номера ведомости
            entity.StatementNumber = $"ВЕД-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

            // 2. Автоматический пересчет итоговых сумм по категориям СИЗ в позициях ведомости
            entity.TotalGasMasks = entity.Items.Count(i => i.PPEName.Contains("Противогаз", StringComparison.OrdinalIgnoreCase));
            entity.TotalKIMGZ = entity.Items.Count(i => i.PPEName.Contains("КИМГЗ", StringComparison.OrdinalIgnoreCase) || i.PPEName.Contains("Аптечка", StringComparison.OrdinalIgnoreCase));
            entity.TotalOtherPPE = entity.Items.Count - (entity.TotalGasMasks + entity.TotalKIMGZ);

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

            // 3. Обновление полей сущности из модели
            mapper.Map(ppeStatementModel, entity);

            // 4. Пересчет итоговых сумм СИЗ
            entity.TotalGasMasks = entity.Items.Count(i => i.PPEName.Contains("Противогаз", StringComparison.OrdinalIgnoreCase));
            entity.TotalKIMGZ = entity.Items.Count(i => i.PPEName.Contains("КИМГЗ", StringComparison.OrdinalIgnoreCase) || i.PPEName.Contains("Аптечка", StringComparison.OrdinalIgnoreCase));
            entity.TotalOtherPPE = entity.Items.Count - (entity.TotalGasMasks + entity.TotalKIMGZ);

            // 5. Сохранение изменений в БД
            ppeStatementRepository.Update(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
