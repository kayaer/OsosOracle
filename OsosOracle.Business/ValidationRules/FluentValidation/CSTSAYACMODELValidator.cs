using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class CSTSAYACMODELValidator : AbstractValidator<CSTSAYACMODEL>
    {
        public CSTSAYACMODELValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
            RuleFor(t => t.SayacTuruKayitNo).NotEmpty().WithMessage("Sayaç Türü boş olamaz");
            RuleFor(t => t.AD).Length(1, 100).WithMessage("AD 100 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
         
        }
    }
}
