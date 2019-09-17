using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class CSTHUMODELValidator : AbstractValidator<CSTHUMODEL>
{
public CSTHUMODELValidator()
{
RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");

RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
RuleFor(t => t.YAZILIMVERSIYON).Length(1, 20).WithMessage("YAZILIMVERSIYON 20 karakterden büyük olamaz");
RuleFor(t => t.CONTROLLER).Length(1, 50).WithMessage("CONTROLLER 50 karakterden büyük olamaz");
RuleFor(t => t.AD).Length(1, 100).WithMessage("AD 100 karakterden büyük olamaz");
}
}
}
