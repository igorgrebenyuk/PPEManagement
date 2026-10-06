using FluentValidation;
using PPEManagement.Services.Contracts.Models.PPECard;

namespace PPEManagement.Services.Validators;

public class PPECardUpdateModelValidator : AbstractValidator<PPECardUpdateModel>
{
    public PPECardUpdateModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Идентификатор карточки обязателен.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Наименование СИЗ обязательно.");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Количество не может быть отрицательным.");
        RuleFor(x => x.ExpirationDate).GreaterThan(DateTime.MinValue).WithMessage("Укажите корректный срок годности.");
    }
}