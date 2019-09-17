using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class PRMTARIFEELKValidator : AbstractValidator<PRMTARIFEELK>
    {
        public PRMTARIFEELKValidator()
        {
            RuleFor(t => t.KREDIKATSAYI).NotEmpty().WithMessage("KREDIKATSAYI giriniz");
            RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
            RuleFor(t => t.YEDEKKREDI).NotEmpty().WithMessage("YEDEKKREDI giriniz");
            RuleFor(t => t.KRITIKKREDI).NotEmpty().WithMessage("KRITIKKREDI giriniz");
            RuleFor(t => t.CARPAN).NotEmpty().WithMessage("CARPAN giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("FIYAT1 giriniz");
            RuleFor(t => t.FIYAT2).NotEmpty().WithMessage("FIYAT2 giriniz");
            RuleFor(t => t.FIYAT3).NotEmpty().WithMessage("FIYAT3 giriniz");
            //RuleFor(t => t.LIMIT1).NotEmpty().WithMessage("LIMIT1 giriniz");
            RuleFor(t => t.LIMIT2).NotEmpty().WithMessage("LIMIT2 giriniz");
            RuleFor(t => t.YUKLEMELIMIT).NotEmpty().WithMessage("YUKLEMELIMIT giriniz");
            RuleFor(t => t.SABAHSAAT).NotEmpty().WithMessage("SABAHSAAT giriniz");
            RuleFor(t => t.AKSAMSAAT).NotEmpty().WithMessage("AKSAMSAAT giriniz");
            RuleFor(t => t.HAFTASONUAKSAM).NotEmpty().WithMessage("HAFTASONUAKSAM giriniz");
            RuleFor(t => t.SABITUCRET).NotEmpty().WithMessage("SABITUCRET giriniz");
            RuleFor(t => t.BAYRAM1GUN).NotEmpty().WithMessage("BAYRAM1GUN giriniz");
            RuleFor(t => t.BAYRAM1AY).NotEmpty().WithMessage("BAYRAM1AY giriniz");
            RuleFor(t => t.BAYRAM1SURE).NotEmpty().WithMessage("BAYRAM1SURE giriniz");
            RuleFor(t => t.BAYRAM2GUN).NotEmpty().WithMessage("BAYRAM2GUN giriniz");
            RuleFor(t => t.BAYRAM2AY).NotEmpty().WithMessage("BAYRAM2AY giriniz");
            RuleFor(t => t.BAYRAM2SURE).NotEmpty().WithMessage("BAYRAM2SURE giriniz");

            RuleFor(t => t.AD).Length(1, 200).WithMessage("AD 200 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 400).WithMessage("ACIKLAMA 400 karakterden büyük olamaz");
        }
    }
}
