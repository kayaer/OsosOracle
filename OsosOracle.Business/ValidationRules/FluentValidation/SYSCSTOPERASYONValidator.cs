using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class SYSCSTOPERASYONValidator : AbstractValidator<SYSCSTOPERASYON>
{
public SYSCSTOPERASYONValidator()
{
RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
RuleFor(t => t.ACIKLAMA).NotEmpty().WithMessage("ACIKLAMA giriniz");
RuleFor(t => t.OPERASYONTUR).NotEmpty().WithMessage("OPERASYONTUR giriniz");
RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");

RuleFor(t => t.AD).Length(1, 40).WithMessage("AD 40 karakterden büyük olamaz");
RuleFor(t => t.ACIKLAMA).Length(1, 120).WithMessage("ACIKLAMA 120 karakterden büyük olamaz");
}
}
}
