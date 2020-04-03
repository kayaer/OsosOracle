using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSROLValidator : AbstractValidator<SYSROL>
    {
        public SYSROLValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("Rol adı giriniz");
           

            RuleFor(t => t.AD).Length(1, 40).WithMessage("Rol adı 40 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 120).WithMessage("Açıklama 120 karakterden büyük olamaz");
        }
    }
}
