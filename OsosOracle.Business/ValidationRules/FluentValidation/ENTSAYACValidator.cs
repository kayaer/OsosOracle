using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTSAYACValidator : AbstractValidator<ENTSAYAC>
    {
        public ENTSAYACValidator()
        {
            RuleFor(t => t.SERINO).NotEmpty().WithMessage("SERINO giriniz");
            RuleFor(t => t.SAYACTUR).NotEmpty().WithMessage("SAYACTUR giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.SAYACID).Length(1, 12).WithMessage("SAYACID 12 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
        }
    }
}
