using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSGOREVROLValidator : AbstractValidator<SYSGOREVROL>
    {
        public SYSGOREVROLValidator()
        {
            RuleFor(t => t.GOREVKAYITNO).NotEmpty().WithMessage("G�rev giriniz");
            RuleFor(t => t.ROLKAYITNO).NotEmpty().WithMessage("Rol giriniz");
            //RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");

        }
    }
}
