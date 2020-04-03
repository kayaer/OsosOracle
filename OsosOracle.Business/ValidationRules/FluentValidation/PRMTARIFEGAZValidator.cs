using FluentValidation;
using OsosOracle.Entities.Concrete;


namespace OsosOracle.Business.ValidationRules.FluentValidation
{
    public class PRMTARIFEGAZValidator : AbstractValidator<PRMTARIFEGAZ>
    {
        public PRMTARIFEGAZValidator()
        {
            RuleFor(t => t.TUKETIMLIMIT).NotEmpty().WithMessage("Tüketim limit  giriniz");
            RuleFor(t => t.SABAHSAAT).NotEmpty().WithMessage("Sabah saat giriniz");
            RuleFor(t => t.AKSAMSAAT).NotEmpty().WithMessage("Akşam saat giriniz");
            RuleFor(t => t.PULSE).NotEmpty().WithMessage("PULSE giriniz");
            RuleFor(t => t.KRITIKKREDI).NotEmpty().WithMessage("Kritik Kredi giriniz");
            RuleFor(t => t.YEDEKKREDI).NotEmpty().WithMessage("Yedek Kredi giriniz");
            RuleFor(t => t.BIRIMFIYAT).NotEmpty().WithMessage("Birim Fiyat giriniz");
            RuleFor(t => t.AD).NotEmpty().WithMessage("Ad giriniz");
            RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("Fiyat giriniz");
            RuleFor(t => t.AD).Length(1, 200).WithMessage("AD 200 karakterden büyük olamaz");
            RuleFor(t => t.SAYACCAP).NotEmpty().WithMessage("Sayaç Çap Giriniz");
            RuleFor(t => t.SAYACTUR).NotEmpty().WithMessage("Sayaç Tip Giriniz");
        }
    }
}
