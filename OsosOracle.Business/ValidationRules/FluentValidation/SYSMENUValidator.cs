using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class SYSMENUValidator : AbstractValidator<SYSMENU>
    {
        public SYSMENUValidator()
        {
            RuleFor(t => t.TR).NotEmpty().WithMessage("TR giriniz");
            RuleFor(t => t.EN).NotEmpty().WithMessage("EN giriniz");
            RuleFor(t => t.MENUORDER).NotEmpty().WithMessage("Sıra numarası giriniz");
            RuleFor(t => t.TR).Length(1, 100).WithMessage("TR 100 karakterden büyük olamaz");
            RuleFor(t => t.EN).Length(1, 100).WithMessage("EN 100 karakterden büyük olamaz");
            RuleFor(t => t.YEREL).Length(1, 100).WithMessage("YEREL 100 karakterden büyük olamaz");
            RuleFor(t => t.EXTERNALURL).Length(1, 200).WithMessage("EXTERNALURL 200 karakterden büyük olamaz");
            RuleFor(t => t.AREA).Length(1, 200).WithMessage("AREA 200 karakterden büyük olamaz");
            RuleFor(t => t.ACTION).Length(1, 200).WithMessage("ACTION 200 karakterden büyük olamaz");
            RuleFor(t => t.CONTROLLER).Length(1, 200).WithMessage("CONTROLLER 200 karakterden büyük olamaz");
            RuleFor(t => t.PARAMETERS).Length(1, 200).WithMessage("PARAMETERS 200 karakterden büyük olamaz");
            RuleFor(t => t.ICON).Length(1, 100).WithMessage("ICON 100 karakterden büyük olamaz");
        }
    }
}
