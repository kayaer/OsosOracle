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
using System.Linq;
using OsosOracle.Framework.Aspects.TransactionAspects;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using System;

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
            _eNTABONEDal.Sil(idler);
        }

        [TransactionScopeAspect]
        public void AboneEkle(AboneIslemleri model)
        {
            //VAlidateler yapılacak
            Validate(model.ENTABONE);
            var eklenenAbone = Ekle(model.ENTABONE.List());


            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            model.ENTABONESAYAC.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
            aboneSayacService.Ekle(model.ENTABONESAYAC.List());
        }

        [TransactionScopeAspect]
        public void YesilVadiAboneEkle(YesilVadiAboneIslemleri model)
        {
            Validate(model.ENTABONE);
            var eklenenAbone = Ekle(model.ENTABONE.List());

            if (model.SuSayac.SAYACKAYITNO >= 0)
            {
                var aboneSayacService = ServisGetir<IENTABONESAYACService>();
                model.SuSayac.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
                model.SuSayac.OLUSTURAN = model.ENTABONE.OLUSTURAN;
                aboneSayacService.Ekle(model.SuSayac.List());
            }

            if (model.ElektrikSayac.SAYACKAYITNO >= 0)
            {
                var aboneSayacService = ServisGetir<IENTABONESAYACService>();
                model.ElektrikSayac.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
                model.ElektrikSayac.OLUSTURAN = model.ENTABONE.OLUSTURAN;
                aboneSayacService.Ekle(model.ElektrikSayac.List());
            }



        }

        [TransactionScopeAspect]
        public void AboneGuncelle(AboneIslemleri model)
        {
            //VAlidateler yapılacak
            Validate(model.ENTABONE);
            Guncelle(model.ENTABONE.List());

            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            aboneSayacService.Guncelle(model.ENTABONESAYAC.List());
        }

        public List<AboneAutoComplete> AutoCompleteBilgileriGetir(ENTABONEAra filtre = null)
        {
            return _eNTABONEDal.AutoCompleteBilgileriGetir(filtre);
        }

        [TransactionScopeAspect]
        public void AboneEkleAvm(AvmKaydetModel model)
        {
            //Validateler yapılacak
            Validate(model.ENTABONE);
            var eklenenAbone = Ekle(model.ENTABONE.List());


            var aboneSayacService = ServisGetir<IENTABONESAYACService>();
            model.SuSayac.ABONEKAYITNO = eklenenAbone.FirstOrDefault().KAYITNO;
            aboneSayacService.Ekle(model.SuSayac.List());

            aboneSayacService.Ekle(model.ElektrikSayac.List());
        }

        public AboneGenel AboneGenelBilgileriGetir(int aboneKayitNo)
        {
            return _eNTABONEDal.AboneGenelBilgileriGetir(aboneKayitNo);
        }
    }
}