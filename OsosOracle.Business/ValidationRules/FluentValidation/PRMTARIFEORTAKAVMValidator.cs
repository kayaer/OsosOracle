using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class PRMTARIFEORTAKAVMValidator : AbstractValidator<PRMTARIFEORTAKAVM>
{
public PRMTARIFEORTAKAVMValidator()
{
RuleFor(t => t.AD).NotEmpty().WithMessage("AD giriniz");
RuleFor(t => t.YEDEKKREDI).NotEmpty().WithMessage("YEDEKKREDI giriniz");
RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
RuleFor(t => t.FIYAT1).NotEmpty().WithMessage("FIYAT1 giriniz");
RuleFor(t => t.FIYAT2).NotEmpty().WithMessage("FIYAT2 giriniz");
RuleFor(t => t.FIYAT3).NotEmpty().WithMessage("FIYAT3 giriniz");
RuleFor(t => t.FIYAT4).NotEmpty().WithMessage("FIYAT4 giriniz");
RuleFor(t => t.LIMIT1).NotEmpty().WithMessage("LIMIT1 giriniz");
RuleFor(t => t.LIMIT2).NotEmpty().WithMessage("LIMIT2 giriniz");

RuleFor(t => t.AD).Length(1, 200).WithMessage("AD 200 karakterden büyük olamaz");
RuleFor(t => t.ACIKLAMA).Length(1, 400).WithMessage("ACIKLAMA 400 karakterden büyük olamaz");
RuleFor(t => t.SAYACTARIH).Length(1, 15).WithMessage("SAYACTARIH 15 karakterden büyük olamaz");
}
}
}
