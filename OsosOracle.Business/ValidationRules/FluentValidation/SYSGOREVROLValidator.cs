using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class SYSGOREVROLValidator : AbstractValidator<SYSGOREVROL>
{
public SYSGOREVROLValidator()
{
RuleFor(t => t.GOREVKAYITNO).NotEmpty().WithMessage("GOREV giriniz");
RuleFor(t => t.ROLKAYITNO).NotEmpty().WithMessage("ROL giriniz");
RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");

}
}
}
