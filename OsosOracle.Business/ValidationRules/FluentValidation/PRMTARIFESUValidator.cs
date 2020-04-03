using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class PRMTARIFESUValidator : AbstractValidator<PRMTARIFESU>
    {
        public PRMTARIFESUValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("Tarife Adı giriniz");
            //RuleFor(t => t.YEDEKKREDI).NotEmpty().WithMessage("Yedek Kredi giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("Durum giriniz");
            RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("Fiyat1 giriniz");
            RuleFor(t => t.FIYAT2).NotEmpty().WithMessage("Fiyat2 giriniz");
            RuleFor(t => t.FIYAT3).NotEmpty().WithMessage("Fiyat3 giriniz");
            RuleFor(t => t.FIYAT4).NotEmpty().WithMessage("Fiyat4 giriniz");
            RuleFor(t => t.FIYAT5).NotEmpty().WithMessage("Fiyat5 giriniz");
            RuleFor(t => t.LIMIT1).NotEmpty().WithMessage("Limit1 giriniz");
            RuleFor(t => t.LIMIT2).NotEmpty().WithMessage("Limit2 giriniz");
            RuleFor(t => t.LIMIT3).NotEmpty().WithMessage("Limit3 giriniz");
            RuleFor(t => t.LIMIT4).NotEmpty().WithMessage("Limit4 giriniz");
            //RuleFor(t => t.TUKETIMKATSAYI).NotEmpty().WithMessage("Tüketim katsayı giriniz");
            //RuleFor(t => t.KREDIKATSAYI).NotEmpty().WithMessage("Kredi katsayı giriniz");
            //RuleFor(t => t.SABITUCRET).NotEmpty().WithMessage("Sabit ücret giriniz");
            RuleFor(t => t.SAYACCAP).NotEmpty().WithMessage("Sayaç çapı giriniz");
            RuleFor(t => t.BAYRAM1AY).NotEmpty().WithMessage("Bayram 1 ay giriniz");
            RuleFor(t => t.BAYRAM1GUN).NotEmpty().WithMessage("Bayram 1 gün giriniz");
            RuleFor(t => t.BAYRAM1SURE).NotEmpty().WithMessage("Bayram 1 süre giriniz");
            RuleFor(t => t.BAYRAM2AY).NotEmpty().WithMessage("Bayram 2 ay giriniz");
            RuleFor(t => t.BAYRAM2GUN).NotEmpty().WithMessage("Bayram 2 gün giriniz");
            RuleFor(t => t.BAYRAM2SURE).NotEmpty().WithMessage("Bayram 2 süre giriniz");

            RuleFor(t => t.AD).Length(1, 200).WithMessage("Tarife Adı 200 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 400).WithMessage("Açıklama 400 karakterden büyük olamaz");
        }
    }
}
