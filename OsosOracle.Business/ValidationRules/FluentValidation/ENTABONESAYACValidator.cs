using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTABONESAYACValidator : AbstractValidator<ENTABONESAYAC>
    {
        public ENTABONESAYACValidator()
        {
            RuleFor(t => t.ABONEKAYITNO).NotEmpty().WithMessage("ABONE giriniz");
            RuleFor(t => t.SAYACKAYITNO).NotEmpty().WithMessage("SAYAC giriniz");

        }
    }
}
