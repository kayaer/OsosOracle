using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class ENTHABERLESMEUNITESIValidator : AbstractValidator<ENTHABERLESMEUNITESI>
    {
        public ENTHABERLESMEUNITESIValidator()
        {
            RuleFor(t => t.SERINO).NotEmpty().WithMessage("SERINO giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");
            RuleFor(t => t.KURUMKAYITNO).NotEmpty().WithMessage("KURUM giriniz");

            RuleFor(t => t.SERINO).Length(1, 30).WithMessage("SERINO 30 karakterden büyük olamaz");
            RuleFor(t => t.SIMTELNO).Length(1, 11).WithMessage("SIMTELNO 11 karakterden büyük olamaz");
            RuleFor(t => t.IP).Length(1, 50).WithMessage("IP 50 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
        }
    }
}
