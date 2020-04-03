using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSOPERASYONGOREVValidator : AbstractValidator<SYSOPERASYONGOREV>
    {
        public SYSOPERASYONGOREVValidator()
        {
            RuleFor(t => t.OPERASYONKAYITNO).NotEmpty().WithMessage("OPERASYON giriniz");
            RuleFor(t => t.GOREVKAYITNO).NotEmpty().WithMessage("GOREV giriniz");
            //RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");

        }
    }
}
