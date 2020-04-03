using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class NESNEDEGERValidator : AbstractValidator<NESNEDEGER>
{
public NESNEDEGERValidator()
{
RuleFor(t => t.NESNETIPKAYITNO).NotEmpty().WithMessage("NESNETIP giriniz");
RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
RuleFor(t => t.SIRANO).NotEmpty().WithMessage("SIRANO giriniz");

RuleFor(t => t.AD).Length(1, 80).WithMessage("AD 80 karakterden büyük olamaz");
RuleFor(t => t.DEGER).Length(1, 80).WithMessage("DEGER 80 karakterden büyük olamaz");
RuleFor(t => t.BILGI).Length(1, 80).WithMessage("BILGI 80 karakterden büyük olamaz");
}
}
}
