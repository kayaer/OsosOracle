using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class NESNETIPValidator : AbstractValidator<NESNETIP>
{
public NESNETIPValidator()
{
RuleFor(t => t.ADI).NotEmpty().WithMessage("ADI giriniz");

RuleFor(t => t.ADI).Length(1, 80).WithMessage("ADI 80 karakterden büyük olamaz");
}
}
}
