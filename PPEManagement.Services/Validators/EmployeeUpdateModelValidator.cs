using FluentValidation;
using PPEManagement.Services.Contracts.Models.Employee;
using PPEManagement.Services.Validators.Constraints;

namespace PPEManagement.Services.Validators;

/// <summary>
    /// Валидатор для <see cref="EmployeeUpdateModel"/>
    /// </summary>
    public class EmployeeUpdateModelValidator : AbstractValidator<EmployeeUpdateModel>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public EmployeeUpdateModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Идентификатор сотрудника обязателен");

            RuleFor(x => x.PersonnelNumber)
                .NotEmpty()
                .WithMessage("Табельный номер обязателен")
                .MaximumLength(EmployeeConstraints.PersonnelNumberMaxLength)
                .WithMessage($"Длина табельного номера не может превышать {EmployeeConstraints.PersonnelNumberMaxLength}");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Имя обязательно")
                .MaximumLength(EmployeeConstraints.FirstNameMaxLength)
                .WithMessage($"Длина имени не может превышать {EmployeeConstraints.FirstNameMaxLength}");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Фамилия обязательна")
                .MaximumLength(EmployeeConstraints.LastNameMaxLength)
                .WithMessage($"Длина фамилии не может превышать {EmployeeConstraints.LastNameMaxLength}");

            RuleFor(x => x.Department)
                .NotEmpty()
                .WithMessage("Подразделение обязательно")
                .MaximumLength(EmployeeConstraints.DepartmentMaxLength)
                .WithMessage($"Длина наименования подразделения не может превышать {EmployeeConstraints.DepartmentMaxLength}");

            RuleFor(x => x.Position)
                .NotEmpty()
                .WithMessage("Должность обязательна")
                .MaximumLength(EmployeeConstraints.PositionMaxLength)
                .WithMessage($"Длина наименования должности не может превышать {EmployeeConstraints.PositionMaxLength}");
        }
    }