using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class ENTTUKETIMSUValidator : AbstractValidator<ENTTUKETIMSU>
{
public ENTTUKETIMSUValidator()
{
RuleFor(t => t.SAYACID).NotEmpty().WithMessage("SAYACID giriniz");

RuleFor(t => t.SAYACID).Length(1, 12).WithMessage("SAYACID 12 karakterden büyük olamaz");
}
}
}
