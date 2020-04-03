using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class CONDILValidator : AbstractValidator<CONDIL>
{
public CONDILValidator()
{
RuleFor(t => t.DIL).NotEmpty().WithMessage("DIL giriniz");

RuleFor(t => t.DIL).Length(1, 200).WithMessage("DIL 200 karakterden büyük olamaz");
}
}
}
