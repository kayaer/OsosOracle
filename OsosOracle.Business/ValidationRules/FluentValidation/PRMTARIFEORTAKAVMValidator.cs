using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class PRMTARIFEKALORIMETREValidator : AbstractValidator<PRMTARIFEKALORIMETRE>
    {
        public PRMTARIFEKALORIMETREValidator()
        {
            RuleFor(t => t.AD).NotEmpty().WithMessage("Tarife Adı giriniz");
            //RuleFor(t => t.YEDEKKREDI).NotEmpty().WithMessage("Yedek Kredi giriniz");
            RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
            RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("Fiyat 1 giriniz");
            RuleFor(t => t.FIYAT2).NotEmpty().WithMessage("Fiyat 2 giriniz");
            RuleFor(t => t.FIYAT3).NotEmpty().WithMessage("Fiyat 3 giriniz");
            RuleFor(t => t.FIYAT4).NotEmpty().WithMessage("Fiyat 4 giriniz");
            RuleFor(t => t.LIMIT1).NotEmpty().WithMessage("Limit 1 giriniz");
            RuleFor(t => t.LIMIT2).NotEmpty().WithMessage("Limit 2 giriniz");         
            RuleFor(t => t.TUKETIMKATSAYI).NotEmpty().WithMessage("Tüketim Katsayı giriniz");
            RuleFor(t => t.KREDIKATSAYI).NotEmpty().WithMessage("Kredi Katsayı giriniz");
            RuleFor(t => t.BAYRAM1GUN).NotEmpty().WithMessage("BAYRAM1GUN giriniz");
            RuleFor(t => t.BAYRAM1AY).NotEmpty().WithMessage("BAYRAM1AY giriniz");
            RuleFor(t => t.BAYRAM1SURE).NotEmpty().WithMessage("BAYRAM1SURE giriniz");
            RuleFor(t => t.BAYRAM2GUN).NotEmpty().WithMessage("BAYRAM2GUN giriniz");
            RuleFor(t => t.BAYRAM2AY).NotEmpty().WithMessage("BAYRAM2AY giriniz");
            RuleFor(t => t.BAYRAM2SURE).NotEmpty().WithMessage("BAYRAM2SURE giriniz");
            RuleFor(t => t.AD).Length(1, 200).WithMessage("Tarife adı 200 karakterden büyük olamaz");
            RuleFor(t => t.ACIKLAMA).Length(1, 400).WithMessage("Açıklama 400 karakterden büyük olamaz");
           
        }
    }
}
