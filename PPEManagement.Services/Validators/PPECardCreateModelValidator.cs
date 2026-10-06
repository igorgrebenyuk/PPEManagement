using FluentValidation;
using PPEManagement.Services.Contracts.Models.PPECard;

namespace PPEManagement.Services.Validators;

/// <summary>
/// Валидатор модели создания карточки СИЗ
/// </summary>
public class PPECardCreateModelValidator : AbstractValidator<PPECardCreateModel>
{
    public PPECardCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Наименование СИЗ не может быть пустым")
            .MaximumLength(250).WithMessage("Наименование СИЗ не должно превышать 250 символов");

        RuleFor(x => x.BatchNumber)
            .NotEmpty().WithMessage("Номер партии/сертификата не может быть пустым")
            .MaximumLength(100).WithMessage("Номер партии не должен превышать 100 символов");

        RuleFor(x => x.MeasureUnit)
            .NotEmpty().WithMessage("Единица измерения не может быть пустой")
            .MaximumLength(50).WithMessage("Единица измерения не должна превышать 50 символов");

        RuleFor(x => x.WearPeriodMonths)
            .GreaterThan(0).WithMessage("Срок носки должен быть больше 0 месяцев")
            .LessThanOrEqualTo(120).WithMessage("Срок носки не может превышать 120 месяцев (10 лет)");
    }
}