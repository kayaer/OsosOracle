using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class SYSKULLANICIDETAYValidator : AbstractValidator<SYSKULLANICIDETAY>
{
public SYSKULLANICIDETAYValidator()
{
RuleFor(t => t.EPOSTA).NotEmpty().WithMessage("EPOSTA giriniz");
RuleFor(t => t.KULLANICIKAYITNO).NotEmpty().WithMessage("KULLANICI giriniz");

RuleFor(t => t.EPOSTA).Length(1, 40).WithMessage("EPOSTA 40 karakterden büyük olamaz");
}
}
}
