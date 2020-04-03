using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSGOREVValidator : AbstractValidator<SYSGOREV>
    {
        public SYSGOREVValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("Görev Adı giriniz");
            RuleFor(t => t.ACIKLAMA).NotEmpty().WithMessage("Açıklama giriniz");
           
            RuleFor(t => t.AD).Length(1, 40).WithMessage("Görev Adı 40 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 120).WithMessage("Açıklama 120 karakterden büyük olamaz");
        }
    }
}
