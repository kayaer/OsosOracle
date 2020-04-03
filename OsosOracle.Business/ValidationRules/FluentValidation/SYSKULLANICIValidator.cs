using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSKULLANICIValidator : AbstractValidator<SYSKULLANICI>
    {
        public SYSKULLANICIValidator()
        {
            RuleFor(t => t.KULLANICIAD).NotEmpty().WithMessage("Kullanıcı Adı giriniz");
            RuleFor(t => t.SIFRE).NotEmpty().WithMessage("Şifre giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.DIL).NotEmpty().WithMessage("DIL giriniz");
            RuleFor(t => t.KULLANICIAD).Length(1, 40).WithMessage("Kullanıcı Adı 40 karakterden büyük olamaz");
            RuleFor(t => t.SIFRE).Length(1, 40).WithMessage("Şifre 40 karakterden büyük olamaz");
            RuleFor(t => t.AD).Length(1, 50).WithMessage("Ad 50 karakterden büyük olamaz");
            RuleFor(t => t.SOYAD).Length(1, 50).WithMessage("Soyad 50 karakterden büyük olamaz");
          
        }
    }
}
