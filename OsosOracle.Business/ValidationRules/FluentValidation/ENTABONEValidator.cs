using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTABONEValidator : AbstractValidator<ENTABONE>
    {
        public ENTABONEValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
            RuleFor(t => t.SOYAD).NotEmpty().WithMessage("SOYAD giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.KURUMKAYITNO).NotEmpty().WithMessage("KURUM giriniz");

            RuleFor(t => t.AD).Length(1, 100).WithMessage("AD 100 karakterden büyük olamaz");
            RuleFor(t => t.SOYAD).Length(1, 100).WithMessage("SOYAD 100 karakterden büyük olamaz");
        }
    }
}
