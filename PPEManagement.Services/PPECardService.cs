using AutoMapper;
using FluentValidation;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Contracts.Exceptions;
using PPEManagement.Services.Contracts.Models.PPECard;
using PPEManagement.Common;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Services
{
    /// <summary>
    /// Сервис для работы с карточками СИЗ.
    /// </summary>
    public class PPECardService : IPPECardService
    {
        private readonly IPPECardRepository ppeCardRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<PPECardCreateModel> createValidator;
        private readonly IValidator<PPECardUpdateModel> updateValidator;

        public PPECardService(
            IPPECardRepository ppeCardRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<PPECardCreateModel> createValidator,
            IValidator<PPECardUpdateModel> updateValidator)
        {
            this.ppeCardRepository = ppeCardRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
        }

        public async Task<IReadOnlyCollection<PPECardModel>> GetPPECardsAsync(CancellationToken cancellationToken)
        {
            var entities = await ppeCardRepository.GetPPECardsAsync(cancellationToken);
            return mapper.Map<IReadOnlyCollection<PPECardModel>>(entities);
        }

        public async Task<PPECardModel> GetPPECardByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await ppeCardRepository.GetPPECardByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<PPECard>(id);
            }

            return mapper.Map<PPECardModel>(entity);
        }

        public async Task<PPECardModel> AddPPECardAsync(PPECardCreateModel ppeCardModel, CancellationToken cancellationToken)
        {
            var validationResult = await createValidator.ValidateAsync(ppeCardModel, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }));
            }

            var entity = mapper.Map<PPECard>(ppeCardModel);
            ppeCardRepository.Add(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<PPECardModel>(entity);
        }

        public async Task UpdatePPECardAsync(PPECardUpdateModel ppeCardModel, CancellationToken cancellationToken)
        {
            var validationResult = await updateValidator.ValidateAsync(ppeCardModel, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }));
            }

            var entity = await ppeCardRepository.GetPPECardByIdAsync(ppeCardModel.Id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<PPECard>(ppeCardModel.Id);
            }

            mapper.Map(ppeCardModel, entity);
            ppeCardRepository.Update(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePPECardAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await ppeCardRepository.GetPPECardByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<PPECard>(id);
            }

            ppeCardRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}