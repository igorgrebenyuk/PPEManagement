using FluentValidation;
using PPEManagement.Services.Contracts.Models.PPEStatement;

namespace PPEManagement.Services.Validators;

public class PPEStatementCreateModelValidator : AbstractValidator<PPEStatementCreateModel>
{
    public PPEStatementCreateModelValidator()
    {
        RuleFor(x => x.OrganizationName).NotEmpty().WithMessage("Наименование организации обязательно.");
        RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("Подразделение обязательно.");
        RuleFor(x => x.ResponsiblePerson).NotEmpty().WithMessage("Укажите ответственное лицо.");
        RuleFor(x => x.Items).NotEmpty().WithMessage("Ведомость должна содержать хотя бы одну позицию.");
    }
}