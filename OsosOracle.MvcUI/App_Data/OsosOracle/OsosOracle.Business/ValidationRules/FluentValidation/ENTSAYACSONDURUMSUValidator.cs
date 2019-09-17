using FluentValidation;
using OsosOracle.Entities.Concrete;

namespace OsosOracle.Business.ValidationRules.FluentValidation
{
public class ENTSAYACSONDURUMSUValidator : AbstractValidator<ENTSAYACSONDURUMSU>
{
public ENTSAYACSONDURUMSUValidator()
{

RuleFor(t => t.ITERASYON).Length(1, 20).WithMessage("ITERASYON 20 karakterden büyük olamaz");
RuleFor(t => t.PILAKIM).Length(1, 12).WithMessage("PILAKIM 12 karakterden büyük olamaz");
RuleFor(t => t.PILVOLTAJ).Length(1, 12).WithMessage("PILVOLTAJ 12 karakterden büyük olamaz");
RuleFor(t => t.SONYUKLENENKREDITARIH).Length(1, 20).WithMessage("SONYUKLENENKREDITARIH 20 karakterden büyük olamaz");
RuleFor(t => t.ABONENO).Length(1, 20).WithMessage("ABONENO 20 karakterden büyük olamaz");
RuleFor(t => t.ABONETIP).Length(1, 20).WithMessage("ABONETIP 20 karakterden büyük olamaz");
RuleFor(t => t.ILKPULSETARIH).Length(1, 20).WithMessage("ILKPULSETARIH 20 karakterden büyük olamaz");
RuleFor(t => t.SONPULSETARIH).Length(1, 20).WithMessage("SONPULSETARIH 20 karakterden büyük olamaz");
RuleFor(t => t.BORCTARIH).Length(1, 20).WithMessage("BORCTARIH 20 karakterden büyük olamaz");
RuleFor(t => t.VANAACMATARIH).Length(1, 20).WithMessage("VANAACMATARIH 20 karakterden büyük olamaz");
RuleFor(t => t.VANAKAPAMATARIH).Length(1, 20).WithMessage("VANAKAPAMATARIH 20 karakterden büyük olamaz");
RuleFor(t => t.PARAMETRE).Length(1, 30).WithMessage("PARAMETRE 30 karakterden büyük olamaz");
RuleFor(t => t.ACIKLAMA).Length(1, 100).WithMessage("ACIKLAMA 100 karakterden büyük olamaz");
}
}
}
