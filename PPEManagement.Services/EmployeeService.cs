using AutoMapper;
using FluentValidation;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Contracts.Exceptions;
using PPEManagement.Services.Contracts.Models.Employee;
using PPEManagement.Common;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Services
{
    /// <summary>
    /// Сервис для работы с сотрудниками.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<EmployeeCreateModel> createValidator;
        private readonly IValidator<EmployeeUpdateModel> updateValidator;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EmployeeCreateModel> createValidator,
            IValidator<EmployeeUpdateModel> updateValidator)
        {
            this.employeeRepository = employeeRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
        }

        public async Task<IReadOnlyCollection<EmployeeModel>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var entities = await employeeRepository.GetEmployeesAsync(cancellationToken);
            return mapper.Map<IReadOnlyCollection<EmployeeModel>>(entities);
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<Employee>(id);
            }

            return mapper.Map<EmployeeModel>(entity);
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateModel employeeModel, CancellationToken cancellationToken)
        {
            var validationResult = await createValidator.ValidateAsync(employeeModel, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }));
            }

            var entity = mapper.Map<Employee>(employeeModel);
            employeeRepository.Add(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<EmployeeModel>(entity);
        }

        public async Task UpdateEmployeeAsync(EmployeeUpdateModel employeeModel, CancellationToken cancellationToken)
        {
            var validationResult = await updateValidator.ValidateAsync(employeeModel, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new PPEValidationException(validationResult.Errors.Select(e => new InvalidateItemModel
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }));
            }

            var entity = await employeeRepository.GetEmployeeByIdAsync(employeeModel.Id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<Employee>(employeeModel.Id);
            }

            mapper.Map(employeeModel, entity);
            employeeRepository.Update(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException<Employee>(id);
            }

            employeeRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}