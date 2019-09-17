using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class RPTDASHBOARDValidator : AbstractValidator<RPTDASHBOARD>
{
public RPTDASHBOARDValidator()
{
RuleFor(t => t.TARIH).NotEmpty().WithMessage("TARIH giriniz");
RuleFor(t => t.ADET).NotEmpty().WithMessage("ADET giriniz");

}
}
}
