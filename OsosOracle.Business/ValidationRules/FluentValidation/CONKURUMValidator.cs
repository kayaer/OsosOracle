using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class CONKURUMValidator : AbstractValidator<CONKURUM>
    {
        public CONKURUMValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
            RuleFor(t => t.ACIKLAMA).NotEmpty().WithMessage("ACIKLAMA giriniz");
           
            RuleFor(t => t.AD).Length(1, 40).WithMessage("AD 40 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 120).WithMessage("ACIKLAMA 120 karakterden büyük olamaz");
        }
    }
}
