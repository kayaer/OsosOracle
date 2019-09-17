using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTABONEComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Entities.ComplexType.ENTABONEBILGIComplexTypes;
using System.Linq;
using OsosOracle.Framework.Aspects.TransactionAspects;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;

namespace OsosOracle.Business.Concrete
{
    public class ENTABONEManager : BaseManager, IENTABONEService
    {
        private readonly IENTABONEDal _eNTABONEDal;
        public ENTABONEManager(IENTABONEDal eNTABONEDal)
        {
            _eNTABONEDal = eNTABONEDal;
        }

        public ENTABONE GetirById(int id)
        {
            return _eNTABONEDal.Getir(id);
        }

        public ENTABONEDetay DetayGetirById(int id)
        {
            return _eNTABONEDal.DetayGetir(id);
        }

        public List<ENTABONE> Getir(ENTABONEAra filtre)
        {
            return _eNTABONEDal.Getir(filtre);
        }

        public int KayitSayisiGetir(ENTABONEAra filtre)
        {
            return _eNTABONEDal.KayitSayisiGetir(filtre);
        }

        public List<ENTABONEDetay> DetayGetir(ENTABONEAra filtre)
        {
            return _eNTABONEDal.DetayGetir(filtre);
        }

        public ENTABONEDataTable Ara(ENTABONEAra filtre = null)
        {
            return _eNTABONEDal.Ara(filtre);
        }


        [FluentValidationAspect(typeof(ENTABONEValidator))]
        private void Validate(ENTABONE entity)
        {
            //Kontroller Yapılacak
        }

        public List<ENTABONE> Ekle(List<ENTABONE> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            return _eNTABONEDal.Ekle(entityler.ConvertEfList<ENTABONE, ENTABONEEf>());
        }

        public void Guncelle(List<ENTABONE> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _eNTABONEDal.Guncelle(entityler.ConvertEfList<ENTABONE, ENTABONEEf>());
        }

        [TransactionScopeAspect]
        public void Sil(List<int> idler)
        {
            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            var aboneSayac = aboneSayacService.Getir(new ENTABONESAYACAra { ABONEKAYITNO = idler[0] }).FirstOrDefault();
            if (aboneSayac != null)
            {
                aboneSayacService.Sil(aboneSayac.KAYITNO.List());
            }
            var aboneBilgiService = ServisGetir<IENTABONEBILGIService>();
            var aboneBilgi = aboneBilgiService.Getir(new ENTABONEBILGIAra { ABONEKAYITNO = idler[0] }).FirstOrDefault();
            if (aboneBilgi != null)
            {
                aboneBilgiService.Sil(aboneBilgi.KAYITNO.List());
            }

            _eNTABONEDal.Sil(idler);
        }

        [TransactionScopeAspect]
        public void AboneEkle(AboneIslemleri model)
        {
            //VAlidateler yapılacak
            Validate(model.ENTABONE);
            var eklenenAbone = Ekle(model.ENTABONE.List());
            var aboneBilgiService = ServisGetir<IENTABONEBILGIService>();
            model.ENTABONEBILGI.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
            aboneBilgiService.Ekle(model.ENTABONEBILGI.List());

            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            model.ENTABONESAYAC.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
            aboneSayacService.Ekle(model.ENTABONESAYAC.List());
        }

        [TransactionScopeAspect]
        public void AboneGuncelle(AboneIslemleri model)
        {
            //VAlidateler yapılacak
            Validate(model.ENTABONE);
            Guncelle(model.ENTABONE.List());
            var aboneBilgiService = ServisGetir<IENTABONEBILGIService>();
            aboneBilgiService.Guncelle(model.ENTABONEBILGI.List());
            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            aboneSayacService.Guncelle(model.ENTABONESAYAC.List());
        }

        public List<AboneAutoComplete> AutoCompleteBilgileriGetir(ENTABONEAra filtre = null)
        {
            return _eNTABONEDal.AutoCompleteBilgileriGetir(filtre);
        }
    }
}