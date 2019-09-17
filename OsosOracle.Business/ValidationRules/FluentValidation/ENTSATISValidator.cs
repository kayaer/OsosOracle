using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTSATISValidator : AbstractValidator<ENTSATIS>
    {
        public ENTSATISValidator()
        {
            RuleFor(t => t.ABONEKAYITNO).NotEmpty().WithMessage("ABONE giriniz");
            RuleFor(t => t.SAYACKAYITNO).NotEmpty().WithMessage("SAYAC giriniz");
            RuleFor(t => t.ODEME).NotEmpty().WithMessage("ODEME giriniz");


        }
    }
}
