using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTABONEBILGIValidator : AbstractValidator<ENTABONEBILGI>
    {
        public ENTABONEBILGIValidator()
        {
            RuleFor(t => t.ABONENO).NotEmpty().WithMessage("ABONENO giriniz");
            RuleFor(t => t.ABONEKAYITNO).NotEmpty().WithMessage("ABONE giriniz");
            RuleFor(t => t.BLOKEACIKLAMA).Length(1, 100).WithMessage("BLOKEACIKLAMA 100 karakterden büyük olamaz");
            RuleFor(t => t.ABONENO).Length(1, 30).WithMessage("ABONENO 30 karakterden büyük olamaz");
            RuleFor(t => t.EPOSTA).Length(1, 200).WithMessage("EPOSTA 200 karakterden büyük olamaz");
            RuleFor(t => t.ADRES).Length(1, 200).WithMessage("ADRES 200 karakterden büyük olamaz");
        }
    }
}
