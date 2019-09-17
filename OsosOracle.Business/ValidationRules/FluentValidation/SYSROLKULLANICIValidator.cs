using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSROLKULLANICIValidator : AbstractValidator<SYSROLKULLANICI>
    {
        public SYSROLKULLANICIValidator()
        {
            RuleFor(t => t.KULLANICIKAYITNO).NotEmpty().WithMessage("KULLANICI giriniz");
            RuleFor(t => t.ROLKAYITNO).NotEmpty().WithMessage("ROL giriniz");

        }
    }
}
