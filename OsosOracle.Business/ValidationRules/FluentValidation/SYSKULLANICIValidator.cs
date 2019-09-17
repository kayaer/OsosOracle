using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class SYSKULLANICIValidator : AbstractValidator<SYSKULLANICI>
{
public SYSKULLANICIValidator()
{
RuleFor(t => t.KULLANICIAD).NotEmpty().WithMessage("KULLANICIAD giriniz");
RuleFor(t => t.SIFRE).NotEmpty().WithMessage("SIFRE giriniz");
RuleFor(t => t.BIRIMKAYITNO).NotEmpty().WithMessage("BIRIM giriniz");
RuleFor(t => t.VERSIYON).NotEmpty().WithMessage("VERSIYON giriniz");
RuleFor(t => t.DURUM).NotEmpty().WithMessage("DURUM giriniz");
RuleFor(t => t.SIFREKOD).NotEmpty().WithMessage("SIFREKOD giriniz");
RuleFor(t => t.DIL).NotEmpty().WithMessage("DIL giriniz");

RuleFor(t => t.KULLANICIAD).Length(1, 40).WithMessage("KULLANICIAD 40 karakterden büyük olamaz");
RuleFor(t => t.SIFRE).Length(1, 40).WithMessage("SIFRE 40 karakterden büyük olamaz");
RuleFor(t => t.AD).Length(1, 50).WithMessage("AD 50 karakterden büyük olamaz");
RuleFor(t => t.SOYAD).Length(1, 50).WithMessage("SOYAD 50 karakterden büyük olamaz");
RuleFor(t => t.SIFREKOD).Length(1, 10).WithMessage("SIFREKOD 10 karakterden büyük olamaz");
}
}
}
