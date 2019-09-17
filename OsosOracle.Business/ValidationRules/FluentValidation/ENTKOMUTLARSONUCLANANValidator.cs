using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class ENTKOMUTLARSONUCLANANValidator : AbstractValidator<ENTKOMUTLARSONUCLANAN>
{
public ENTKOMUTLARSONUCLANANValidator()
{

RuleFor(t => t.KONSSERINO).Length(1, 30).WithMessage("KONSSERINO 30 karakterden büyük olamaz");
RuleFor(t => t.KOMUT).Length(1, 2500).WithMessage("KOMUT 2500 karakterden büyük olamaz");
RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
RuleFor(t => t.CEVAP).Length(1, 1000).WithMessage("CEVAP 1000 karakterden büyük olamaz");
}
}
}
