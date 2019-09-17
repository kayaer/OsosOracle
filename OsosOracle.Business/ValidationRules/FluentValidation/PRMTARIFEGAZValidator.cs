using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class PRMTARIFEGAZValidator : AbstractValidator<PRMTARIFEGAZ>
{
public PRMTARIFEGAZValidator()
{
RuleFor(t => t.TUKETIMLIMIT).NotEmpty().WithMessage("TUKETIMLIMIT giriniz");
RuleFor(t => t.SABAHSAAT).NotEmpty().WithMessage("SABAHSAAT giriniz");
RuleFor(t => t.AKSAMSAAT).NotEmpty().WithMessage("AKSAMSAAT giriniz");
RuleFor(t => t.PULSE).NotEmpty().WithMessage("PULSE giriniz");
RuleFor(t => t.KRITIKKREDI).NotEmpty().WithMessage("KRITIKKREDI giriniz");
RuleFor(t => t.SAYACTUR).NotEmpty().WithMessage("SAYACTUR giriniz");
RuleFor(t => t.SAYACCAP).NotEmpty().WithMessage("SAYACCAP giriniz");
RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("FIYAT1 giriniz");

RuleFor(t => t.AD).Length(1, 200).WithMessage("AD 200 karakterden büyük olamaz");
}
}
}
