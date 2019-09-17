using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTHUSAYACValidator : AbstractValidator<ENTHUSAYAC>
    {
        public ENTHUSAYACValidator()
        {
            RuleFor(t => t.HUKAYITNO).NotEmpty().WithMessage("HU giriniz");
            RuleFor(t => t.SAYACKAYITNO).NotEmpty().WithMessage("SAYAC giriniz");
            RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");

        }
    }
}
