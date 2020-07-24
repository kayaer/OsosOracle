using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class EntIsEmriValidator : AbstractValidator<EntIsEmri>
    {
        public EntIsEmriValidator()
        {
           
            RuleFor(t => t.SayacKayitNo).NotEmpty().WithMessage("Sayaç Seçiniz");
            RuleFor(t => t.IsEmriKayitNo).NotEmpty().WithMessage("İş Emri Seçiniz");

        }
    }
}
