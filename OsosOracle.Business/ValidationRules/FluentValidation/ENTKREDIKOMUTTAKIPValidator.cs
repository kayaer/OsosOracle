using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class ENTKREDIKOMUTTAKIPValidator : AbstractValidator<ENTKREDIKOMUTTAKIP>
{
public ENTKREDIKOMUTTAKIPValidator()
{
RuleFor(t => t.GUIDID).NotEmpty().WithMessage("GUIDID giriniz");

RuleFor(t => t.KONSSERINO).Length(1, 30).WithMessage("KONSSERINO 30 karakterden büyük olamaz");
RuleFor(t => t.KOMUT).Length(1, 2500).WithMessage("KOMUT 2500 karakterden büyük olamaz");
RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
RuleFor(t => t.DURUM).Length(1, 1).WithMessage("DURUM 1 karakterden büyük olamaz");
}
}
}
